import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<MemberEditComponent> {
    canDeactivate(memberEditcomponent: MemberEditComponent )
    {
        if (memberEditcomponent.memberEditForm.dirty) {
            return confirm('تغییرات را ذخیره نکردی، آیا مهم نیست!؟');
        }
        return true ;
    }
}
