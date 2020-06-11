import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalesmanRouteDetailsComponent } from './salesman-route-details.component';

describe('SalesmanRouteDetailsComponent', () => {
  let component: SalesmanRouteDetailsComponent;
  let fixture: ComponentFixture<SalesmanRouteDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalesmanRouteDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalesmanRouteDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
