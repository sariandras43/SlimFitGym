// auth.guard.ts
import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { Observable, map, take } from 'rxjs';
import { UserService } from '../Services/user.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private userService: UserService, private router: Router) {}

  canActivate(): Observable<boolean | UrlTree> {
    return this.userService.loggedInUser$.pipe(
      take(1), // Take the latest user state
      map((user) => {
        // Allow navigation if user exists, else redirect to login
        return user ? true : this.router.parseUrl('/login');
      })
    );
  }
}