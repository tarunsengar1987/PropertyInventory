import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { APIResponse } from '../model/api-response.model';
import { Contact, CreateContactDto, UpdateContactDto } from '../model/contact.model';

@Injectable({
  providedIn: 'root'
})
export class ContactService {

  private baseUrl = `${environment.apiUrl}Contact`;

  constructor(private http: HttpClient) { }

  getAll(search?: string): Observable<APIResponse<any>> {
    const params: any = {};
    if (search) params['search'] = search;
    return this.http.get<APIResponse<Contact[]>>(this.baseUrl, { params });
  }

  getById(id: string): Observable<APIResponse<Contact>> {
    return this.http.get<APIResponse<Contact>>(`${this.baseUrl}/${id}`);
  }

  create(data: CreateContactDto): Observable<APIResponse<Contact>> {
    return this.http.post<APIResponse<Contact>>(this.baseUrl, data);
  }

  update(id: string, data: UpdateContactDto): Observable<APIResponse<Contact>> {
    return this.http.put<APIResponse<Contact>>(`${this.baseUrl}/${id}`, data);
  }

  delete(id: string): Observable<APIResponse<void>> {
    return this.http.delete<APIResponse<void>>(`${this.baseUrl}/${id}`);
  }
}
