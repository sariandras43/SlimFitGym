import { Routes } from '@angular/router';
import { MainPageComponent } from './main-page/main-page.component';
import { SubscriptionsComponent } from './subscriptions/subscriptions.component';
import { SubscriptionsPageComponent } from './subscriptions-page/subscriptions-page.component';
import { LogInPageComponent } from './log-in-page/log-in-page.component';
import { SignUpPageComponent } from './sign-up-page/sign-up-page.component';

export const routes: Routes = [
    { path: '', component: MainPageComponent },
    { path: 'subscriptions', component: SubscriptionsPageComponent },

    {path: 'login', component: LogInPageComponent},
    {path: 'signup', component: SignUpPageComponent}
];
