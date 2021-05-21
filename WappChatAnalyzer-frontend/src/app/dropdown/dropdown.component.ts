import { Component, ElementRef, EventEmitter, HostListener, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-dropdown',
  templateUrl: './dropdown.component.html',
  styleUrls: ['./dropdown.component.css']
})
export class DropdownComponent implements OnInit {

  private _value: any;
  @Input()
  set value(value: any) {
    this._value = value;
  }
  get value(): any {
    return this._value;
  }
  @Output()
  valueChange = new EventEmitter<any>();

  @Input()
  items: any[];

  @Input()
  displayProperty: string;

  pickerVisible: boolean = false;

  constructor(private elementRef: ElementRef) { }

  ngOnInit(): void {
    
  }

  togglePicker() {
    this.pickerVisible = !this.pickerVisible;
  }

  select(item: any) {
    if (this._value != item) {
      this._value = item;
      this.valueChange.emit(this._value);
    }
    this.pickerVisible = false;
  }

  display(item: any) {
    if(this.displayProperty == null)
      return item;
    else
      return item[this.displayProperty];
  }

  @HostListener('document:mousedown', ['$event'])
  onGlobalClick(event): void {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.pickerVisible = false;
    }
  }

}
