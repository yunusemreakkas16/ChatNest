import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChatRoutingModule } from './chat-routing.module';
import { ChatRoomComponent } from './chat-room/chat-room.component';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GroupSettingsComponent } from './group-settings/group-settings.component';
import { CreateChatComponent } from './create-chat/create-chat.component';
import { ChatsComponent } from './chats/chats.component';


@NgModule({
  declarations: [
    ChatRoomComponent,
    GroupSettingsComponent,
    CreateChatComponent,
    ChatsComponent
  ],
  imports: [
    CommonModule,
    ChatRoutingModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class ChatModule { }
