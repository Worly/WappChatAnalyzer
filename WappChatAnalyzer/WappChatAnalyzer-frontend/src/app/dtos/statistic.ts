import { Sender } from "./sender";

export class Statistic {
    public statisticName: string;

    public filter: { fromDate: Date, toDate: Date, groupingPeriod: string };

    public senders: { [Key: number]: Sender }

    public total: number;
    public totalBySenders: { [Key: number]: number };

    public timePeriods: string[];
    public valuesBySendersOnTimePeriods: { [Key: number]: number[]; };
    public totalsOnTimePeriods: number[];
}