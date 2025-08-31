import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VisittypesComponent } from './visittypes.component';

describe('VisittypesComponent', () => {
  let component: VisittypesComponent;
  let fixture: ComponentFixture<VisittypesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VisittypesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VisittypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
