import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-permissions-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './permissions-list.component.html',
  styleUrl: './permissions-list.component.scss'
})
export class PermissionsListComponent {

  permissions = [
    'ProductsView',
    'ProductsCreate',
    'ProductsDelete',
    'ProductsChangeStatus',
    'UsersView',
    'UsersCreate',
    'StatisticsView',
    'ProductStatusHistoriesView',
    'RolesView'
  ];
}