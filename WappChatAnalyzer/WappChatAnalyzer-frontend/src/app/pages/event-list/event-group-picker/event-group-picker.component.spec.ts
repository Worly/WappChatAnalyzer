import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventGroupPickerComponent } from './event-group-picker.component';

describe('EventGroupPickerComponent', () => {
  let component: EventGroupPickerComponent;
  let fixture: ComponentFixture<EventGroupPickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventGroupPickerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EventGroupPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
