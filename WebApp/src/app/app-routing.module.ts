import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'case',
    pathMatch: 'full',
  },
  {
    path: 'case',
    loadChildren: () =>
      import('./features/cases/cases.module').then((m) => m.CasesModule),
  },
]

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
