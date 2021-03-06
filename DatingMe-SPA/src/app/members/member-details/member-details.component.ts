import { Component, OnInit, ViewChild } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { IUser } from 'src/app/_models/user';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery-9';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css']
})
export class MemberDetailsComponent implements OnInit {
  user: IUser;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  @ViewChild('memberTabs', {static : true}) membarTabs: TabsetComponent;
  constructor(private userService: UserService, private alertify: AlertifyService, private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    const users = 'users';
    this.activeRoute.data.subscribe(data => {
      this.user = data[users];
    });
    this.activeRoute.queryParams.subscribe(param =>
      {
        // tslint:disable-next-line: no-string-literal
        const selectedTab = param['tab'];
        this.membarTabs.tabs[selectedTab > 0 ? selectedTab : 0].active = true;
      });
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];
    this.galleryImages = this.getImages();
  }
  getImages(){
    const imageUrls = [];
    console.log(this.user.photos);
    for (const photo of this.user.photos) {
      imageUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url,
        description: photo.description
      });
    }
    return imageUrls;
  }
  selectTab(tabId: number){
    this.membarTabs.tabs[tabId].active = true;
  }

  // getUser(){
  //   this.userService.getUserById(+this.activeHttp.snapshot.params['id']).subscribe( (user: IUser) => {
  //     this.user = user;
  //   },
  //    error => {this.alertify.error(error.error);
  //   });
  // }

}
