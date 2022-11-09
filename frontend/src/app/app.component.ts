import { Component} from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  
  settings = require('../../settings.json');
  currentService : string

  ngOnInit() {
    this.currentService = this.settings.currentService.service;
  }

}
