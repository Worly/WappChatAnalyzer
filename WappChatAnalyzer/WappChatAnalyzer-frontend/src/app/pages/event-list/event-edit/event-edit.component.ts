import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { EventService } from 'src/app/services/event.service';
import { Event, EventGroup, EventInfo, EventTemplate } from "../../../dtos/event";
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

  @Input()
  template: EventTemplate;

  originalEvent: Event;
  event: Event;

  newEvent: Event;

  isDirty: boolean = false;

  isSaving: boolean;
  isDeleting: boolean;

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

      if (this.template != null) {
        e.emoji = this.template.emoji;
        e.name = this.template.name;
        if (this.template.eventGroupId != null) {
          var eventGroup = new EventGroup();
          eventGroup.id = this.template.eventGroupId;
          eventGroup.name = this.template.eventGroupName;
          e.eventGroup = eventGroup;
        }
      }

      this.loadedEvent(e);

      if (this.template != null)
        this.isDirty = true;
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

    //DIRTY FIX
    if (propertyName == "date")
      value = dateFormat(value, "yyyy-mm-dd");

    this.isDirty = true;
    this.newEvent[propertyName] = value;
  }

  onEmojiClick(event) {
    this.emojiButton.togglePicker(event.srcElement);
  }

  onSaveClick() {
    if (!this.isDirty)
      return;

    this.isSaving = true;
    this.eventService.saveEvent(this.newEvent).subscribe(e => {
      this.loadedEvent(e);

      this.isSaving = false;

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
    this.isDeleting = true;
    this.eventService.deleteEvent(this.originalEvent.id).subscribe(() => {
      this.isDeleting = false;
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
