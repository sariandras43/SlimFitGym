import { Component, input } from '@angular/core';
import { NgClass } from '@angular/common';
import { PassService } from '../../Services/pass.service';
import { PassModel } from '../../Models/pass.model';
import { PassCardComponent } from '../cards/pass-card/pass-card.component';
import { UserService } from '../../Services/user.service';
import { UserPageComponent } from '../../Pages/user-page/user-page.component';
import { UserModel } from '../../Models/user.model';
import { PurchaseModalComponent } from "../purchase-modal/purchase-modal.component";

@Component({
  selector: 'app-subscriptions',
  imports: [NgClass, PassCardComponent, PurchaseModalComponent],
  templateUrl: './subscriptions.component.html',
  styleUrl: './subscriptions.component.scss',
})
export class SubscriptionsComponent {
  showCardModal = false;
  selectedPass: PassModel | undefined;
  handlePassPurchase($event: PassModel) {
    if (!this.user) return;
    $event.isLoading = true;
    return this.passService
      .buyPass({ accountId: this.user.id, passId: $event.id })
      .subscribe({
        next: (purchase) => {
          this.userPass = this.passes?.find(
            (pass) => pass.id == purchase.passId
          );
          this.displayUserPass();
          $event.isLoading = false;
        },
        error: (err)=>{
          $event.isLoading= false;
        }
      });
  }
  
  openPurchaseModal(pass: PassModel) {
    this.selectedPass = pass;
    this.showCardModal = true;
  }

  handleCardSubmit() {
    console.log("MI A FASZ")
    if (this.selectedPass) {
      this.handlePassPurchase(this.selectedPass);
      this.closeCardModal();
    }
  }

  closeCardModal() {
    this.showCardModal = false;
  }
  highlightOnly = input<boolean>();
  title = input<string>();
  passes: PassModel[] | undefined;
  userPass: PassModel | undefined;
  user: UserModel | undefined;
  constructor(
    private passService: PassService,
    private userService: UserService
  ) {
    passService.allPasses$.subscribe((passes) => {
      this.passes = passes;
      this.displayUserPass();
      this.passes = this.passes?.sort((a, b) => a.price - b.price);
    });
    userService.loggedInUser$.subscribe((user) => {
      this.user = user;
    });
    userService.loggedInUserPass$.subscribe((pass) =>
      this.displayUserPass(pass)
    );
  }

  displayUserPass(pass?: PassModel) {
    this.passes?.forEach((p) => (p.passOfUser = false));
    if (pass) this.userPass = pass;
    if (!this.userPass) return;

    let passOfUser = this.passes?.find((p) => p.id == this.userPass?.id);
    console.log(passOfUser);
    if (passOfUser) {
      passOfUser.passOfUser = true;
      console.log(this.passes);
    } else {
      this.userPass.passOfUser = true;
      this.passes?.push(this.userPass);
    }
  }
}
