import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http'
import { from, GroupedObservable, Observable } from 'rxjs';
import { groupBy, mergeMap, toArray } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class WalletsService {

  constructor( private http: HttpClient) { }
  readonly BaseURI='https://localhost:7080/api'

  public wallets: WalletModel[]=[];
  public walletGroups: Observable<WalletModel[][]>=new Observable();

  refreshWallets(){
    this.getWallets().subscribe(
      res=>{
        this.wallets=res as WalletModel[];
        this.walletGroups=from(this.wallets).pipe(groupBy(g=>g.groupName), mergeMap(g=>g.pipe(toArray()))).pipe(toArray());
      },
      err=>{
        console.log(err);
      }
    )
  }

  getWallets(){
    return this.http.get(this.BaseURI+'/wallets');
  }

}


export class WalletModel{
  id?:bigint=undefined;
  name:string='';
  groupName:string='';
  comment:string='';
  balance?:number=undefined;
}
