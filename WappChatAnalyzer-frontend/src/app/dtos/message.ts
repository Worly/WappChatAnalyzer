export class Message {
    public id: number;
    public sentDateTime: string;
    public sender: string;
    public text: string;
    public isMedia: boolean;

    public nativeElement;
    public emojifiedText: string;
}