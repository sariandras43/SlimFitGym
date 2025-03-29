import { Routes } from '@angular/router';
import { MainPageComponent } from './Pages/main-page/main-page.component';
import { SubscriptionsPageComponent } from './Pages/subscriptions-page/subscriptions-page.component';
import { LogInPageComponent } from './Pages/log-in-page/log-in-page.component';
import { SignUpPageComponent } from './Pages/sign-up-page/sign-up-page.component';
import { RoomsComponent } from './Pages/rooms/rooms.component';
import { RoomDetailComponent } from './Pages/room-detail/room-detail.component';
import { MachinesPageComponent } from './Pages/machines-page/machines-page.component';
import { TrainigsPageComponent } from './Pages/trainigs-page/trainigs-page.component';
import { UserPageComponent } from './Pages/user-page/user-page.component';
import { BasicUserDataComponent } from './Components/CMS/basic-user-data/basic-user-data.component';
import { MachinesCMSComponent } from './Components/CMS/machines-cms/machines-cms.component';
import { PassesCMSComponent } from './Components/CMS/passes-cms/passes-cms.component';
import { RoomCMSComponent } from './Components/CMS/room-cms/room-cms.component';
import { UserCMSComponent } from './Components/CMS/user-cms/user-cms.component';
import { MyPassComponent } from './Components/CMS/my-pass/my-pass.component';

export const routes: Routes = [
  { path: '', component: MainPageComponent },
  { path: 'subscriptions', component: SubscriptionsPageComponent },
  { path: 'rooms', component: RoomsComponent },
  { path: 'rooms/:id', component: RoomDetailComponent },
  { path: 'machines', component: MachinesPageComponent },
  { path: 'training', component: TrainigsPageComponent },
  {
    path: 'user',
    component: UserPageComponent,
    children: [
      { path: 'userData', component: BasicUserDataComponent },
      { path: 'machines', component: MachinesCMSComponent },
      { path: 'passes', component: PassesCMSComponent },
      { path: 'rooms', component: RoomCMSComponent },
      { path: 'users', component: UserCMSComponent },
      { path: 'myPass', component: MyPassComponent },
      { path: '', redirectTo: 'userData', pathMatch: 'full' },
    ],
  },

  { path: 'login', component: LogInPageComponent },
  { path: 'signup', component: SignUpPageComponent },
];
