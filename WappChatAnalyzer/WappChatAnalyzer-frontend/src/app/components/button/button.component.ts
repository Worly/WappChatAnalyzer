import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IconName, IconPrefix } from '@fortawesome/fontawesome-svg-core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss']
})
export class ButtonComponent implements OnInit {

  @Input()
  text: string;

  @Input()
  curved: boolean = true;

  @Input() curvedTopLeft: boolean = false;
  @Input() curvedTopRight: boolean = false;
  @Input() curvedBottomLeft: boolean = false;
  @Input() curvedBottomRight: boolean = false;

  @Input()
  look: "solid" | "outlined" | "text" = "text";

  @Input()
  color: "success" | "danger" | "normal" = "normal";

  @Input()
  disabled: boolean = false;

  @Input()
  width: string;

  @Input()
  iconPrefix: IconPrefix = "fas";

  @Input()
  icon: IconName;

  @Output()
  onClick = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

  onClicked(): void {
    if (this.disabled)
      return;

    this.onClick.emit();
  }

}
