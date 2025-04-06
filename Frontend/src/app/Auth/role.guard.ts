import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UserService } from '../Services/user.service';
import { map, take } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
  constructor(private userService: UserService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> {
    const allowedRoles = route.data['allowedRoles'];
    
    return this.userService.loggedInUser$.pipe(
      take(1),
      map(user => {
        if (user && allowedRoles.includes(user.role)) {
          return true;
        }
        return this.router.parseUrl('/user'); 
      })
    );
  }
}