import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeliveryService {

  url = "http://localhost:44309/cities";

  constructor(private http: HttpClient) { }

  getCities(): Observable<any> {
    return this.http.get(this.url).pipe(result => result);
  }
}
