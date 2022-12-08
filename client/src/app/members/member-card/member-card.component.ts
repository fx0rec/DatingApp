import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { Member } from 'src/app/_models/member';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
  //encapsulation: ViewEncapsulation.Emulated

})
export class MemberCardComponent implements OnInit {
@Input() member: Member | undefined; //member is currently undefined, we don't have access to it yet. This way we get around the TS error

  constructor() { }

  ngOnInit(): void {
  }

}
