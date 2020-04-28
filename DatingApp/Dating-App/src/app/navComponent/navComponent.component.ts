import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-navComponent',
  templateUrl: './navComponent.component.html',
  styleUrls: ['./navComponent.component.css']
})
export class NavComponentComponent implements OnInit {
  model: any =  {};
  photoUrl: string;

  constructor(public authService: AuthService, private alertify: AlertifyService,
              private router: Router ) { }

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('LoggedIn successfully');
      this.model.username = '';
      this.model.password = '';
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.router.navigate(['/members']);
      });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  loggedOut() {
    localStorage.removeItem('token');
    this.alertify.message('logged out');
    this.router.navigate(['/home']);
    localStorage.removeItem('user');
    this.authService.currentUser = null;
    this.authService.decodedToken = null;
  }
}
