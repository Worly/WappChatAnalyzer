import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RatioBarComponent } from './ratio-bar.component';

describe('RatioBarComponent', () => {
  let component: RatioBarComponent;
  let fixture: ComponentFixture<RatioBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RatioBarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RatioBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
