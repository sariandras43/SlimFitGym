import { Component } from '@angular/core';
import { PurchaseModel } from '../../../Models/purchase.model';
import { PurchaseService } from '../../../Services/purchase.service';
import { UserService } from '../../../Services/user.service';
import { PassService } from '../../../Services/pass.service';
import { UserModel } from '../../../Models/user.model';
import { PassModel } from '../../../Models/pass.model';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-purhcases',
  imports: [DatePipe],
  templateUrl: './purhcases.component.html',
  styleUrl: './purhcases.component.scss',
})
export class PurhcasesComponent {
  findPassPrice(purhcase: PurchaseModel) {
    return this.passes.find((p) => p.id == purhcase.passId)?.price;
  }
  findPass(purhcase: PurchaseModel) {
    return this.passes.find((p) => p.id == purhcase.passId)?.name;
  }
  findUser(purchase: PurchaseModel) {
    return this.users.find((u) => u.id == purchase.id)?.name;
  }

  purchases: PurchaseModel[] = [];
  users: UserModel[] = [];
  passes: PassModel[] = [];

  constructor(
    private purchaseservice: PurchaseService,
    private userService: UserService,
    private passService: PassService
  ) {}
  ngOnInit() {
    this.purchaseservice.purchases$.subscribe((p) => {
      if (p) this.purchases = p;
    });
    this.userService.getAllUsers().subscribe((usrs) => {
      if (usrs) this.users = usrs;
    });
    this.passService.allPasses$.subscribe((passes) => {
      if (passes) this.passes = passes;
    });
  }
}
