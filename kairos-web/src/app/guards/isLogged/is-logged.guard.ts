import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { catchError, from, map, of, Observable } from 'rxjs';
import { AuthService } from 'src/app/services/auth/auth.service';

@Injectable({
  providedIn: 'root'
})

export class IsLoggedGuard implements CanActivate {
  constructor(private auth: AuthService, private router: Router) { }

  canActivate = (
    route: ActivatedRouteSnapshot, 
    state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> => {
    return from(this.auth.isLoggedIn()).pipe(
      catchError(error => {
        console.error(error);
        this.router.navigate(['/landing']);
        return of(false);
      }),
      // Vérifie si l'utilisateur est authentifié
      map(isLoggedIn => {
        if (isLoggedIn) {
          return true;
        } else {
          this.router.navigate(['/landing']);
          return false;
        }
      })
    );
  }
}
