import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DriverRouteDetailsComponent } from './driver-route-details.component';

describe('DriverRouteDetailsComponent', () => {
  let component: DriverRouteDetailsComponent;
  let fixture: ComponentFixture<DriverRouteDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DriverRouteDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DriverRouteDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
