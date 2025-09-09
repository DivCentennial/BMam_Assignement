import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EmployeePersonal, EmployeeProfessional, EmployeeUpsertRequest } from '../models/employee.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = 'https://localhost:7000/api'; // Update with your API URL

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  private getHeaders(): HttpHeaders {
    const authHeaders = this.authService.getAuthHeaders();
    return new HttpHeaders(authHeaders);
  }

  getAllEmployees(): Observable<EmployeePersonal[]> {
    return this.http.get<EmployeePersonal[]>(`${this.apiUrl}/employee`);
  }

  getEmployeeById(id: number): Observable<EmployeePersonal> {
    return this.http.get<EmployeePersonal>(`${this.apiUrl}/employee/${id}`);
  }

  createEmployee(employeeRequest: EmployeeUpsertRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/employee`, employeeRequest, {
      headers: this.getHeaders()
    });
  }

  updateEmployee(id: number, employeeRequest: EmployeeUpsertRequest): Observable<any> {
    return this.http.put(`${this.apiUrl}/employee/${id}`, employeeRequest, {
      headers: this.getHeaders()
    });
  }

  deleteEmployee(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/employee/${id}`, {
      headers: this.getHeaders()
    });
  }

  uploadImage(id: number, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.apiUrl}/employee/${id}/image`, formData, {
      headers: this.getHeaders()
    });
  }

  downloadImage(id: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/employee/${id}/image`, {
      responseType: 'blob'
    });
  }

  deleteImage(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/employee/${id}/image`, {
      headers: this.getHeaders()
    });
  }

  uploadDocument(id: number, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.apiUrl}/employee/${id}/document`, formData, {
      headers: this.getHeaders()
    });
  }

  downloadDocument(id: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/employee/${id}/document`, {
      responseType: 'blob'
    });
  }

  deleteDocument(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/employee/${id}/document`, {
      headers: this.getHeaders()
    });
  }

  exportCsv(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/employee/export/csv`, {
      responseType: 'blob'
    });
  }

  importCsv(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.apiUrl}/employee/import/csv`, formData);
  }
}

