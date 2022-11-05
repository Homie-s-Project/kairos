import { Component, OnInit, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss']
})
export class LandingComponent implements OnInit {
  title: string = 'KAIROS';
  //subTitle: string = 'Contrôler votre temps';
  subTitle: string = 'Prenez contrôle de votre temps';
  subTitleDico: string[];

  constructor(private renderer: Renderer2) {
    this.subTitleDico = ["Contrôler votre temps", "Prenez contrôle de votre temps", "Etudier intelligemment"];
    this.renderer.addClass(document.body, 'landing-background');
    this.renderer.addClass(document.body, 'centered');
   }

  ngOnInit(): void {
    this.subTitle = this.subTitleDico[this.getRandomInt(this.subTitleDico.length)];

    let textDiv = document.getElementById("text-div");
    let buttonDiv = document.getElementById("button-div");

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
}
