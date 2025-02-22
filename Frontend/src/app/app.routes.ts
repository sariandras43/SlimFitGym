import { Routes } from '@angular/router';
import { MainPageComponent } from './main-page/main-page.component';
import { SubscriptionsPageComponent } from './subscriptions-page/subscriptions-page.component';
import { LogInPageComponent } from './log-in-page/log-in-page.component';
import { SignUpPageComponent } from './sign-up-page/sign-up-page.component';
import { RoomsComponent } from './rooms/rooms.component';
import { RoomDetailComponent } from './room-detail/room-detail.component';

export const routes: Routes = [
    { path: '', component: MainPageComponent },
    { path: 'subscriptions', component: SubscriptionsPageComponent },
    { path: 'rooms', component: RoomsComponent },
    {path: 'rooms/pelda', component: RoomDetailComponent},

    {path: 'login', component: LogInPageComponent},
    {path: 'signup', component: SignUpPageComponent}
];
