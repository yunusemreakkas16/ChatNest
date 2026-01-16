import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';


import { AppComponent } from './app.component';
import { AuthenticateModule } from './features/authenticate/authenticate.module';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { UserModule } from './features/user/user.module';
import { FriendshipModule } from './features/friendship/friendship.module';
import { ChatModule } from './features/chat/chat.module';

@NgModule({ declarations: [
        AppComponent
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        AppRoutingModule,
        AuthenticateModule,
        FormsModule,
        UserModule,
        FriendshipModule,
        ChatModule], providers: [
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
        provideHttpClient(withInterceptorsFromDi())
    ] })
export class AppModule { }
