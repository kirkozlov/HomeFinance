import { Component, OnInit } from '@angular/core';
import { WalletsService } from '../shared/wallets.service';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styles: [
  ]
})
export class OverviewComponent implements OnInit {

  wallets:any;

  constructor(private walletsService : WalletsService) { }

  ngOnInit(): void {
    this.walletsService.getWallets().subscribe(
      res=>{
        this.wallets=res;
      },
      err=>{
        console.log(err);
      }

    )
  }

}
