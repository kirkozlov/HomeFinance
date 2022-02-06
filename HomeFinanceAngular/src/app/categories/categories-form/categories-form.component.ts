import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CategoriesService, CategoryModel, OperationType } from 'src/app/shared/categories.service';

@Component({
  selector: 'app-categories-form',
  templateUrl: './categories-form.component.html',
  styles: [
  ]
})
export class CategoriesFormComponent implements OnInit {

  @Input() formData: CategoryModel = new CategoryModel();
  @Output() onExitForm = new EventEmitter();

  public possibleExpenseParents: CategoryModel[] = [];
  public possibleIncomeParents: CategoryModel[] = [];
  constructor(public categoriesService: CategoriesService) {
  }

  ngOnInit() {
    this.categoriesService.refreshCategories().then(
      res => {
        this.possibleExpenseParents = this.categoriesService.categories.filter(i => i.operationType == OperationType.Expense && i.id != this.formData.id);
        this.possibleIncomeParents = this.categoriesService.categories.filter(i => i.operationType == OperationType.Income && i.id != this.formData.id);
      });

  }


  onSubmit(form: NgForm) {

    if (this.formData.id == null)
      this.categoriesService.postCategory(form.value)
    else
      this.categoriesService.putCategory(form.value)

    this.onExitForm.emit();
  }

  onDelete() {
    if (this.formData.id == null)
      return;
    this.categoriesService.deleteCategory(this.formData.id)
    this.onExitForm.emit();
  }

}
