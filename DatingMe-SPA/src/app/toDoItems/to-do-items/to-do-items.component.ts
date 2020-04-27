import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-to-do-items',
  templateUrl: './to-do-items.component.html',
  styleUrls: ['./to-do-items.component.css']
})
export class ToDoItemsComponent implements OnInit {
  toDoItems: any;

  constructor( private httpClient: HttpClient) { }

  ngOnInit(): void {
    this.getAll();
  }

  getAll(){
    this.httpClient.get('http://localhost:5000/WeatherForecast/').subscribe(response => {
      this.toDoItems = response;
    }, error => {
      console.log(error);
     });
  }

}
