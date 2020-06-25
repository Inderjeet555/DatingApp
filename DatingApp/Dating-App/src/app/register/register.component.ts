import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/User';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  user: User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;
  selectedCity: any;
  private value: any = {};
  private _disabledV: string = '0';
  public disabled: boolean = false;


  data = [{
    id: 1,
    name: 'Amritsar'
  },
  {
    id: 2,
    name: 'Jalandhar'
  },
  {
    id: 3,
    name: 'Batala'
  },
  {
    id: 3,
    name: 'Los Angeles'
  }
];

  constructor(private authService: AuthService, private alertify: AlertifyService, private fb: FormBuilder, private router: Router) { }

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red'
    };
    this.createRegisterForm();
  }

  passwordMatchValidator(g: FormGroup) {
    // tslint:disable-next-line: object-literal-key-quotes
    return g.get('password').value === g.get('confirmPassowrd').value ? null : {'mismatch': true};
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassowrd: ['', Validators.required]
    }, {validators: this.passwordMatchValidator});
  }
 
  private get disabledV(): string {
    return this._disabledV;
  }
 
  private set disabledV(value: string) {
    this._disabledV = value;
    this.disabled = this._disabledV === '1';
  }
 
  public selected(value: any): void {
    console.log('Selected value is: ', value);
  }
 
  public removed(value: any): void {
    console.log('Removed value is: ', value);
  }
 
  public typed(value: any): void {
    console.log('New search input: ', value);
  }
 
  public refreshValue(value: any): void {
    this.value = value;
  }

  register() {
   // console.log(this.registerForm.get('city').value.name);
   // this.user.city = this.registerForm.get('city').value.toString();
    // const useri = Object.assign({}, this.registerForm.value);
    // useri.city = this.registerForm.get('city').value.name;
    // console.log(useri);
    if (this.registerForm.valid) {
      const useri = Object.assign({}, this.registerForm.value);
      useri.city = this.registerForm.get('city').value.name;
      this.user =  Object.assign({}, useri);
      this.authService.register(this.user).subscribe(() => {
        this.alertify.success('Registered Successfully');
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.authService.login(this.user).subscribe(() => {
          this.router.navigate(['/members']);
        });
      });
    }
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log('canceled');
  }

}
