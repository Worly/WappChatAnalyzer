import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatisticDisplayEmojisComponent } from './statistic-display-emojis.component';

describe('StatisticDisplayEmojisComponent', () => {
  let component: StatisticDisplayEmojisComponent;
  let fixture: ComponentFixture<StatisticDisplayEmojisComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatisticDisplayEmojisComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StatisticDisplayEmojisComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
