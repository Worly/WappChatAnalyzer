import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupingPeriodPickerComponent } from './grouping-period-picker.component';

describe('GroupingPeriodPickerComponent', () => {
  let component: GroupingPeriodPickerComponent;
  let fixture: ComponentFixture<GroupingPeriodPickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GroupingPeriodPickerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupingPeriodPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
