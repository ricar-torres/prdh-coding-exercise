import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CaseSummary } from '../models/cases-summary';

@Injectable({
  providedIn: 'root',
})
export class CasesService {
  private loadingSubject = new BehaviorSubject<boolean>(false);

  public loading$ = this.loadingSubject.asObservable();

  private countSubject = new BehaviorSubject<number>(0);
  public counter$ = this.countSubject.asObservable();

  constructor(private http: HttpClient) {}

  getCases(
    sort = 'asc',
    order = '',
    pageNumber = 0,
    pageSize = 0,
    startDate?: Date,
    endDate?: Date
  ): Observable<CaseSummary[]> {
    let params = new HttpParams()
      .set('sort', sort)
      .set('order', order)
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (startDate) {
      params = params.set('startDate', startDate.toISOString());
    }

    if (endDate) {
      params = params.set('endDate', endDate.toISOString());
    }

    return this.http.get<CaseSummary[]>(`${environment.baseURL}/cases/list`, {
      params: params,
    });
  }
}
