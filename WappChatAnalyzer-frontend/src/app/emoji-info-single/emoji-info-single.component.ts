import { Component, Input, OnInit } from '@angular/core';
import { EmojiInfo } from '../dtos/emojiInfoTotal';

@Component({
  selector: 'app-emoji-info-single',
  templateUrl: './emoji-info-single.component.html',
  styleUrls: ['./emoji-info-single.component.css']
})
export class EmojiInfoSingleComponent implements OnInit {
  public _emojiInfo: EmojiInfo;
  @Input()
  public set emojiInfo(value: EmojiInfo) {
    this._emojiInfo = value;
    this.render();
  }

  constructor() { }

  ngOnInit(): void {
  }

  render() {

  }
}
