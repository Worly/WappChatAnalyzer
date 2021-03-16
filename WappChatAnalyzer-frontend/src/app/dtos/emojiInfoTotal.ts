export class EmojiInfoTotal {
    public emojiInfos: EmojiInfo[];
}

export class EmojiInfo {
    public emojiCodePoints: string;
    public total: number;
    public bySenders: { [Key: string]: number };
}