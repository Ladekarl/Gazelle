import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav-menu-salesman',
  templateUrl: './nav-menu-salesman.component.html',
  styleUrls: ['./nav-menu-salesman.component.css']
})
export class NavMenuSalesmanComponent {

  isExpanded = false;
  counterElement = document.getElementById("nav_counterLink");
  fetchElement = document.getElementById("nav_fetchLink");

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
