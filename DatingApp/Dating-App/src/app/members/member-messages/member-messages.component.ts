import { AlertifyService } from './../../_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from './../../_services/auth.service';
import { Message } from './../../_models/Message';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
@Input() recipientId: number;
messages: Message[] = [];
newMessages: any = {};


  constructor(private authService: AuthService, private userService: UserService, private alertiFy: AlertifyService) { }

  ngOnInit() {
    this.loadMessages();
  }

  loadMessages() {
    this.userService.getMessagesThread(this.authService.decodedToken.nameid, this.recipientId).subscribe(messages => {
      this.messages = messages;
    }, error => {
      this.alertiFy.error(error);
    });
  }

  sendMessage() {
    this.newMessages.recipientId = this.recipientId;
    this.userService.sendMessage(this.authService.decodedToken.nameid, this.newMessages).subscribe((messages: Message) => {
      this.messages.unshift(messages);
      // console.log(this.messages);
      this.newMessages.content = '';
    }, error => {
      this.alertiFy.error(error);
    });
  }

}
