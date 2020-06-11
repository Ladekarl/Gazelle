import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalesmanRouteListComponent } from './salesman-route-list.component';

describe('SalesmanRouteListComponent', () => {
  let component: SalesmanRouteListComponent;
  let fixture: ComponentFixture<SalesmanRouteListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalesmanRouteListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalesmanRouteListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
