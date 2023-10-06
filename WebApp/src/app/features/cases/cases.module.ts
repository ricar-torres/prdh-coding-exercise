import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CasesRoutingModule } from './cases-routing.module';
import { CasesListComponent } from './cases-list/cases-list.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatTableModule } from '@angular/material/table';
import { FormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSortModule } from '@angular/material/sort';
import { MatChipsModule } from '@angular/material/chips';
import { MatCardModule } from '@angular/material/card';
import { NextKeyPipe } from 'src/app/pipes/NextKeyPipe';

@NgModule({
  declarations: [CasesListComponent, NextKeyPipe],
  imports: [
    CommonModule,
    CasesRoutingModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatTableModule,
    FormsModule,
    MatPaginatorModule,
    MatInputModule,
    MatDatepickerModule,
    MatToolbarModule,
    ReactiveFormsModule,
    MatSortModule,
    MatChipsModule,
    MatCardModule,
  ],
})
export class CasesModule {}
