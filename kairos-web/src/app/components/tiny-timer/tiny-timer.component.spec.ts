import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TinyTimerComponent } from './tiny-timer.component';

describe('TinyTimerComponent', () => {
  let component: TinyTimerComponent;
  let fixture: ComponentFixture<TinyTimerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TinyTimerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TinyTimerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
