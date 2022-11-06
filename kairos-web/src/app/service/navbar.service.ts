import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NavbarService {
  isVisible: boolean;
  isBackVisible: boolean

  constructor() {
    this.isVisible = true;
    this.isBackVisible = false; 
  }

  hideNavbar = () => {
    this.isVisible = false;
  }

  showNavbar = () => {
    this.isVisible = true;
  }

  hideBackButton = () => {
    this.isBackVisible = false;
  }

  showBackButton = () => {
    this.isBackVisible = true;
  }
}
