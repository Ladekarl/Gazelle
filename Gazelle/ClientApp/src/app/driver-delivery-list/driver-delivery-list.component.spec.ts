import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DriverDeliveryListComponent } from './driver-delivery-list.component';

describe('DriverDeliveryListComponent', () => {
  let component: DriverDeliveryListComponent;
  let fixture: ComponentFixture<DriverDeliveryListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DriverDeliveryListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DriverDeliveryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
