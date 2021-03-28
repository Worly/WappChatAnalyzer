import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventGroupFilterComponent } from './event-group-filter.component';

describe('EventGroupFilterComponent', () => {
  let component: EventGroupFilterComponent;
  let fixture: ComponentFixture<EventGroupFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventGroupFilterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EventGroupFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
