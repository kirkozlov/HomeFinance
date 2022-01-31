import { Component, OnInit } from '@angular/core';
import { WalletModel, WalletsService } from 'src/app/shared/wallets.service';

@Component({
  selector: 'app-wallet-form',
  templateUrl: './wallet-form.component.html',
  styles: [
  ]
})
export class WalletFormComponent implements OnInit {

  constructor(public walletsService: WalletsService) { }

  ngOnInit(): void {
  }

  public formData:WalletModel=new WalletModel();

}
