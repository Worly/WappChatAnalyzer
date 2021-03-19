import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../services/data.service';
import { ToastComponent } from '../toast/toast.component';

@Component({
  selector: 'app-emoji',
  templateUrl: './emoji.component.html',
  styleUrls: ['./emoji.component.css']
})
export class EmojiComponent implements OnInit {

  private readonly emojiIconApiPoint = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/72/whatsapp/273/";

  private _codePoints: string;
  @Input()
  public set codePoints(value: string) {
    this._codePoints = value;
    this.render();
  }

  @ViewChild(ToastComponent) toast: ToastComponent;

  public iconUrl: string;
  public showWappEmoji: boolean;
  public emojiHtml: string;
  public emojiName: string;
  public showName: boolean;

  constructor(private dataService: DataService) {

  }

  ngOnInit(): void {

  }

  render() {
    if (this._codePoints == null) {
      this.emojiHtml = null;
      this.iconUrl = null;
      this.emojiName = null;
      this.showWappEmoji = true;
    }
    else {
      this.emojiHtml = this._codePoints.split(' ').map(c => "&#x" + c + ";").join('');

      this.dataService.getEmojiByCodePoints(this._codePoints).subscribe(r => {
        let name = r.name.toLowerCase().split(' ').join("-");
        let codePoints = r.codePoints.toLowerCase().split(' ').join('-');

        this.iconUrl = this.emojiIconApiPoint + name + "_" + codePoints + ".png";


        this.emojiName = r.name;


        this.showWappEmoji = true;
      });
    }
  }

  imgLoadingError() {
    this.showWappEmoji = false;
  }

  mouseEnter() {
    this.showName = true;
  }

  mouseLeave() {
    this.showName = false;
  }

  click(event: Event) {
    var emoji = String.fromCodePoint(...this._codePoints.split(' ').map(o => parseInt(o, 16)))
    navigator.clipboard.writeText(emoji);
    event.stopPropagation();
    this.toast.show(emoji + " copied to clipboard");
  }
}
