import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmojiInfoSingleComponent } from './emoji-info-single.component';

describe('EmojiInfoSingleComponent', () => {
  let component: EmojiInfoSingleComponent;
  let fixture: ComponentFixture<EmojiInfoSingleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmojiInfoSingleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmojiInfoSingleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
