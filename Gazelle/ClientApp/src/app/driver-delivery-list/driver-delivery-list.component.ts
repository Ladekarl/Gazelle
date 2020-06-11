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
    //this.buildTableRows();
  }

  /**buildTableRows() {
    var table = <HTMLTableElement> document.getElementById("driverDeliveryTableBody");
    var row = table.insertRow(0);
    var titleCellPackageId = row.insertCell(0);
    var titleCellOriginCity = row.insertCell(1);
    var titleCellDestinationCity = row.insertCell(2);

    let listOfDeliveries = this.deliveryListService.getDeliveryList();

    titleCellPackageId.innerHTML = "1-placeholder";
    titleCellOriginCity.innerHTML = "2-placeholders";
    titleCellDestinationCity.innerHTML = "3-happy-little-placeholders";
  }*/
}
