import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../services/data.service';
import { EmojiService } from '../services/emoji.service';
import { ToastComponent } from '../toast/toast.component';

@Component({
  selector: 'app-emoji',
  templateUrl: './emoji.component.html',
  styleUrls: ['./emoji.component.css']
})
export class EmojiComponent implements OnInit {
  private _codePoints: string;
  @Input()
  public set codePoints(value: string) {
    this._codePoints = value;
    this.render();
  }

  @Input()
  public size: number = 50;

  @Input()
  public interactible: boolean = true;

  @ViewChild(ToastComponent) toast: ToastComponent;

  public iconUrl: string;
  public showWappEmoji: boolean;
  public emojiHtml: string;
  public emojiName: string;
  public showName: boolean;

  constructor(private emojiService: EmojiService) {

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

      this.emojiService.getEmojiByCodePoints(this._codePoints).subscribe(r => {
        this.iconUrl = this.emojiService.getLinkToWappEmojiImage(r)

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
    if (!this.interactible)
      return;

    var emoji = String.fromCodePoint(...this._codePoints.split(' ').map(o => parseInt(o, 16)))
    navigator.clipboard.writeText(emoji);
    event.stopPropagation();
    this.toast.show(emoji + " copied to clipboard");
  }
}
