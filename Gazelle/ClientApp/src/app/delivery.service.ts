import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeliveryService {

  baseUrl;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  getCities(): Observable<any> {
    return this.http.get(this.baseUrl + 'cities').pipe(result => result);
  }

  post(DriverId, Weight, StartCity, EndCity, ApprovedRouteId) {
    return this.http.post(this.baseUrl + 'deliveries', {
      DriverId,
      Weight,
      StartCity,
      EndCity,
      ApprovedRouteId
    });
  }

  getRoutes(origin, destination, weight, length, height, depth, deliveryTypes): Observable<any> {
    return this.http.get(this.baseUrl + `routes?origin=${origin}&destination=${destination}&weight=${weight}&length=${length}&height=${height}&depth=${depth}&deliveryTypes=${deliveryTypes}`).pipe(result => result);
  }
}
