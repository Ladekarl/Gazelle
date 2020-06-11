import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavMenuDriverComponent } from './nav-menu-driver.component';

describe('NavMenuDriverComponent', () => {
  let component: NavMenuDriverComponent;
  let fixture: ComponentFixture<NavMenuDriverComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NavMenuDriverComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavMenuDriverComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
