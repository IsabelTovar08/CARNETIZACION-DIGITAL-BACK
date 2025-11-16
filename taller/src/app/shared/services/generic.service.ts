import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EntityName } from './enums/EntityName';
import { environment } from '../../../environments/environments';

@Injectable({
  providedIn: 'root'
})
export class GenericService <TList, TCreate, TUpdate> {

  private readonly baseUrl: string;
  constructor(
    private readonly http: HttpClient,
    private readonly entity: EntityName
  ) {
    this.baseUrl = `${environment.apiUrl}/${entity}`;
  }

  getAll(params?: Record<string, any>): Observable<TList[]> {
    let httpParams = new HttpParams();
    if (params) {
      Object.entries(params).forEach(([key, value]) => {
        httpParams = httpParams.append(key, value);
      });
    }
    return this.http.get<TList[]>(this.baseUrl, { params: httpParams });
  }

  getById(id: number | string): Observable<TList> {
    return this.http.get<TList>(`${this.baseUrl}/${id}`);
  }

  create(data: TCreate): Observable<TList> {
    return this.http.post<TList>(this.baseUrl, data);
  }

  update(id: number | string, data: TUpdate): Observable<TList> {
    return this.http.put<TList>(`${this.baseUrl}/${id}`, data);
  }

  delete(id: number | string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
