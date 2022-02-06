import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { UserComponent } from './user/user.component';
import { OverviewComponent } from './overview/overview.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './auth/auth.guard';
import { WalletsComponent } from './wallets/wallets.component';
import { CategoriesComponent } from './categories/categories.component';
const routes: Routes = [
  {path:'',redirectTo:'user/login', pathMatch:'full'},
  {
    path:'home', component:HomeComponent
  },
  {
    path:'overview', component:OverviewComponent, canActivate:[AuthGuard]
  },
  {
    path:'wallets', component:WalletsComponent, canActivate:[AuthGuard]
  },
  {
    path:'categories', component:CategoriesComponent, canActivate:[AuthGuard]
  },
  {
    path:'user', component:UserComponent, 
    children:[
      {path:'registration', component:RegistrationComponent},
      {path:'login', component:LoginComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
