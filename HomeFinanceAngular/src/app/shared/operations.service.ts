import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NumberValueAccessor } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { lastValueFrom } from 'rxjs';
import { inherits } from 'util';
import { OperationType } from './categories.service';
@Injectable({
  providedIn: 'root'
})
export class OperationsService {

  constructor(private http: HttpClient, private toastr: ToastrService) { }
  readonly BaseURI = 'https://localhost:7080/api/operations'





  postOperation(operation: OperationModel):Promise<void> {
    return this.http.post(this.BaseURI, operation).toPromise().then(
      res => {
        this.toastr.success('Operation added');
      },
      err => {
        console.log(err);
      }
    );
  }



}







export class OperationModelBase{
  id?: bigint=undefined;
  walletId!: bigint;
  operationType: OperationType = OperationType.Expense;
  categoryId?: bigint = undefined;
  walletIdTo?: bigint = undefined;
  amount!:number;
  comment!: string;
}

export class OperationModel extends  OperationModelBase{
  datetime!:Date;
}



export class MonthOverviewModel{
    month!:Date;
    walletId?:bigint=undefined;
    monthBegin!:Date;
    monthDiff!:Date;
    monthEnd!:Date;
    operations!:OperationModel[];
}



