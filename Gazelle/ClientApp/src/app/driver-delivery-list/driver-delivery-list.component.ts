import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { DeliveryListService } from '../delivery-list.service';

@Component({
  selector: 'app-driver-delivery-list',
  templateUrl: './driver-delivery-list.component.html',
  styleUrls: ['./driver-delivery-list.component.css']
})
export class DriverDeliveryListComponent implements OnInit{

  constructor(private deliveryListService: DeliveryListService) {}

  deliveries: string[];

  getRouteDetails() {
    alert("CLICKED");

  }

  public displayRuteDetails(delivery) {
    delivery.open = !delivery.open;
  }

  ngOnInit() {
    this.deliveryListService.getDeliveryList().subscribe(result => {
      this.deliveries = result;
    }, error => console.error(error));
  }
}
