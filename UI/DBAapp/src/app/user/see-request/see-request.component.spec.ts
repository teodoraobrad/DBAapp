import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SeeRequestComponent } from './see-request.component';

describe('SeeRequestComponent', () => {
  let component: SeeRequestComponent;
  let fixture: ComponentFixture<SeeRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SeeRequestComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SeeRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
