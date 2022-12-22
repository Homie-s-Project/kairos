import { Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { faGoogle, faMicrosoft } from '@fortawesome/free-brands-svg-icons';
import { NavbarService } from 'src/app/services/navbar/navbar.service';
import { TimerService } from 'src/app/services/timer/timer.service';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss']
})
export class LandingComponent implements OnInit, OnDestroy {
  faGoogle = faGoogle;
  faMicrosoft = faMicrosoft;


  title: string = 'KAIROS';
  subTitle: string = 'Prenez contrôle de votre temps';
  subTitleDico: string[];

  constructor(public nav: NavbarService, public timer: TimerService, private renderer: Renderer2, private _router: Router) {
    this.timer.stopCountdown();
    this.subTitleDico = ["Contrôler votre temps", "Prenez contrôle de votre temps", "Etudier intelligemment"];
    this.renderer.addClass(document.body, 'landing-background');
    this.renderer.addClass(document.getElementById('app-container'), 'centered');
   }

  ngOnInit(): void {
    this.nav.hideNavbar();
    this.subTitle = this.subTitleDico[this.getRandomInt(this.subTitleDico.length)];

    let textDiv = document.getElementById("title-container");
    let buttonDiv = document.getElementById("auth-container");

    if(textDiv) {
      textDiv.style.animationPlayState = "running";
      textDiv.classList.remove("hide");
    }
  
    if(buttonDiv) {
      buttonDiv.style.animationPlayState = "running";
      buttonDiv.classList.remove("hide");
    }
  }

  getRandomInt = (max: number) => {
    return Math.floor(Math.random() * max);
  }

  ngOnDestroy(): void {
    this.renderer.removeClass(document.body, 'landing-background');
    this.renderer.removeClass(document.getElementById('app-container'), 'centered');
    this.nav.showNavbar();
  }
}
