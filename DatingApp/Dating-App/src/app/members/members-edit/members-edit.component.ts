import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/User';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-members-edit',
  templateUrl: './members-edit.component.html',
  styleUrls: ['./members-edit.component.css']
})
export class MembersEditComponent implements OnInit {
  user: User;
  @ViewChild('editForm') editForm: NgForm;
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
      if (this.editForm.dirty) {
        $event.returnValue = true;
      }
  }
    constructor( private router: ActivatedRoute, private alertify: AlertifyService, private userService: UserService,
                 private authService: AuthService) { }

  ngOnInit() {
    this.router.data.subscribe(data => {
        // tslint:disable-next-line: no-string-literal
        this.user = data['user'];
    });
  }

  updateUser(id) {
    this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
      this.alertify.success('profile updated successfully');
      this.editForm.reset(this.user);
    }, error => {
        this.alertify.error(error);
    });
  }

}
