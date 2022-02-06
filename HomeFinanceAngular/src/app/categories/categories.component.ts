import { Component, OnInit } from '@angular/core';
import { CategoriesService, CategoryModel } from '../shared/categories.service';
@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styles: [
  ]
})
export class CategoriesComponent implements OnInit {

  constructor(public categoriesService: CategoriesService) { }



  ngOnInit(): void {
    this.categoriesService.refreshCategories();
  }

  onAddNewCategory(){
    this.categoryModel=new CategoryModel();
    this.showForm=true;
  }

  categoryModel:CategoryModel=new CategoryModel();

  showForm:boolean=false;

  onChangeCategory(categoryModel:CategoryModel){
    this.categoryModel=Object.assign({}, categoryModel);
    this.showForm=true;
  }

  onExitForm(){
    this.showForm=false;
  }

}
