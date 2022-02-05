import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http'
import { from, GroupedObservable, Observable } from 'rxjs';
import { groupBy, mergeMap, toArray } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
@Injectable({
  providedIn: 'root'
})
export class CategoriesService {

  constructor( private http: HttpClient, private toastr:ToastrService) { }
  readonly BaseURI='https://localhost:7080/api/categories'

  public categories: CategoryModel[]=[];

  refreshCategories(){
    this.getCategories().subscribe(
      res=>{
        this.categories=res as CategoryModel[];
      },
      err=>{
        console.log(err);
      }
    
    )
  }

  getCategories(){
    return this.http.get(this.BaseURI);
  }

  postCategory(category:CategoryModel){
    this.http.post(this.BaseURI, category).subscribe(
      res=>{
        this.refreshCategories();
        this.toastr.success('Category added');
      },
      err=>{
        console.log(err);
      }
    );
  }
  putCategory(category:CategoryModel){
    this.http.put(this.BaseURI+"/"+category.id, category).subscribe(
      res=>{
        this.refreshCategories();
        this.toastr.info('Category changed');
      },
      err=>{
        console.log(err);
      }
    );
  }

  deleteCategory(categoryId:bigint){
    this.http.delete(this.BaseURI+"/"+categoryId).subscribe(
      res=>{
        this.refreshCategories();
        this.toastr.error('Category deleted');
      },
      err=>{
        console.log(err);
      }
    );
  }

}

enum OperationType{
  Income, Expense, Transfer
}

export class CategoryModel{
  id?:bigint=undefined;
  name:string='';
  operationType:OperationType=OperationType.Expense;
  parentId?:bigint=undefined;
  comment:string='';
}
