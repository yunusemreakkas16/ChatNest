import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FriendListComponent } from './friend-list/friend-list.component';
import { FriendRequestComponent } from './friend-request/friend-request.component';
import { AddFriendComponent } from './add-friend/add-friend.component';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: 'list', component: FriendListComponent },
      {path: 'requests', component: FriendRequestComponent },
      {path: 'addFriend', component: AddFriendComponent }
      ]
    )
  ],
  exports: [RouterModule]
})

export class FriendshipRoutingModule {}
