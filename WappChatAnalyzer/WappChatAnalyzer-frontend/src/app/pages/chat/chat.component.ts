import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { Message } from '../../dtos/message';
import { EmojiService } from '../../services/emoji.service';
import { MessagesService } from '../../services/messages.service';
import GraphemeSplitter from 'grapheme-splitter';
import { Sender } from '../../dtos/sender';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, AfterViewInit {
  readonly LOAD_COUNT = 30;
  readonly MAX_NUMBER_OF_MESSAGES = 120;

  @ViewChild("chat")
  chat: ElementRef;

  me: Sender;
  senders: Sender[] = [];
  messages: Message[] = [];

  dateConfig = {
    format: "YYYY-MM-DD",
    firstDayOfWeek: "mo",
    closeOnSelectDelay: 0
  };

  private maxLoadedId: number;
  private minLoadedId: number;

  private nothingToLoadMoreLater: boolean = false;
  private nothingToLoadMoreEarlier: boolean = false;

  suspendAutoLoading: boolean = false;
  suspendIncreaseDecreaseDate: boolean = false;

  constructor(private messagesService: MessagesService, private emojiService: EmojiService, private changeDetectorRef: ChangeDetectorRef) {

  }

  ngOnInit(): void {
    this.messagesService.getAllSenders().subscribe(s => {
      this.senders = s;
      this.me = this.senders[0];
    });
    this.messagesService.getLastMessageId().subscribe(r => {
      this.loadFirstMessages(r - this.LOAD_COUNT, r, () => this.scrollToBottom());
    });
  }

  ngAfterViewInit() {
  }

  private getDistanceFromTop(message: Message): number {
    return message.nativeElement?.offsetTop - this.chat.nativeElement?.scrollTop;
  }

  private setScrollToSetDistanceFromTop(message: Message, distanceFromTop: number) {
    this.chat.nativeElement.scrollTop = message.nativeElement.offsetTop - distanceFromTop;
  }

  private getDistanceFromBottom(message: Message): number {
    return this.chat.nativeElement.offsetHeight - (message.nativeElement.offsetTop - this.chat.nativeElement.scrollTop)
  }

  private setScrollToSetDistanceFromBottom(message: Message, distanceFromBottom: number) {
    this.chat.nativeElement.scrollTop = distanceFromBottom - this.chat.nativeElement.offsetHeight + message.nativeElement.offsetTop;
  }

  private scrollToBottom() {
    this.chat.nativeElement.scrollTop = this.chat.nativeElement.scrollHeight - this.chat.nativeElement.offsetHeight;
  }

  getFirstVisibleMessage(): Message {
    for (let msg of this.messages) {
      if (this.getDistanceFromTop(msg) + msg.nativeElement?.offsetHeight >= 15)
        return msg;
    }

    return null;
  }

  private getLastVisibleMessage(): Message {
    for (let i = this.messages.length - 1; i >= 0; i--) {
      let msg = this.messages[i];
      if (this.getDistanceFromBottom(msg) >= 0.5 * msg.nativeElement?.offsetHeight)
        return msg;
    }

    return null;
  }

  private loadFirstMessages(fromId: number, toId: number, onLoaded: () => void) {
    this.messages = [];
    this.nothingToLoadMoreEarlier = this.nothingToLoadMoreLater = false;

    this.suspendAutoLoading = true;
    this.messagesService.getMessages(fromId, toId).subscribe(r => {
      this.suspendAutoLoading = false;
      this.insertNewMessages(r);
      onLoaded();
    });
  }

  private loadMoreEarlier() {
    if (this.nothingToLoadMoreEarlier)
      return;
    this.messagesService.getMessages(this.minLoadedId - this.LOAD_COUNT, this.minLoadedId - 1).subscribe(r => {
      if (r.length == 0)
        this.nothingToLoadMoreEarlier = true;
      else
        this.insertNewMessages(r);
    });
  }

  private loadMoreLater() {
    if (this.nothingToLoadMoreLater)
      return;
    this.messagesService.getMessages(this.maxLoadedId + 1, this.maxLoadedId + this.LOAD_COUNT).subscribe(r => {
      if (r.length == 0)
        this.nothingToLoadMoreLater = true;
      else
        this.insertNewMessages(r);
    });
  }

  private insertNewMessages(newMessages: Message[]) {
    for (let msg of newMessages)
      this.emojify(msg.text).subscribe(r => msg.emojifiedText = r);

    if (this.messages.length == 0 || newMessages.every(o => o.id < this.messages[0].id)) {
      let firstMessage = this.getFirstVisibleMessage();
      if (firstMessage != null)
        var distanceFromTop = this.getDistanceFromTop(firstMessage);

      this.messages.unshift(...newMessages);
      if (this.messages.length > this.MAX_NUMBER_OF_MESSAGES) {
        this.messages.splice(this.messages.length - this.LOAD_COUNT, this.LOAD_COUNT);
        this.nothingToLoadMoreLater = false;
      }

      this.changeDetectorRef.detectChanges();

      if (firstMessage != null)
        this.setScrollToSetDistanceFromTop(firstMessage, distanceFromTop);
    }
    else if (newMessages.every(o => o.id > this.messages[this.messages.length - 1].id)) {
      let lastMessage = this.getLastVisibleMessage();
      if (lastMessage != null)
        var distanceFromBottom = this.getDistanceFromBottom(lastMessage);

      this.messages.push(...newMessages);
      if (this.messages.length > this.MAX_NUMBER_OF_MESSAGES) {
        this.messages.splice(0, this.LOAD_COUNT);
        this.nothingToLoadMoreEarlier = false;
      }

      this.changeDetectorRef.detectChanges();

      if (lastMessage != null)
        this.setScrollToSetDistanceFromBottom(lastMessage, distanceFromBottom);
    }
    else
      throw "This is not implemented";


    this.minLoadedId = Math.min(...this.messages.map(o => o.id));
    this.maxLoadedId = Math.max(...this.messages.map(o => o.id));
  }

  bindElementToMessage(event: ElementRef, index: number) {
    this.messages[index].nativeElement = event.nativeElement;
  }

  onScroll() {
    if (this.suspendAutoLoading)
      return;

    if (this.chat.nativeElement.scrollTop == 0)
      this.loadMoreEarlier();
    if (this.chat.nativeElement.scrollTop + this.chat.nativeElement.offsetHeight >= this.chat.nativeElement.scrollHeight)
      this.loadMoreLater();
  }

  isNotLastInGroup(index: number): boolean {
    if (this.messages.length <= index + 1)
      return false;

    return this.messages[index].sender.id == this.messages[index + 1].sender.id;
  }

  isFirstInGroup(index: number): boolean {
    return index == 0 || this.messages[index - 1].sender.id != this.messages[index].sender.id;
  }

  isMessageMine(message: Message): boolean {
    return this.me != null && message.sender != null && message.sender.id == this.me.id;
  }

  getCurrentDate() {
    return this.getFirstVisibleMessage()?.sentDateTime;
  }

  increaseDate() {
    if (this.getFirstVisibleMessage() == null || this.suspendIncreaseDecreaseDate)
      return;

    var currentDate = new Date(this.getCurrentDate());
    currentDate.setDate(currentDate.getDate() + 1);
    this.suspendIncreaseDecreaseDate = true;
    this.messagesService.getFirstMessageOfDayAfter(currentDate).subscribe((msg: Message) => {
      this.suspendIncreaseDecreaseDate = false;
      if (msg == null)
        return;
      this.loadFirstMessages(msg.id - this.LOAD_COUNT / 2, msg.id + this.LOAD_COUNT, () => {
        this.focusOnMessage(this.messages.find(o => o.id == msg.id));
      });
    });
  }

  decreaseDate() {
    if (this.getFirstVisibleMessage() == null || this.suspendIncreaseDecreaseDate)
      return;

    var currentDate = new Date(this.getCurrentDate());
    this.suspendIncreaseDecreaseDate = true;
    this.messagesService.getFirstMessageOfDayBefore(currentDate).subscribe((msg: Message) => {
      this.suspendIncreaseDecreaseDate = false;
      if (msg == null)
        return;
      this.loadFirstMessages(msg.id - this.LOAD_COUNT / 2, msg.id + this.LOAD_COUNT, () => {
        this.focusOnMessage(this.messages.find(o => o.id == msg.id));
      });
    });
  }

  focusOnMessage(message: Message) {
    this.setScrollToSetDistanceFromTop(message, 4);
  }

  onDateSelected(event) {
    if (this.getFirstVisibleMessage() == null || this.suspendIncreaseDecreaseDate)
      return;

    this.suspendIncreaseDecreaseDate = true;
    this.messagesService.getFirstMessageOfDayAfter(event).subscribe((msg: Message) => {
      this.suspendIncreaseDecreaseDate = false;
      if (msg == null)
        return;
      this.loadFirstMessages(msg.id - this.LOAD_COUNT / 2, msg.id + this.LOAD_COUNT, () => {
        this.focusOnMessage(this.messages.find(o => o.id == msg.id));
      });
    });
  }

  emojify(text: string): Observable<string> {
    return new Observable<string>(observer => {
      let html = [];
      let emojiRegexExpr = /\p{Extended_Pictographic}/u;

      let subscriptions = 0;
      let completedSubscriptions = 0;
      let madeAllSubscriptions = false;

      let splitter = new GraphemeSplitter();

      for (let char of splitter.splitGraphemes(text)) {
        if (char == "<")
          html.push("&#60;");
        else
          html.push(char);
        if (emojiRegexExpr.test(char)) {
          let codePoints = Array.from(char)
            .map((v) => v.codePointAt(0).toString(16).toUpperCase())
            .join(" ");

          subscriptions++;
          this.emojiService.getEmojiByCodePoints(codePoints).subscribe(r => {
            let url = this.emojiService.getLinkToWappEmojiImage(r);

            var img = "";
            img = img.concat("<img class=\"emoji\" src=\"");
            img = img.concat(url);
            img = img.concat("\" alt=\"");
            img = img.concat(char);
            img = img.concat("\" />");

            for (let j = 0; j < html.length; j++) {
              if (html[j] == char)
                html[j] = img;
            }

            completedSubscriptions++;
            if (madeAllSubscriptions && completedSubscriptions == subscriptions) {
              observer.next(html.join(""));
              observer.complete();
            }
          });
        }
      }

      madeAllSubscriptions = true;
      if (madeAllSubscriptions && completedSubscriptions == subscriptions) {
        observer.next(html.join(""));
        observer.complete();
      }
      return { unsubscribe() { } };
    });
  }
}
