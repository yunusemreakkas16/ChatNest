import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatRoomComponent } from './chat-room/chat-room.component';
import { ChatsComponent } from './chats/chats.component';
import { GroupSettingsComponent } from './group-settings/group-settings.component';
import { CreateChatComponent } from './create-chat/create-chat.component';

const routes: Routes = [
  { path: '', component: ChatsComponent }, // chat lists
  { path: 'create-chat', component: CreateChatComponent }, // önce spesifik
  { path: 'group-settings/:id', component: GroupSettingsComponent }, // sonra spesifik
  { path: ':id', component: ChatRoomComponent } // en sona generic
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChatRoutingModule { }
