import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import { WalletModel, WalletsService } from 'src/app/shared/wallets.service';

@Component({
  selector: 'app-wallet-form',
  templateUrl: './wallet-form.component.html',
  styles: [
  ]
})
export class WalletFormComponent implements OnInit {

  @Input() formData: WalletModel = new WalletModel();
  @Output() onExitForm = new EventEmitter();

  constructor(public walletsService: WalletsService) { }

  ngOnInit(): void {
  }


  onSubmit(form: NgForm) {

    if (this.formData.id == null)
      this.walletsService.postWallet(form.value)
    else
      this.walletsService.putWallet(form.value)

    this.onExitForm.emit();
  }

  onDelete() {
    if (this.formData.id == null)
      return;
    this.walletsService.deleteWallet(this.formData.id)
    this.onExitForm.emit();
  }


}
