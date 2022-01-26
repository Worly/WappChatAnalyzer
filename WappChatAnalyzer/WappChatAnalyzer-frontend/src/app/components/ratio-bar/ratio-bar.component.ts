import { Component, Input, OnInit } from '@angular/core';
import { appConfig } from '../../app.config';

@Component({
  selector: 'app-ratio-bar',
  templateUrl: './ratio-bar.component.html',
  styleUrls: ['./ratio-bar.component.css']
})
export class RatioBarComponent implements OnInit {

  @Input()
  public items: { [Key: string]: number };

  @Input()
  public height: number;

  public appConfig = appConfig;

  constructor() { }

  ngOnInit(): void {
  }

  getItems() {
    let result = {};
    
    for(let key in this.items) {
      if (this.items[key] > 0)
        result[key] = this.items[key];
    }

    return result;
  }

  total(): number {
    return Object.values(this.items).reduce((f, s) => f + s);
  }

  invertColor(hex, bw) {
    if (hex.indexOf('#') === 0) {
      hex = hex.slice(1);
    }
    // convert 3-digit hex to 6-digits.
    if (hex.length === 3) {
      hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
    }
    if (hex.length !== 6) {
      throw new Error('Invalid HEX color.');
    }
    var r = parseInt(hex.slice(0, 2), 16),
      g = parseInt(hex.slice(2, 4), 16),
      b = parseInt(hex.slice(4, 6), 16);
    if (bw) {
      // http://stackoverflow.com/a/3943023/112731
      return (r * 0.299 + g * 0.587 + b * 0.114) > 186
        ? '#000000'
        : '#FFFFFF';
    }
    // invert color components
    let rOut = (255 - r).toString(16);
    let gOut = (255 - g).toString(16);
    let bOut = (255 - b).toString(16);
    // pad each with zeros and return
    return "#" + this.padZero(rOut, rOut.length) + this.padZero(gOut, gOut.length) + this.padZero(bOut, bOut.length);
  }

  padZero(str, len) {
    len = len || 2;
    var zeros = new Array(len).join('0');
    return (zeros + str).slice(-len);
  }

}
