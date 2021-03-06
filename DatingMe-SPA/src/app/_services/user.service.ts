import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IUser } from '../_models/user';
import { PaginationResult } from '../_models/Pagination';
import { map } from 'rxjs/Operators';
import { IMessage } from '../_models/message';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiurl;

  constructor(private http: HttpClient) {
   }
   getUsers(currentPage?, pageSize?, userParams?, likeFilter? ): Observable<PaginationResult<IUser[]>> {
     const paginatedUser: PaginationResult<IUser[]> = new PaginationResult<IUser[]>();
     let params = new HttpParams();
     if (currentPage != null && pageSize != null) {
       params = params.append('curentPage', currentPage);
       params = params.append('pageSize', pageSize);
     }

     if (userParams != null ) {
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('city', userParams.city);
      params = params.append('cuntry', userParams.cuntry);
      params = params.append('gender', userParams.gender);
      params = params.append('sortOrder', userParams.sortOrder);
      params = params.append('sortType', userParams.sortType);
     }
     if (likeFilter === 'Likers') {
      params = params.append('likers', 'true');
     }
     if (likeFilter === 'Likeds') {
      params = params.append('likeds', 'true');
    }
     return this.http.get<IUser[]>(this.baseUrl + 'users', {observe: 'response', params})
    .pipe(
      map( response => {
        paginatedUser.pageResult = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedUser.paginationResult = JSON.parse(response.headers.get('pagination'));
        }
        return paginatedUser;
      })
    );
  }
  getUserById(id: number): Observable<IUser> {
    return this.http.get<IUser>(this.baseUrl + 'users/' + id);
  }
  saveUser(id: number, user: IUser){
    return this.http.put(this.baseUrl + 'users/' + id, user);
  }
  setMainPhoto(userId: number, id: number){
    return this.http.post(this.baseUrl + 'users/' + userId + '/photos/' + id + '/setMain', {});
  }
  deletePhoto(userId: number, id: number){
    return this.http.delete(this.baseUrl + 'users/' + userId + '/photos/' + id);
  }
  createLike(userId: number, recievedId: number ){
    return this.http.post(this.baseUrl + 'users/' + userId + '/likes/' + recievedId, {});
  }
  gethMessageThread(userId: number, recipientId: number): Observable<IMessage[]>{
    return this.http.get<IMessage[]>(this.baseUrl + 'users/' + userId + '/message/thread/' + recipientId);
  }
  createMessage(userId: number, message: IMessage){
    return this.http.post(this.baseUrl + 'users/' + userId + '/message', message);
  }
  deleteMessage(userId: number, id: number){
    return this.http.post(this.baseUrl + 'users/' + userId + '/message/' + id, {});
  }
  markAsRead(userId: number, id: number){
    return this.http.post(this.baseUrl + 'users/' + userId + '/message/' + id + '/read', {})
    .subscribe();
  }
  getMessages(userId: number, currentPage?, pageSize?, messageContainer?: string): Observable<PaginationResult<IMessage[]>>{
    const paginatedMessage: PaginationResult<IMessage[]> = new PaginationResult<IMessage[]>();
    let params = new HttpParams();
    if (currentPage != null && pageSize != null) {
       params = params.append('curentPage', currentPage);
       params = params.append('pageSize', pageSize);
     }
    if (messageContainer == null) {
      messageContainer = 'unRead';
     }
    params = params.append('messageContainer', messageContainer);
    return this.http.get<IMessage[]>(this.baseUrl + 'users/' + userId + '/message', { observe: 'response', params })
    .pipe( map(
      response => {
        paginatedMessage.pageResult = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedMessage.paginationResult = JSON.parse(response.headers.get('pagination'));
        }
        return paginatedMessage;
      }
    ));
  }
}
