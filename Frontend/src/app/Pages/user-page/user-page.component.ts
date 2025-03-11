import { Component } from '@angular/core';
import { AccordionSegmentComponent } from "../../Components/accordion-segment/accordion-segment.component";
import { NewPasswordComponent } from "../../Components/CMS/new-password/new-password.component";
import { BasicUserDataComponent } from "../../Components/CMS/basic-user-data/basic-user-data.component";

@Component({
  selector: 'app-user-page',
  imports: [AccordionSegmentComponent, NewPasswordComponent, BasicUserDataComponent],
  templateUrl: './user-page.component.html',
  styleUrl: './user-page.component.scss'
})
export class UserPageComponent {

}
