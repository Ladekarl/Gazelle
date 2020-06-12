import { Component, OnInit } from '@angular/core';
import { DeliveryService } from "../delivery.service";

@Component({
  selector: 'app-create-deliveries',
  templateUrl: './create-deliveries.component.html',
  styleUrls: ['./create-deliveries.component.css']
})
export class CreateDeliveriesComponent implements OnInit {

  cities: String[];
  origin: any;
  destination: any;
  fragile: any;
  frozen: any;
  animal: any;
  recordedDelivery: any;
  length: any;
  height: any
  depth: any;
  weight: any;
  routes: any;


  constructor(private deliveryService: DeliveryService) {}


  ngOnInit() {
    this.deliveryService.getCities().subscribe(result => {
      this.cities = result;
    });
  }


  showRoutes() {
    var deliveryTypes = '';
    if (this.fragile) {
      deliveryTypes += 'fragile,';
    }
    if (this.frozen) {
      deliveryTypes += 'frozen,';
    }
    if (this.animal) {
      deliveryTypes += 'animal,';
    }
    if (this.recordedDelivery) {
      deliveryTypes += 'recordedDelivery,';
    }
    this.deliveryService.getRoutes(this.origin.cityName, this.destination.cityName, this.weight, this.length, this.height, this.depth, deliveryTypes).subscribe(result => {
      this.routes = result;
    });
  }

  onSelect(route) {
    var driverId = Math.floor(Math.random() * 6);  
    this.deliveryService.post(driverId, this.weight, this.origin.cityName, this.destination.cityName, route.routeId).subscribe(result => {
      alert("Ruten er blevet tildelt chauffør: " + driverId);
    });
  }
}
