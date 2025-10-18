import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SqlserversComponent } from './sqlservers.component';

describe('SqlserversComponent', () => {
  let component: SqlserversComponent;
  let fixture: ComponentFixture<SqlserversComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SqlserversComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SqlserversComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
