import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { EventService } from 'src/app/services/event.service';
import { Event, EventGroup, EventInfo, EventTemplate } from "../../../dtos/event";
import * as dateFormat from "dateformat";
import { createPopup, PopupPickerController } from "@picmo/popup-picker";
import { fromEvent, Subscription } from 'rxjs';

@Component({
  selector: 'app-event-edit',
  templateUrl: './event-edit.component.html',
  styleUrls: ['./event-edit.component.css']
})
export class EventEditComponent implements OnInit, OnDestroy {

  private _emojiButton: ElementRef<HTMLElement>;
  @ViewChild("emojiButton", { read: ElementRef }) set emojiButton(b: ElementRef<HTMLElement>) {
    if (b == this._emojiButton)
      return;

    if (this._emojiButton != null && this.emojiPopup != null) {
      this.emojiPopup.destroy();
      this.emojiPopup = null;
    }

    this._emojiButton = b;

    if (b != null) {
      this.emojiPopup = createPopup({
        animate: true,
        emojiSize: "min(8vw, 50px)"
      }, {
        referenceElement: b.nativeElement,
        hideOnEmojiSelect: true,
        hideOnClickOutside: true,
        hideOnEscape: true,
        showCloseButton: false,
        position: "auto",
        className: "emoji-picker"
      });

      this.emojiPopup.addEventListener("emoji:select", selection => {
        this.event.emoji = selection.emoji;
        this.onPropertyChange("emoji", selection.emoji);
      });
    }
  }

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

  emojiPopup: PopupPickerController;

  private subs: Subscription[] = [];

  constructor(private eventService: EventService) {
    history.pushState(null, null, window.location.href);
    this.subs.push(fromEvent(window, 'popstate').subscribe(e => {
      if (this.emojiPopup?.isOpen) {
        history.pushState(null, null, window.location.href);
        this.emojiPopup.close();
      }
      else
        history.back();
    }));
  }

  ngOnInit(): void {
    if (this.isNew) {
      var e = new Event();

      let date = new Date();
      //date.setDate(date.getDate() - 1);

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
  }

  ngOnDestroy(): void {
    this.emojiPopup.destroy();

    this.subs.forEach(s => s.unsubscribe());
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

  onEmojiClick(event: MouseEvent) {
    event.stopImmediatePropagation();
    this.emojiPopup.toggle();
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
