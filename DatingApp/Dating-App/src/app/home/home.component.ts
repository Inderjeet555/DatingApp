import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
registerMode = false;
  constructor() {console.log(this.registerMode); }

  ngOnInit() {
  }

  registerToggle() {
    this.registerMode = true;
    console.log(this.registerMode);
  }

  cancelRegisterMode(registerMode: boolean) {
      this.registerMode = registerMode;
  }

}
