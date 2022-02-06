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
    this.showForm=true;
    this.categoryModel=new CategoryModel();
  }

  categoryModel:CategoryModel=new CategoryModel();

  showForm:boolean=false;

  onExitForm(){
    this.showForm=false;
  }

}
