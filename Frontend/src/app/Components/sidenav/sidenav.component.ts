import { Component, HostListener } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../Services/user.service';
import { UserModel } from '../../Models/user.model';

@Component({
  selector: 'app-sidenav',
  imports: [RouterLink],
  templateUrl: './sidenav.component.html',
  styleUrl: './sidenav.component.scss',
})
export class SidenavComponent {
  isMobileExpanded = false;
  private readonly breakpointRem = 70;
  userRole: string = '';

  constructor(private user: UserService, private router: Router) {}
  ngOnInit() {
    this.user.loggedInUser$.subscribe((s) => {
      this.userRole = s?.role || '';
    });
  }
  private getRemInPixels(): number {
    const rootFontSize = parseFloat(
      getComputedStyle(document.documentElement).fontSize
    );
    return this.breakpointRem * rootFontSize;
  }
  logout() {
    this.user.logout();
    this.router.navigate(['/']);
  }
  toggleMobileMenu() {
    this.isMobileExpanded = !this.isMobileExpanded;
  }

  @HostListener('window:resize', ['$event'])
  onResize() {
    if (window.innerWidth > this.getRemInPixels()) {
      this.isMobileExpanded = false;
    }
  }

  @HostListener('mouseleave')
  onMouseLeave() {
    if (window.innerWidth > this.getRemInPixels()) {
      this.isMobileExpanded = false;
    }
  }
}
