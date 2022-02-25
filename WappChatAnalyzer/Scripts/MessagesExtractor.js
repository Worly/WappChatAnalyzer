
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function getTextFromElement(element) {
    let result = "";

    for (let el of element.childNodes) {
        if (el.nodeType == 3) // TEXT_NODE
        {
            result += el.nodeValue
            continue;
        }
        else if (el.nodeType == 1) //ELEMENT_NODE
        {
            if (el.nodeName == "IMG") {
                if (el.getAttribute("class").includes("copyable-text")) { // emoji
                    result += el.getAttribute("alt");
                    continue;
                }
                else if (el.getAttribute("class") == "_2UdhN _1xeoG _1jJBG i0jNr") // big heart animation
                    continue;
            }
            else if (el.nodeName == "A") { // link
                if (el.nodeValue == null && el.childNodes.length > 0)
                    result += getTextFromElement(el);
                else
                    result += el.nodeValue
                continue;
            }
            else {
                result += getTextFromElement(el);
                continue;
            }
        }

        console.log("Cannot get text from element", element, " Unknown at ", el);
    }

    return result;
}

function tryAsTextMessage(messageElement) {
    let firstChild = messageElement.querySelector(".cvjcv._1Ilru");
    if (firstChild == null)
        return null;

    let secondChild = firstChild.querySelector(".Nm1g1._22AX6");
    if (secondChild == null)
        return null;

    let thirdChild = secondChild.querySelector("._22Msk");
    if (thirdChild == null)
        return null;

    let fourthChild = thirdChild.querySelector(".copyable-text");
    if (fourthChild == null)
        return null;

    let dataPrePlainText = fourthChild.getAttribute("data-pre-plain-text");

    let fifthChild = fourthChild.querySelector("._1Gy50");
    if (fifthChild == null)
        return null;

    let sixthChild = fifthChild.querySelector(".i0jNr.selectable-text.copyable-text");
    if (sixthChild == null)
        return null;

    let text = getTextFromElement(sixthChild);

    return [{
        data: dataPrePlainText,
        text: text
    }];
}

function tryAsVoiceMessage(messageElement) {
    let firstChild = messageElement.querySelector(".cvjcv.JZd-w");
    if (firstChild == null)
        return null;

    let secondChild = firstChild.querySelector(".Nm1g1._22AX6");
    if (secondChild == null)
        return null;

    let sender = secondChild.childNodes[0].getAttribute("aria-label");

    let timeElement = secondChild.querySelector(".l7jjieqr.fewfhwl7");
    if (timeElement == null)
        return null;

    let time = timeElement.textContent;

    return [{
        sender: sender,
        time: time,
        text: "<Media omitted>",
        type: "VOICE_MESSAGE"
    }];
}

function tryAsImageMessage(messageElement) {
    let textMessage;

    let firstChild = messageElement.querySelector(".cvjcv._3QK-g");
    if (firstChild == null)
        return null;

    let secondChild = firstChild.querySelector(".Nm1g1._22AX6");
    if (secondChild == null)
        return null;

    let dataChild = secondChild.querySelector("[data-pre-plain-text]");
    if (dataChild != null) {
        let dataPrePlainText = dataChild.getAttribute("data-pre-plain-text");

        let textChild = dataChild.querySelector(".i0jNr.selectable-text.copyable-text");
        if (textChild != null) {
            let text = getTextFromElement(textChild);

            textMessage = {
                data: dataPrePlainText,
                text: text
            };
        }
    }

    let sender = secondChild.childNodes[0].getAttribute("aria-label");

    if (sender == null) {
        if (messageElement.getAttribute("class").includes("message-in"))
            sender = "Her:";
        else
            sender = "You:";
    }

    let timeElement = secondChild.querySelector(".l7jjieqr.fewfhwl7");
    if (timeElement == null)
        return null;

    let time = timeElement.textContent;

    if (textMessage) {
        return [
            textMessage,
            {
                data: textMessage.data,
                text: "<Media omitted>",
                type: "IMAGE_MESSAGE"
            }
        ];
    }
    else {
        return [{
            sender: sender,
            time: time,
            text: "<Media omitted>",
            type: "IMAGE_MESSAGE"
        }];
    }
}

function tryAsLiveLocationMessage(messageElement) {
    let firstChild = messageElement.querySelector(".cvjcv._3zfPb");
    if (firstChild == null)
        return null;

    let dataChild = firstChild.querySelector("[data-pre-plain-text]");
    if (dataChild == null)
        return null;
    let dataPrePlainText = dataChild.getAttribute("data-pre-plain-text");

    return [{
        data: dataPrePlainText,
        text: "live location shared",
        type: "LIVE_LOCATION_MESSAGE"
    }];
}

function tryAsAttachmentMessage(messageElement) {
    let firstChild = messageElement.querySelector(".cvjcv._1CJ4I");
    if (firstChild == null)
        return null;

    let secondChild = firstChild.querySelector(".Nm1g1._22AX6");
    if (secondChild == null)
        return null;

    let sender = secondChild.childNodes[0].getAttribute("aria-label");

    let timeElement = secondChild.querySelector(".l7jjieqr.fewfhwl7");
    if (timeElement == null)
        return null;

    let time = timeElement.textContent;

    return [{
        sender: sender,
        time: time,
        text: "<Media omitted>",
        type: "ATTACHMENT_MESSAGE"
    }];
}

function tryAsLocationMessage(messageElement) {
    let firstChild = messageElement.querySelector(".cvjcv._3Y4UU");
    if (firstChild == null)
        return null;

    let dataChild = firstChild.querySelector("[data-pre-plain-text]");
    if (dataChild == null)
        return null;
    let dataPrePlainText = dataChild.getAttribute("data-pre-plain-text");

    return [{
        data: dataPrePlainText,
        text: "<Media omitted>",
        type: "LOCATION_MESSAGE"
    }];
}

function getMessagesFromElement(messageElement) {
    let messages = tryAsTextMessage(messageElement);
    if (messages != null)
        return messages;

    messages = tryAsVoiceMessage(messageElement);
    if (messages != null)
        return messages;

    messages = tryAsImageMessage(messageElement);
    if (messages != null)
        return messages;

    messages = tryAsLiveLocationMessage(messageElement);
    if (messages != null)
        return messages;

    messages = tryAsAttachmentMessage(messageElement);
    if (messages != null)
        return messages;

    messages = tryAsLocationMessage(messageElement);
    if (messages != null)
        return messages;

    console.log("Unknown at", messageElement);

    return [];
}

function cleanUp(messages) {
    let newMessages = [];

    for (let msg of messages) {
        if (msg == null)
            console.log("NULL in messages!", messages);

        let newMsg = {};

        if (msg.data) {
            var split1 = msg.data.split("] ");
            newMsg.sender = split1[1];

            split2 = split1[0].split(", ");
            newMsg.date = split2[1];
            newMsg.time = split2[0].substring(1);
        }
        else {
            newMsg.sender = msg.sender + " ";
            newMsg.date = newMessages[newMessages.length - 1].date;
            newMsg.time = msg.time
        }

        if (newMsg.sender == "You: ")
            newMsg.sender = "Valentino Vukelic: ";
        else if (newMsg.sender == "Her: ")
            newMsg.sender = "Lara <3: ";

        newMsg.text = msg.text;

        newMessages.push(newMsg);
    }

    return newMessages;
}

function serialize(cleanedMessages) {
    let result = "";

    for (let i = cleanedMessages.length - 1; i >= 0; i--) {
        result += cleanedMessages[i].date + ", " + cleanedMessages[i].time + " - " + cleanedMessages[i].sender + cleanedMessages[i].text + "\n";
    }

    return result;
}

var stopped = false;
async function extractMessages(count, deleteCompleted) {
    stopped = false;

    let messages = [];

    let message = null;

    let startTime = performance.now();

    for (let i = 0; i < count; i++) {
        if (stopped) {
            console.log("STOPPED");
            return;
        }

        let elements = [...document.querySelectorAll(".message-in, .message-out")];

        if (message == null)
            message = elements[elements.length - 1];
        else {
            let index;
            do {
                index = elements.indexOf(message);
                if (index == 0) {
                    console.log("Waiting");
                    await sleep(10);
                    elements = [...document.querySelectorAll(".message-in, .message-out")];
                }
            } while (index == 0);

            if (deleteCompleted)
                message.remove();

            message = elements[index - 1];
        }
        message.scrollIntoView();

        messages.push(...getMessagesFromElement(message));

        if (i > 0 && i % 100 == 0) {
            let elapsedMs = performance.now() - startTime;
            let elapsedMin = elapsedMs / 1000 / 60;

            let percentage = i / count;

            let expected = elapsedMin / percentage;

            console.log("Elapsed mins ", elapsedMin, " expected mins: ", expected, " percentage ", percentage * 100 + "%");
        }
    }
    console.log("DONE")

    let cleanedMessages = cleanUp(messages);

    let newWindow = window.open();
    let pre = newWindow.document.createElement("pre");
    pre.appendChild(newWindow.document.createTextNode(serialize(cleanedMessages)));
    newWindow.document.body.appendChild(pre);
}

function stopExtraction() {
    stopped = true;
}

extractMessages(1000, false);