import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatisticDisplayCustomComponent } from './statistic-display-custom.component';

describe('StatisticDisplayCustomComponent', () => {
  let component: StatisticDisplayCustomComponent;
  let fixture: ComponentFixture<StatisticDisplayCustomComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatisticDisplayCustomComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StatisticDisplayCustomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
