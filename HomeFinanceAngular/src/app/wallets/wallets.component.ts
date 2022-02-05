import { Component, OnInit, ViewChild } from '@angular/core';
import { from,  Observable } from 'rxjs';
import { groupBy, mergeMap, toArray } from 'rxjs/operators';
import { WalletModel, WalletsService } from '../shared/wallets.service';
import { WalletFormComponent } from './wallet-form/wallet-form.component';
@Component({
  selector: 'app-wallets',
  templateUrl: './wallets.component.html',
  styles: [
  ]
})
export class WalletsComponent implements OnInit {

  // @ViewChild(WalletFormComponent)
  // private walletForm!: WalletFormComponent;

  walletModel!:WalletModel;

  constructor(public walletsService : WalletsService) { }

  ngOnInit(): void {
    this.walletsService.refreshWallets();
  }
  
  onWalletClick(walletModel:WalletModel){
    this.showForm=true;
    this.walletModel=Object.assign({}, walletModel);
  }

  showForm:boolean=false;


  onAddNewWallet(){
    this.showForm=true;
    this.walletModel=new WalletModel();
  }

  onExitForm(){
    this.showForm=false;
  }

}
