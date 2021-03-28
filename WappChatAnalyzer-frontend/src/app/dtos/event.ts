export class EventInfo {
    public id: number;
    public date: string;
    public name: string;
    public emoji: string;
    public groupName: string;
}

export class Event {
    public id: number;
    public date: string;
    public name: string;
    public emoji: string;
    public eventGroup: EventGroup;
}

export class EventGroup {
    public id: number;
    public name: string;
}