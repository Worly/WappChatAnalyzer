import { Component, ElementRef, EventEmitter, HostListener, Input, OnInit, Output } from '@angular/core';
import { EventGroup } from 'src/app/dtos/event';
import { EventService } from 'src/app/services/event.service';

@Component({
  selector: 'app-event-group-picker',
  templateUrl: './event-group-picker.component.html',
  styleUrls: ['./event-group-picker.component.css']
})
export class EventGroupPickerComponent implements OnInit {

  private _value: EventGroup;
  @Input()
  set value(value: EventGroup) {
    this._value = value;
  }
  get value(): EventGroup {
    return this._value;
  }
  @Output()
  valueChange = new EventEmitter<EventGroup>();

  eventGroups: EventGroup[] = [];

  pickerVisible: boolean = false;
  isLoading: boolean = false;

  constructor(private eventService: EventService, private elementRef: ElementRef) { }

  ngOnInit(): void {
    this.isLoading = true;
    this.eventService.getEventGroups().subscribe(r => {
      this.eventGroups = r;
      this.isLoading = false;
    })
  }

  togglePicker() {
    this.pickerVisible = !this.pickerVisible;
  }

  select(eventGroup: EventGroup) {
    this._value = eventGroup;
    this.valueChange.emit(this._value);
    this.pickerVisible = false;
  }

  @HostListener('document:mousedown', ['$event'])
  onGlobalClick(event): void {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.pickerVisible = false;
    }
  }

}
