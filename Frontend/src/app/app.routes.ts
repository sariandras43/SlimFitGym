import { Routes } from '@angular/router';
import { MainPageComponent } from './main-page/main-page.component';
import { SubscriptionsComponent } from './subscriptions/subscriptions.component';
import { SubscriptionsPageComponent } from './subscriptions-page/subscriptions-page.component';

export const routes: Routes = [
    { path: '', component: MainPageComponent },
    { path: 'subscriptions', component: SubscriptionsPageComponent },
];
