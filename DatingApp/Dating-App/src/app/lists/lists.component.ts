import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from './../_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from './../_services/auth.service';
import { Pagination, PaginationResult } from './../_models/Pagination';
import { User } from './../_models/User';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  likesParamas: string;

  constructor(private authService: AuthService, private userService: UserService,
              private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      // tslint:disable-next-line: no-string-literal
      this.users = data['users'].result;
      // tslint:disable-next-line: no-string-literal
      this.pagination = data['users'].pagination;
    });
    this.likesParamas = 'Likers';
  }

  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, null, this.likesParamas)
    .subscribe((res: PaginationResult<User[]>) => {
      this.users = res.result;
      this.pagination = res.pagination;
    },
    error => {
      this.alertify.error(error);
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

}
