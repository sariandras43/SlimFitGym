import { Routes } from '@angular/router';
import { MainPageComponent } from './Pages/main-page/main-page.component';
import { SubscriptionsPageComponent } from './Pages/subscriptions-page/subscriptions-page.component';
import { LogInPageComponent } from './Pages/log-in-page/log-in-page.component';
import { SignUpPageComponent } from './Pages/sign-up-page/sign-up-page.component';
import { RoomsComponent } from './Pages/rooms/rooms.component';
import { RoomDetailComponent } from './Pages/room-detail/room-detail.component';
import { MachinesPageComponent } from './Pages/machines-page/machines-page.component';
import { TrainigsPageComponent } from './Pages/trainings-page/trainings-page.component';
import { UserPageComponent } from './Pages/user-page/user-page.component';
import { BasicUserDataComponent } from './Components/CMS/basic-user-data/basic-user-data.component';
import { MachinesCMSComponent } from './Components/CMS/machines-cms/machines-cms.component';
import { PassesCMSComponent } from './Components/CMS/passes-cms/passes-cms.component';
import { RoomCMSComponent } from './Components/CMS/room-cms/room-cms.component';
import { UserCMSComponent } from './Components/CMS/user-cms/user-cms.component';
import { MyPassComponent } from './Components/CMS/my-pass/my-pass.component';
import { TrainingsCMSComponent } from './Components/CMS/trainings-cms/trainings-cms.component';
import { MyTrainingsCMSComponent } from './Components/CMS/my-trainings-cms/my-trainings-cms.component';
import { RoleGuard } from './Auth/role.guard';
import { AuthGuard } from './Auth/auth.guard';
import { NotFoundComponent } from './Pages/not-found/not-found.component';
import { SubscribedTrainingsComponent } from './Components/CMS/subscribed-trainings/subscribed-trainings.component';
import { DashboardComponent } from './Components/CMS/dashboard/dashboard.component';
import { WorkerPageComponent } from './Components/CMS/worker-page/worker-page.component';

export const routes: Routes = [
  { path: '', component: MainPageComponent },
  { path: 'subscriptions', component: SubscriptionsPageComponent },
  { path: 'rooms', component: RoomsComponent },
  { path: 'rooms/:id', component: RoomDetailComponent },
  { path: 'machines', component: MachinesPageComponent },
  { path: 'training', component: TrainigsPageComponent },
  {
    path: 'employee',
    component: WorkerPageComponent,
    data: { allowedRoles: ['employee'] },
    canActivate: [RoleGuard]
  },
  {
    path: 'user',
    component: UserPageComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'userData', component: BasicUserDataComponent },
      { path: 'subscribedTrainings', component: SubscribedTrainingsComponent },

      {
        path: 'myPass',
        component: MyPassComponent,
        data: { allowedRoles: ['worker'] },
      },
      {
        path: 'myTrainings',
        component: MyTrainingsCMSComponent,
        canActivate: [RoleGuard],
        data: { allowedRoles: ['trainer', 'admin'] },
      },

      {
        path: 'trainings',
        component: TrainingsCMSComponent,
        canActivate: [RoleGuard],
        data: { allowedRoles: ['admin'] },
      },
      {
        path: 'dashboard',
        component: DashboardComponent,
        canActivate: [RoleGuard],
        data: { allowedRoles: ['admin'] },
      },
      {
        path: 'passes',
        component: PassesCMSComponent,
        canActivate: [RoleGuard],
        data: { allowedRoles: ['admin'] },
      },
      {
        path: 'rooms',
        component: RoomCMSComponent,
        canActivate: [RoleGuard],
        data: { allowedRoles: ['admin'] },
      },
      {
        path: 'users',
        component: UserCMSComponent,
        canActivate: [RoleGuard],
        data: { allowedRoles: ['admin'] },
      },
      {
        path: 'machines',
        component: MachinesCMSComponent,
        canActivate: [RoleGuard],
        data: { allowedRoles: ['admin'] },
      },
      { path: '', redirectTo: 'userData', pathMatch: 'full' },
    ],
  },

  { path: 'login', component: LogInPageComponent },
  { path: 'signup', component: SignUpPageComponent },
  { path: '404', component: NotFoundComponent },
  { path: '**', redirectTo: '/404' },
];
