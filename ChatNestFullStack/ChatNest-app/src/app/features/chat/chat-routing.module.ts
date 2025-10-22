import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatRoomComponent } from './chat-room/chat-room.component';
import { ChatsComponent } from './chats/chats.component';

const routes: Routes = [
  { path: '', component: ChatsComponent }, // chat lists
  { path: ':id', component: ChatRoomComponent } // chat room with id
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChatRoutingModule { }
