import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DeliveryListService {

  url: string = "http://localhost:44309/deliveries";
  url2: string = "";
  constructor(private http: HttpClient) {
  }

  public getDeliveryList() {
    return this.http.get(this.url);//.subscribe(result => result), error => console.error(error);
  }
}

/*interface Deliveries {
  driverId: number;
  weight: number;
  originCity: string;
  destinationCity: string;
}


 public int DeliveryId { get; set; }
        public double DriverId { get; set; }
        public double Weight { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public City StartCity { get; set; }
        public City EndCity { get; set; }
        public double Length { get; set; }
        public Route ApprovedRoute { get; set; }
        public ICollection < Route > Routes { get; set; }*/
