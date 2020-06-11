import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeliveryListService {

  url: string = "http://localhost:44309/deliveries";
  url2: string = "";

  baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') base: string) {
    this.baseUrl = base;
  }

  getDeliveryList() {
    return this.http.get<string[]>(this.baseUrl + 'deliveries');
  }
}
