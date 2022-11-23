import { Component, OnInit, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.scss']
})
export class ErrorComponent implements OnInit {

  constructor(private renderer: Renderer2) {
    this.renderer.removeClass(document.body, 'landing-background')
    this.renderer.addClass(document.getElementById('app-container'), 'centered');
  }

  ngOnInit(): void {
  }

}
