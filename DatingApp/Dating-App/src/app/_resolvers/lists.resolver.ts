import {Injectable} from '@angular/core';
import { from, Observable, of } from 'rxjs';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/User';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ListsResolver implements Resolve<User[]> {
    pageNumber = 1;
    pageSize = 10;
    likesParams = 'Likers';
    constructor(private userService: UserService, private alertify: AlertifyService, private router: Router) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
        // tslint:disable-next-line: no-string-literal
        return this.userService.getUsers(this.pageNumber, this.pageSize, null , this.likesParams).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving user');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
