export class BasicInfoTotal {
    public basicInfo: BasicInfo;
    public basicInfoForSenders: { [Key: string]: BasicInfo }
}

class BasicInfo {
    public totalNumberOfMessages: number;
    public totalNumberOfWords: number;
    public totalNumberOfCharacters: number;
    public totalNumberOfMedia: number;
    public averageMessageLengthInCharacters: number;
    public averageMessageLengthInWords: number;
}