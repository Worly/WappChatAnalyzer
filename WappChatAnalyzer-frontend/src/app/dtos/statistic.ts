export class Statistic {
    public statisticName: string;

    public filter: { fromDate: Date, toDate: Date, groupingPeriod: string };

    public total: number;
    public totalBySenders: { [Key: string]: number };

    public timePeriods: string[];
    public valuesBySendersOnTimePeriods: { [Key: string]: number[]; };
}