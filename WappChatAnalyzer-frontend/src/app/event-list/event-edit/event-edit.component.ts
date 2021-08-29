import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { EventService } from 'src/app/services/event.service';
import { Event, EventGroup, EventInfo } from "../../dtos/event";
import { EmojiButton } from "@joeattardi/emoji-button";
import * as dateFormat from "dateformat";

@Component({
  selector: 'app-event-edit',
  templateUrl: './event-edit.component.html',
  styleUrls: ['./event-edit.component.css']
})
export class EventEditComponent implements OnInit {

  @Input()
  set id(value: number) {
    this.loadEvent(value);
  }

  @Output()
  onDone = new EventEmitter<{ saved: boolean, deleted: boolean, info: EventInfo }>();

  @Input()
  isNew: boolean;

  originalEvent: Event;
  event: Event;

  newEvent: Event;

  isDirty: boolean = false;

  dayPickerConfig = {
    format: "YYYY-MM-DD",
    firstDayOfWeek: "mo",
    opens: "left",
    closeOnSelectDelay: 0
  };

  emojiButton: EmojiButton = new EmojiButton({
    emojiSize: "40px",
    styleProperties: {
      "--category-button-active-color": "#D05353"
    }
  });

  constructor(private eventService: EventService) { }

  ngOnInit(): void {
    if (this.isNew) {
      var e = new Event();

      let date = new Date();
      date.setDate(date.getDate() - 1);

      e.date = dateFormat(date, "yyyy-mm-dd");
      
      this.loadedEvent(e);
    }

    this.emojiButton.on("emoji", selection => {
      this.event.emoji = selection.emoji;
      this.onPropertyChange("emoji", selection.emoji);
    });
  }

  loadedEvent(event: Event) {
    this.originalEvent = event;
    this.event = { ...this.originalEvent }
    this.newEvent = { ...this.originalEvent };
    this.isDirty = false;
  }

  loadEvent(id: number) {
    this.eventService.getEvent(id).subscribe(e => {
      this.loadedEvent(e);
    });
  }

  onPropertyChange(propertyName: string, value: any) {
    this.isDirty = true;
    this.newEvent[propertyName] = value;
  }

  onEmojiClick(event) {
    this.emojiButton.togglePicker(event.srcElement);
  }

  onSaveClick() {
    if (!this.isDirty)
      return;

    this.eventService.saveEvent(this.newEvent).subscribe(e => {
      this.loadedEvent(e);

      this.onDone.emit({
        saved: true,
        deleted: false,
        info: this.getInfoFromDTO(this.event)
      });
    });
  }

  onCancelClick() {
    this.onDone.emit({
      saved: false,
      deleted: false,
      info: this.getInfoFromDTO(this.originalEvent)
    });
  }

  onDeleteClick() {
    this.eventService.deleteEvent(this.originalEvent.id).subscribe(() => {
      this.onDone.emit({
        saved: false,
        deleted: true,
        info: this.getInfoFromDTO(this.originalEvent)
      });
    });
  }

  getInfoFromDTO(event: Event): EventInfo {
    var eInfo = new EventInfo();
    eInfo.id = event.id;
    eInfo.name = event.name;
    eInfo.date = event.date;
    eInfo.order = event.order;
    eInfo.emoji = event.emoji;
    eInfo.groupName = event.eventGroup?.name;
    return eInfo;
  }

  groupChanged(eventGroup: EventGroup) {
    this.onPropertyChange("eventGroup", eventGroup);
  }

  increaseOrder() {
    this.onPropertyChange("order", this.newEvent.order + 1);
  }

  decreaseOrder() {
    this.onPropertyChange("order", this.newEvent.order - 1);
  }
}
