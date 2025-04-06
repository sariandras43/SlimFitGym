import { Component } from '@angular/core';
import { HeroComponent } from '../../Components/hero/hero.component';
import { CounterBubbleComponent } from '../../Components/counter-bubble/counter-bubble.component';
import { SubscriptionsComponent } from '../../Components/subscriptions/subscriptions.component';
import { PromotionComponent } from '../../Components/promotion/promotion.component';
import { RoomService } from '../../Services/room.service';
import { RoomModel } from '../../Models/room.model';
import { MachineService } from '../../Services/machine.service';

@Component({
  selector: 'app-main-page',
  imports: [
    HeroComponent,
    CounterBubbleComponent,
    SubscriptionsComponent,
    PromotionComponent,
  ],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.scss',
})
export class MainPageComponent {
  roomCount = 0;
  fullMaxPeople = 0;
  machineCount = 0;
  constructor(private roomservice: RoomService, private machineService: MachineService) {}
  ngAfterViewInit() {
    this.roomservice.rooms$.subscribe((rooms) => {
      this.roomCount = rooms?.length || 0;
      this.fullMaxPeople = 0;
      rooms?.forEach(r=>{
        this.fullMaxPeople += r.recommendedPeople;
      })
      this.machineService.allMachines$.subscribe(m=>{
        this.machineCount = m?.length || 0;
       
      })
    });
  }
}
