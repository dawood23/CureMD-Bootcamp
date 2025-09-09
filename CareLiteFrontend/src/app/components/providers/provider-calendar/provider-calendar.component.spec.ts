import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProviderCalendarComponent } from './provider-calendar.component';

describe('ProviderCalendarComponent', () => {
  let component: ProviderCalendarComponent;
  let fixture: ComponentFixture<ProviderCalendarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProviderCalendarComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProviderCalendarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
