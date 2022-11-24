import { Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { NavbarService } from 'src/app/services/navbar/navbar.service';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss']
})
export class LandingComponent implements OnInit, OnDestroy {
  title: string = 'KAIROS';
  subTitle: string = 'Prenez contrôle de votre temps';
  subTitleDico: string[];

  constructor(public nav: NavbarService, private renderer: Renderer2, private _router: Router) {
    this.subTitleDico = ["Contrôler votre temps", "Prenez contrôle de votre temps", "Etudier intelligemment"];
    this.renderer.addClass(document.body, 'landing-background');
    this.renderer.addClass(document.getElementById('app-container'), 'centered');
   }

  ngOnInit(): void {
    this.nav.hide();
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

  // Méthode temporaire pour naviguer sur le timer
  test = () => {
    this.renderer.removeClass(document.body, 'landing-background')
    this.renderer.addClass(document.getElementById('app-container'), 'centered');
    this._router.navigate(['timer'])
  }

  ngOnDestroy(): void {
    this.nav.show();
  }
}
