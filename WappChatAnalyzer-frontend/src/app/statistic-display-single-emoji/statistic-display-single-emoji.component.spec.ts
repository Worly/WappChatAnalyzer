import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatisticDisplaySingleEmojiComponent } from './statistic-display-single-emoji.component';

describe('StatisticDisplaySingleEmojiComponent', () => {
  let component: StatisticDisplaySingleEmojiComponent;
  let fixture: ComponentFixture<StatisticDisplaySingleEmojiComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatisticDisplaySingleEmojiComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StatisticDisplaySingleEmojiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
