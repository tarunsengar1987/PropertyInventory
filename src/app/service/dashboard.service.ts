import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { APIResponse } from '../model/api-response.model';

@Injectable({ providedIn: 'root' })
export class DashboardService {

  constructor(private http: HttpClient) { }

  getSales(): Observable<APIResponse<any[]>> {
    return this.http.get<APIResponse<any[]>>(`${environment.apiUrl}Dashboard/property-sales`);
  }
}