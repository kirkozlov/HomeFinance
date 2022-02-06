import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { from, GroupedObservable, Observable, Subscription } from 'rxjs';
import { groupBy, mergeMap, toArray } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
@Injectable({
  providedIn: 'root'
})
export class CategoriesService {

  constructor(private http: HttpClient, private toastr: ToastrService) { }
  readonly BaseURI = 'https://localhost:7080/api/categories'

  public categories: CategoryModel[] = [];
  public categoryGroups: CategoryGroup[] = [];

  refreshCategories():Promise<void> {
     return this.getCategories().then(
      res => {
        this.categories = res as CategoryModel[];

        this.categoryGroups=[];

        var groupsToFulfill: CategoryGroup[] = [];

        this.categories.filter(s => s.parentId == null).forEach(element => {
          let group = new CategoryGroup();
          group.categoryParent = element;
          this.categoryGroups.push(group);
          groupsToFulfill.push(group);
        });

        while (groupsToFulfill.length > 0) {
          var groupParent=groupsToFulfill.pop();
          this.categories.filter(s => s.parentId == groupParent?.categoryParent.id).forEach(element => {
            let group = new CategoryGroup();
            group.categoryParent = element;
            groupParent?.categoryChildren.push(group);
            groupsToFulfill.push(group);
          });

        }
      },
      err => {
        console.log(err);
      }

    )
  }

  getCategories() {
    return this.http.get(this.BaseURI).toPromise();
  }

  postCategory(category: CategoryModel) {
    this.http.post(this.BaseURI, category).subscribe(
      res => {
        this.refreshCategories();
        this.toastr.success('Category added');
      },
      err => {
        console.log(err);
      }
    );
  }
  putCategory(category: CategoryModel) {
    this.http.put(this.BaseURI + "/" + category.id, category).subscribe(
      res => {
        this.refreshCategories();
        this.toastr.info('Category changed');
      },
      err => {
        console.log(err);
      }
    );
  }

  deleteCategory(categoryId: bigint) {
    this.http.delete(this.BaseURI + "/" + categoryId).subscribe(
      res => {
        this.refreshCategories();
        this.toastr.error('Category deleted');
      },
      err => {
        console.log(err);
      }
    );
  }

}

export enum OperationType {
  Income, Expense, Transfer
}

export class CategoryGroup {
  public categoryParent!: CategoryModel;
  public categoryChildren: CategoryGroup[] = [];
}

export class CategoryModel {
  id?: bigint = undefined;
  name: string = '';
  parentId?: bigint = undefined;
  operationType: OperationType = OperationType.Expense;
  comment: string = '';
}
