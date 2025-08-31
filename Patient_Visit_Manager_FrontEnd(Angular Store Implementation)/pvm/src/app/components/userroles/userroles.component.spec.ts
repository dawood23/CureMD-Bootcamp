import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserrolesComponent } from './userroles.component';

describe('UserrolesComponent', () => {
  let component: UserrolesComponent;
  let fixture: ComponentFixture<UserrolesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserrolesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UserrolesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
