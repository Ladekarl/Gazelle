import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { NavMenuSalesmanComponent } from './nav-menu-salesman/nav-menu-salesman.component';
import { NavMenuDriverComponent } from './nav-menu-driver/nav-menu-driver.component';
import { DriverRouteDetailsComponent } from './driver-route-details/driver-route-details.component';
import { DriverDeliveryListComponent } from './driver-delivery-list/driver-delivery-list.component';
import { SalesmanRouteDetailsComponent } from './salesman-route-details/salesman-route-details.component';
import { SalesmanRouteListComponent } from './salesman-route-list/salesman-route-list.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    NavMenuSalesmanComponent,
    NavMenuDriverComponent,
    DriverRouteDetailsComponent,
    DriverDeliveryListComponent,
    SalesmanRouteDetailsComponent,
    SalesmanRouteListComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'salesman-route-list', component: SalesmanRouteListComponent },
      { path: 'salesman-route-details', component: SalesmanRouteDetailsComponent },
      { path: 'driver-delivery-list', component: DriverDeliveryListComponent },
      { path: 'driver-route-details', component: DriverRouteDetailsComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
