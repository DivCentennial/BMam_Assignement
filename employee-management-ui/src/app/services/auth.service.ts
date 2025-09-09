import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { UserAuth, LoginRequest } from '../models/user-auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7000/api'; // Update with your API URL
  private currentUserSubject = new BehaviorSubject<UserAuth | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    // Check if user is already logged in (only in browser)
    if (typeof window !== 'undefined') {
      const savedUser = localStorage.getItem('currentUser');
      if (savedUser) {
        this.currentUserSubject.next(JSON.parse(savedUser));
      }
    }
  }

  login(loginRequest: LoginRequest): Observable<UserAuth> {
    return this.http.post<UserAuth>(`${this.apiUrl}/auth/login`, loginRequest)
      .pipe(
        tap(user => {
          this.currentUserSubject.next(user);
          if (typeof window !== 'undefined') {
            localStorage.setItem('currentUser', JSON.stringify(user));
          }
        })
      );
  }

  logout(): void {
    this.currentUserSubject.next(null);
    if (typeof window !== 'undefined') {
      localStorage.removeItem('currentUser');
    }
  }

  getCurrentUser(): UserAuth | null {
    return this.currentUserSubject.value;
  }

  isLoggedIn(): boolean {
    return this.currentUserSubject.value !== null;
  }

  isAdmin(): boolean {
    const user = this.getCurrentUser();
    return user?.role === 'Admin';
  }

  getAuthHeaders(): { [key: string]: string } {
    const user = this.getCurrentUser();
    return user ? { 'X-Role': user.role } : {};
  }
}
