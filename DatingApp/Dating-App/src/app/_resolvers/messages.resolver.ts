import { AuthService } from './../_services/auth.service';
import { Message } from './../_models/Message';
import {Injectable} from '@angular/core';
import { from, Observable, of } from 'rxjs';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/User';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MessageResolver implements Resolve<Message[]> {
    pageNumber = 1;
    pageSize = 10;
    messageContainer = 'Unread';
    constructor(private userService: UserService, private alertify: AlertifyService, private router: Router, 
                private authService: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
        // tslint:disable-next-line: no-string-literal
        return this.userService.getMessages(this.authService.decodedToken.nameid
             , this.pageNumber, this.pageSize, this.messageContainer).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving messages');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
