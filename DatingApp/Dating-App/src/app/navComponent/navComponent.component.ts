import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-navComponent',
  templateUrl: './navComponent.component.html',
  styleUrls: ['./navComponent.component.css']
})
export class NavComponentComponent implements OnInit {
  model: any =  {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
        console.log('LoggedIn successfully');
      }, error => {
        console.log('failed to login');
      });
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !! token;
  }

  loggedOut() {
    localStorage.removeItem('token');
    console.log('Logged Out');
  }
}
