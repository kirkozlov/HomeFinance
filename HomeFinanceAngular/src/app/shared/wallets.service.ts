import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http'
import { from, GroupedObservable, Observable } from 'rxjs';
import { groupBy, mergeMap, toArray } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
@Injectable({
  providedIn: 'root'
})
export class WalletsService {

  constructor( private http: HttpClient, private toastr:ToastrService) { }
  readonly BaseURI='https://localhost:7080/api/wallets'

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
    return this.http.get(this.BaseURI);
  }

  postWallet(wallet:WalletModel){
    this.http.post(this.BaseURI, wallet).subscribe(
      res=>{
        this.refreshWallets();
        this.toastr.success('Wallet added');
      },
      err=>{
        console.log(err);
      }
    );
  }
  putWallet(wallet:WalletModel){
    this.http.put(this.BaseURI+"/"+wallet.id, wallet).subscribe(
      res=>{
        this.refreshWallets();
        this.toastr.info('Wallet changed');
      },
      err=>{
        console.log(err);
      }
    );
  }

  deleteWallet(walletId:bigint){
    this.http.delete(this.BaseURI+"/"+walletId).subscribe(
      res=>{
        this.refreshWallets();
        this.toastr.error('Wallet deleted');
      },
      err=>{
        console.log(err);
      }
    );
  }

}


export class WalletModel{
  id?:bigint=undefined;
  name:string='';
  groupName:string='';
  comment:string='';
  balance?:number=undefined;
}
