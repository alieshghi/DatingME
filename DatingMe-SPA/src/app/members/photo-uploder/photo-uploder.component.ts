import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IPhoto } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-photo-uploder',
  templateUrl: './photo-uploder.component.html',
  styleUrls: ['./photo-uploder.component.css']
})
export class PhotoUploderComponent implements OnInit {
  @Input() photos: IPhoto[];
  @Output() mainChanged = new EventEmitter();
  baseUrl = environment.apiurl;
  uploader: FileUploader ;
  hasBaseDropZoneOver: boolean;
  hasAnotherDropZoneOver: boolean;
  currentMain: IPhoto;
    constructor( private authService: AuthService, private alertify: AlertifyService, private userService: UserService) {}

  ngOnInit(): void {
    this.initializeUploader();
  }
  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };
    this.uploader.onSuccessItem = (item, response, stause, headers) => {
      if (response) {
        const data: IPhoto = JSON.parse(response);
        const photo = {
          id: data.id,
          url: data.url,
          isMain: data.isMain,
          description: data.description,
          dateAdded: data.dateAdded
        };
        this.photos.push(photo);
        if (photo.isMain) {
        this.authService.changeMemberPhoto(photo.url);
        this.authService.user.photoUrl = photo.url;
        localStorage.setItem('user', JSON.stringify(this.authService.user));
        }
        this.alertify.success('تصویر یا تصاویر جدید آپلود شدند');
      }
    };
  }
    setMainPhoto(photo: IPhoto){
      const userId = this.authService.decodedToken.nameid;
      this.userService.setMainPhoto(userId, photo.id).subscribe(next => {
        this.currentMain = this.photos.filter(x => x.isMain === true)[0];
        this.currentMain.isMain = false;
        this.alertify.success('عکس پروفایل تغییر کرد');
        photo.isMain = true;
        // this.mainChanged.emit(photo.url);
        this.authService.changeMemberPhoto(photo.url);
        this.authService.user.photoUrl = photo.url;
        localStorage.setItem('user', JSON.stringify(this.authService.user));
      }, error => {
        this.alertify.error(error);
      }
      );
    }
    fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }
  deletePhoto(photo: IPhoto){
    const userId = this.authService.decodedToken.nameid;
    this.alertify.confirm('از حذف مطمئنی؟', () => {
      this.userService.deletePhoto(userId, photo.id).subscribe(next => {
        this.photos.splice(this.photos.findIndex(x => x.id === photo.id ), 1);
        this.alertify.success('عکس شما حذف شد');
      }, error => {
        this.alertify.error(error);
      });
    });
  }

}
