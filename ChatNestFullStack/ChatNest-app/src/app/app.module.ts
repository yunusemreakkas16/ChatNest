import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';


import { AppComponent } from './app.component';
import { AuthenticateModule } from './features/authenticate/authenticate.module';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { ChatsComponent } from './features/chat/chats/chats.component';
import { FriendListComponent } from './features/friendship/friend-list/friend-list.component';
import { FriendRequestComponent } from './features/friendship/friend-request/friend-request.component';
import { AddFriendComponent } from './features/friendship/add-friend/add-friend.component';

@NgModule({
  declarations: [
    AppComponent,
    ChatsComponent,
    FriendListComponent,
    FriendRequestComponent,
    AddFriendComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthenticateModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor , multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
