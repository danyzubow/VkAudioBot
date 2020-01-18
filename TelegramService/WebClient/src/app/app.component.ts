import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  name = "";
  audios: AudioModel[] = [new AudioModel("Animal", "Nickelback", 0), new AudioModel("She keeps me up", "Nickelback", 1)];
  constructor(private http: HttpClient) { }
  ngOnInit() {
    const headers1 = new HttpHeaders();
    headers1.append('Access-Control-Allow-Headers', '*');
    headers1.append('Access-Control-Allow-Methods', 'GET');
    headers1.append('Access-Control-Allow-Origin', '*');

    const headers = new HttpHeaders({ 'Access-Control-Allow-Headers': '*', 'Access-Control-Allow-Methods': 'GET', 'Access-Control-Allow-Origin': '*' });
    //this.http.get('http://localhost:5000/Home/GetPlayList', { headers: headers }).subscribe((data: AudioModel[]) => this.audios = data);
  }
  search(numb: number) {
    console.log(numb);
  }
}
export class AudioModel {

  constructor(title: string, artist: string, index: number) {
    this.title = title;
    this.artist = artist;
    this.indexInPlayList = index;
  }
  title: string;
  artist: string;
  allow: boolean;
  indexInPlayList: number;
}