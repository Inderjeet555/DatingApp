import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-member-cards',
  templateUrl: './member-cards.component.html',
  styleUrls: ['./member-cards.component.css']
})
export class MemberCardsComponent implements OnInit {
  @Input() user: User;

  constructor() { }

  ngOnInit() {
  }

}
