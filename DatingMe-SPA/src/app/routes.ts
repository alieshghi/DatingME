import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guards/auth.guard';
import { ListsComponent } from './lists/lists.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved.guard';

export const appRoutes: Routes = [

    { path: '' , component: HomeComponent},

    // { path: 'home' , component: HomeComponent},
    {
        path: '' ,
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            {
                path: 'lists', component: ListsComponent
            },
            {
                path: 'members', component : MemberListComponent, resolve: {users: MemberListResolver}
            },
            {
                path: 'members/:id', component : MemberDetailsComponent, resolve: {users: MemberDetailResolver}
            },
            {
                path: 'member/Edit', component : MemberEditComponent,
                resolve: {user: MemberEditResolver}, canDeactivate: [PreventUnsavedChanges]
            },
            {
                path: 'messages', component : MessagesComponent
            }
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];

