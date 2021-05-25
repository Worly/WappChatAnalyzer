import { Sender } from "./sender";

export class EmojiInfoTotal {
    public emojiInfos: EmojiInfo[];
}

export class EmojiInfo {
    public emojiCodePoints: string;
    public total: number;
    public senders: { [Key: number]: Sender}
    public bySenders: { [Key: number]: number };
}