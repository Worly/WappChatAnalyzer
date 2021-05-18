export class BasicInfoTotal {
    
    public totalNumberOfMessages: StatisticTotal;
    public totalNumberOfWords: StatisticTotal;
    public totalNumberOfCharacters: StatisticTotal;
    public totalNumberOfMedia: StatisticTotal;
    public totalNumberOfEmojis: StatisticTotal;
}

export class StatisticTotal {
    public total: number;
    public totalForSenders: { [Key: string]: number }
}