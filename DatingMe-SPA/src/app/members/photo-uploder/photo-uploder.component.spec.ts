import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PhotoUploderComponent } from './photo-uploder.component';

describe('PhotoUploderComponent', () => {
  let component: PhotoUploderComponent;
  let fixture: ComponentFixture<PhotoUploderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PhotoUploderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PhotoUploderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
