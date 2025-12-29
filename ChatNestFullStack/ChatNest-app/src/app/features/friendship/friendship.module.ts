import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FriendshipRoutingModule } from './friendship-routing.module';
import { FormsModule } from '@angular/forms';
import { AddFriendComponent } from './add-friend/add-friend.component';
import { FriendListComponent } from './friend-list/friend-list.component';
import { FriendRequestComponent } from './friend-request/friend-request.component';



@NgModule({
  declarations: [
    AddFriendComponent,
    FriendListComponent,
    FriendRequestComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    FriendshipRoutingModule
  ]
})
export class FriendshipModule { }
