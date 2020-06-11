import { Component, OnInit } from '@angular/core';
import { DeliveryListService } from '../delivery-list.service';

@Component({
  selector: 'app-driver-delivery-list',
  templateUrl: './driver-delivery-list.component.html',
  styleUrls: ['./driver-delivery-list.component.css']
})
export class DriverDeliveryListComponent implements OnInit{

  constructor(private deliveryListService: DeliveryListService) {}

  public deliveries = this.deliveryListService.getDeliveryList();

  ngOnInit() {
  }
}
