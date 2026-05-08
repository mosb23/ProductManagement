import {
  Directive,
  Input,
  TemplateRef,
  ViewContainerRef,
  effect,
  inject
} from '@angular/core';

import { AuthService } from '../../core/services/auth/auth.service';

@Directive({
  selector: '[appHasPermission]',
  standalone: true
})
export class HasPermissionDirective {
  private readonly templateRef = inject(TemplateRef<unknown>);
  private readonly viewContainer = inject(ViewContainerRef);
  private readonly authService = inject(AuthService);

  private permission: string | string[] = '';
  private hasView = false;

  constructor() {
    effect(() => {
      this.authService.currentUser();
      this.updateView();
    });
  }

  @Input()
  set appHasPermission(permission: string | string[]) {
    this.permission = permission;
    this.updateView();
  }

  private updateView(): void {
    const hasPermission = Array.isArray(this.permission)
      ? this.authService.hasAnyClaim(this.permission)
      : this.authService.hasClaim(this.permission);

    if (hasPermission && !this.hasView) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasView = true;
      return;
    }

    if (!hasPermission && this.hasView) {
      this.viewContainer.clear();
      this.hasView = false;
    }
  }
}