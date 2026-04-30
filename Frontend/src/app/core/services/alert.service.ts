import { Injectable, signal } from '@angular/core';

export type AlertType = 'success' | 'error' | 'warning' | 'info';

export interface AlertMessage {
  type: AlertType;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  alert = signal<AlertMessage | null>(null);

  success(message: string): void {
    this.show('success', message);
  }

  error(message: string): void {
    this.show('error', message);
  }

  warning(message: string): void {
    this.show('warning', message);
  }

  info(message: string): void {
    this.show('info', message);
  }

  clear(): void {
    this.alert.set(null);
  }

  private show(type: AlertType, message: string): void {
    this.alert.set({ type, message });

    setTimeout(() => {
      this.clear();
    }, 4000);
  }
}