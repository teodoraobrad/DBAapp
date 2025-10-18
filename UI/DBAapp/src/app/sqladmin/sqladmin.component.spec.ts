import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SqladminComponent } from './sqladmin.component';

describe('SqladminComponent', () => {
  let component: SqladminComponent;
  let fixture: ComponentFixture<SqladminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SqladminComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SqladminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
