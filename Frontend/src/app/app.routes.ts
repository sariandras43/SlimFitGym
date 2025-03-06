import { Routes } from '@angular/router';
import { MainPageComponent } from './Pages/main-page/main-page.component';
import { SubscriptionsPageComponent } from './Pages/subscriptions-page/subscriptions-page.component';
import { LogInPageComponent } from './Pages/log-in-page/log-in-page.component';
import { SignUpPageComponent } from './Pages/sign-up-page/sign-up-page.component';
import { RoomsComponent } from './Pages/rooms/rooms.component';
import { RoomDetailComponent } from './Components/room-detail/room-detail.component';
import { MachinesPageComponent } from './Pages/machines-page/machines-page.component';
import { TrainigsPageComponent } from './Pages/trainigs-page/trainigs-page.component';

export const routes: Routes = [
    { path: '', component: MainPageComponent },
    { path: 'subscriptions', component: SubscriptionsPageComponent },
    { path: 'rooms', component: RoomsComponent },
    {path: 'rooms/pelda', component: RoomDetailComponent},
    {path: 'machines', component: MachinesPageComponent},
    {path: 'training', component: TrainigsPageComponent},
    
    {path: 'login', component: LogInPageComponent},
    {path: 'signup', component: SignUpPageComponent}
];
