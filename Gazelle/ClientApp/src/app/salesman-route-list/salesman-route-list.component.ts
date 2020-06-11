import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-salesman-route-list',
  templateUrl: './salesman-route-list.component.html',
  styleUrls: ['./salesman-route-list.component.css']
})
export class SalesmanRouteListComponent implements OnInit {

  ngOnInit() {
    this.buildTableRows();
  }
  buildTableRows() {
    var table = <HTMLTableElement>document.getElementById("driverDeliveryTableBody");
    var row = table.insertRow(0);
    var titleCellPackageId = row.insertCell(0);
    var titleCellOriginCity = row.insertCell(1);
    var titleCellDestinationCity = row.insertCell(2);
    titleCellPackageId.innerHTML = "1-placeholder";
    titleCellOriginCity.innerHTML = "2-placeholders";
    titleCellDestinationCity.innerHTML = "3-happy-little-placeholders";
  }

}
