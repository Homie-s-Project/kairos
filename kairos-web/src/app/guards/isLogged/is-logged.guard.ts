import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateChild, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { catchError, from, map, of, Observable } from 'rxjs';
import { AlertDialogService } from 'src/app/services/alert-dialog/alert-dialog.service';
import { AuthService } from 'src/app/services/auth/auth.service';

@Injectable({
  providedIn: 'root'
})

export class IsLoggedGuard implements CanActivateChild {
  constructor(private auth: AuthService,
    private alertDialog: AlertDialogService, 
    private router: Router) { }

  canActivateChild = (
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
          this.alertDialog.displayAlert({alertMessage: 'Veillez-vous connecter', alertType: 'alert'})
          this.router.navigate(['/landing']);
          return false;
        }
      })
    );
  }
}
