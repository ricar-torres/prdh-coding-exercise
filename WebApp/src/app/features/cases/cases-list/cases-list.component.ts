import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { CasesService } from '../cases.service';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { AppService } from 'src/app/services/app.service';
import {
  distinctUntilChanged,
  merge,
  tap,
} from 'rxjs';
import { CasesDataSource } from '../../models/cases-datasource';

@Component({
  selector: 'app-cases-list',
  templateUrl: './cases-list.component.html',
  styleUrls: ['./cases-list.component.css'],
})
export class CasesListComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  pageEventDocuments: PageEvent;
  pageIndex = 0;

  dataSource: CasesDataSource;
  displayedColumns: string[] = [
    'sampleCollectedDate',
    'quantityOfCases',
    'quantityByTestType',
  ];
  startDate: Date;
  endDate: Date;

  form: FormGroup;
  minDate = new Date(2023, 0, 1);
  maxDate = new Date(2023, 5, 30);

  get startDateControl() {
    return this.form.get('startDate');
  }

  get endDateControl() {
    return this.form.get('endDate');
  }

  constructor(
    private formBuilder: FormBuilder,
    private casesService: CasesService,
    private appService: AppService
  ) {
  }

  ngOnInit(): void {
    this.initForm();
    this.dataSource = new CasesDataSource(
      this.casesService,
    );
  }

  ngAfterViewInit(): void {
    this.loadCases();
    this.setupTableListeners();
  }

  initForm() {
    this.form = this.formBuilder.group({
      startDate: [this.minDate],
      endDate: [this.maxDate],
    });
  }

  setupTableListeners() {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
    const observables = [];

    if (this.startDateControl) {
      observables.push(this.startDateControl.valueChanges);
    }

    if (this.endDateControl) {
      observables.push(this.endDateControl.valueChanges);
    }

    merge(...observables)
      .pipe(
        distinctUntilChanged(),
        tap(() => {
          this.paginator.pageIndex = this.pageIndex;
          this.loadCases();
        })
      )
      .subscribe();

    this.dataSource.counter$
      .pipe(
        tap((count) => {
          this.paginator.length = count;
        })
      )
      .subscribe();

    this.dataSource.loading$
      .pipe(
        tap((loading) => {
          this.appService.loading = loading;
        })
      )
      .subscribe();

    this.sort.sortChange.subscribe(
      () => (this.paginator.pageIndex = this.pageIndex)
    );

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(tap(() => this.loadCases()))
      .subscribe();
  }

  loadCases(): void {
    this.dataSource.loadCases(
      this.sort.active,
      this.sort.direction,
      this.paginator.pageIndex,
      this.paginator.pageSize,
      this.startDateControl?.value,
      this.endDateControl?.value
    );
  }

  asNumber(value: any): number {
    return Number(value);
  }
}
