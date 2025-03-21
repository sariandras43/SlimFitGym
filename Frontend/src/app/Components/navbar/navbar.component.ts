import { Component, HostListener } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserModel } from '../../Models/user.model';
import { UserService } from '../../Services/user.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  imports:[RouterLink]
})
export class NavbarComponent {
  loggedInUser: UserModel | undefined;
  isMenuOpen = false;

  constructor(
    private authService: UserService,
    private router: Router
  ) {
    this.authService.loggedInUser$.subscribe(user => {
      this.loggedInUser = user;
    });

    // Close menu on navigation
    this.router.events.subscribe(() => {
      this.isMenuOpen = false;
    });
  }

  toggleMenu(): void {
    this.isMenuOpen = !this.isMenuOpen;
  }

  @HostListener('window:resize', ['$event'])
  onResize() {
    if (window.innerWidth > 768) {
      this.isMenuOpen = false;
    }
  }

  @HostListener('document:click', ['$event'])
  onClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (!target.closest('.nav-section') && !target.closest('.menu-toggle')) {
      this.isMenuOpen = false;
    }
  }
}