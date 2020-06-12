import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { DeliveryService } from "../delivery.service";


@Component({
  selector: 'app-create-deliveries',
  templateUrl: './create-deliveries.component.html',
  styleUrls: ['./create-deliveries.component.css']
})
export class CreateDeliveriesComponent implements OnInit {

  cities: String [];

  constructor(private deliveryService: DeliveryService) {}

  form = new FormGroup({
    city: new FormControl('', Validators.required),
    destination: new FormControl('', Validators.required),
  });
 

  ngOnInit() {
    this.deliveryService.getCities().subscribe(result => this.cities = result);
  }

}
