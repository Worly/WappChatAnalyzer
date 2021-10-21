import { Sender } from "./sender";

export class StatisticTotal {
    public total: number;
    public senders: { [Key: number]: Sender }
    public totalForSenders: { [Key: number]: number }
}