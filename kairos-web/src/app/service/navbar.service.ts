import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NavbarService {
  isVisible: boolean;
  isBackVisible: boolean;
  isStarted: boolean;

  constructor() {
    this.isVisible = true;
    this.isBackVisible = false;
    this.isStarted = false;
  }

  hideNavbar = () => {
    this.isVisible = false;
  }

  showNavbar = () => {
    this.isVisible = true;
  }
  hideNavbarTongue = () => {
    this.isStarted = false;
  }

  showNavbarTongue = () => {
    this.isStarted = true;
  }
  
  hideBackButton = () => {
    this.isBackVisible = false;
  }

  showBackButton = () => {
    this.isBackVisible = true;
  }
}
