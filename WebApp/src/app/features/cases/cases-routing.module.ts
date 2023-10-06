import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CasesListComponent } from './cases-list/cases-list.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'list',
    pathMatch: 'full',
  },
  {
    path: 'list',
    component: CasesListComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CasesRoutingModule { }
