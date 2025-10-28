import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChatRoutingModule } from './chat-routing.module';
import { ChatRoomComponent } from './chat-room/chat-room.component';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GroupSettingsComponent } from './group-settings/group-settings.component';


@NgModule({
  declarations: [
    ChatRoomComponent,
    GroupSettingsComponent
  ],
  imports: [
    CommonModule,
    ChatRoutingModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class ChatModule { }
