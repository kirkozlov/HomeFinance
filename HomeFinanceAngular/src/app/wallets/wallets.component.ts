import { Component, OnInit } from '@angular/core';
import { from,  Observable } from 'rxjs';
import { groupBy, mergeMap, toArray } from 'rxjs/operators';
import { WalletsService } from '../shared/wallets.service';
@Component({
  selector: 'app-wallets',
  templateUrl: './wallets.component.html',
  styles: [
  ]
})
export class WalletsComponent implements OnInit {

  constructor(public walletsService : WalletsService) { }

  ngOnInit(): void {
    this.walletsService.refreshWallets();
  }
  
  onWalletClick(id:any){

  }

}
