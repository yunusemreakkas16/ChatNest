import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {path: '', redirectTo: 'auth/login', pathMatch: 'full'}, //default route
  {path: 'auth', loadChildren: () => import('./features/authenticate/authenticate.module').then(m => m.AuthenticateModule)}, //lazy loading
  {path: 'chats', loadChildren: () => import('./features/chat/chat.module').then(m => m.ChatModule)} //lazy loading
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
