export class EventInfo {
    public id: number;
    public date: string;
    public order: number;
    public name: string;
    public emoji: string;
    public groupName: string;
}

export class Event {
    public id: number;
    public date: string;
    public order: number;
    public name: string;
    public emoji: string;
    public eventGroup: EventGroup;
}

export class EventTemplate {
    public name: string;
    public emoji: string;
    public eventGroupId: number;
    public eventGroupName: string;
}

export class EventGroup {
    public id: number;
    public name: string;
}