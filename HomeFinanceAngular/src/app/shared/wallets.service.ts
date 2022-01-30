import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http'
@Injectable({
  providedIn: 'root'
})
export class WalletsService {

  constructor( private http: HttpClient) { }
  readonly BaseURI='https://localhost:7080/api'

  getWallets(){
    return this.http.get(this.BaseURI+'/wallets');
  }

}
