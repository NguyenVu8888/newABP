import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { LayoutService } from './service/app.layout.service';
import { AuthService } from '../shared/services/auth.service';
import { Router } from '@angular/router';
import { LOGIN_URL } from '../shared/constants/urls.contants';

@Component({
  selector: 'app-topbar',
  templateUrl: './app.topbar.component.html',
})
export class AppTopBarComponent implements OnInit {
  items!: MenuItem[];
  userMenuItems: MenuItem[];

  @ViewChild('menubutton') menuButton!: ElementRef;

  @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

  @ViewChild('topbarmenu') menu!: ElementRef;

  constructor(
    public layoutService: LayoutService,
    private authService: AuthService,
    private route: Router
  ) {}

  ngOnInit() {
    this.userMenuItems = [
      {
        label: 'thong tin ca nha',
        icon: 'pi pi-id-card',
        routerLink: ['/profile'],
      },
      {
        label: 'doi mat khau',
        icon: 'pi pi-key',
        routerLink: ['/change-password'],
      },
      {
        label: 'dang xuat',
        icon: 'pi pi-sign-out',
        command: event => {
          this.authService.logout();
          this.route.navigate([LOGIN_URL]);
        },
      },
    ];
  }
}