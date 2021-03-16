export class Statistic
{
    public statisticName: string;

    public total: number;
    public totalBySenders: { [Key: string]: number };

    public dates: string[];
    public valuesBySendersOnDates: { [Key: string]: number[]; };
}