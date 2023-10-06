import { BehaviorSubject, Observable, catchError, finalize, of } from 'rxjs';
import { CaseSummary } from './cases-summary';
import { DataSource, CollectionViewer } from '@angular/cdk/collections';
import { CasesService } from '../cases/cases.service';

export class CasesDataSource implements DataSource<CaseSummary> {
  private applicationSubject = new BehaviorSubject<any[]>([]);
  private loadingSubject = new BehaviorSubject<boolean>(false);
  public loading$ = this.loadingSubject.asObservable();
  private countSubject = new BehaviorSubject<number>(0);
  public counter$ = this.countSubject.asObservable();

  constructor(
    private casesService: CasesService  ) {}

  loadCases(
    sort: string,
    order: string,
    pageIndex: number,
    pageSize: number,
    startDate?: Date,
    endDate?: Date,
  ) {
    this.loadingSubject.next(true);
    this.casesService
      .getCases(
        sort,
        order,
        pageIndex + 1,
        pageSize,
        startDate,
        endDate
      )
      .pipe(
        catchError(() => of([])),
        finalize(() => {
          this.loadingSubject.next(false);
        })
      )
      .subscribe((result: any) => {
        this.applicationSubject.next(
          result.data
        );
        this.countSubject.next(result.totalPages);
      });
  }

  connect(_collectionViewer: CollectionViewer): Observable<CaseSummary[]> {
    return this.applicationSubject.asObservable();
  }

  disconnect(_collectionViewer: CollectionViewer): void {
    this.applicationSubject.complete();
    this.countSubject.complete();
    this.loadingSubject.complete();
  }
}
