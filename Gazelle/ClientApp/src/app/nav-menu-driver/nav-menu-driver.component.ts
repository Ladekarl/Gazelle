import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav-menu-driver',
  templateUrl: './nav-menu-driver.component.html',
  styleUrls: ['./nav-menu-driver.component.css']
})
export class NavMenuDriverComponent {

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
