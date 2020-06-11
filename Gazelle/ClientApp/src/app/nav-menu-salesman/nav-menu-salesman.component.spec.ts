import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavMenuSalesmanComponent } from './nav-menu-salesman.component';

describe('NavMenuSalesmanComponent', () => {
  let component: NavMenuSalesmanComponent;
  let fixture: ComponentFixture<NavMenuSalesmanComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NavMenuSalesmanComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavMenuSalesmanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
