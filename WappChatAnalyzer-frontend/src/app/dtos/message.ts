import { Sender } from "./sender";

export class Message {
    public id: number;
    public sentDateTime: string;
    public sender: Sender;
    public text: string;
    public isMedia: boolean;

    public nativeElement;
    public emojifiedText: string;
}