import { Component, Input, OnInit } from '@angular/core';
import { DataService } from '../services/data.service';

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

  public iconUrl: string;
  public showWappEmoji: boolean;
  public emojiUnicode: string;
  public emojiName: string;
  public showName: boolean;

  constructor(private dataService: DataService) { 

  }

  ngOnInit(): void {

  }

  render() {
    this.emojiUnicode = this._codePoints.split(' ').map(c => "&#x" + c + ";").join('');

    this.dataService.getEmojiByCodePoints(this._codePoints).subscribe(r => {
      let name = r.name.toLowerCase().split(' ').join("-");
      let codePoints = r.codePoints.toLowerCase().split(' ').join('-');
      
      this.iconUrl = this.emojiIconApiPoint + name + "_" + codePoints + ".png";


      this.emojiName = r.name;


      this.showWappEmoji = true;
    });
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

}
