import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';
import { AuthService } from '../../core/services/auth.service';
import { NotificationApiService } from '../../core/services/notification.service';
import { NotificationDto } from '../../core/models/notification.model';
import { ROLE_LABELS } from '../../core/models/enums';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule, MatButtonModule, MatMenuModule, MatBadgeModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  @Output() toggleSidebar = new EventEmitter<void>();

  readonly roleLabels = ROLE_LABELS;
  notifications: NotificationDto[] = [];

  constructor(
    public authService: AuthService,
    private notificationApi: NotificationApiService
  ) {}

  ngOnInit(): void {
    this.loadNotifications();
  }

  get unreadCount(): number {
    return this.notifications.filter((n) => !n.read).length;
  }

  get initials(): string {
    const name = this.authService.currentUser()?.name || '';
    const parts = name.trim().split(' ').filter(Boolean);
    if (parts.length === 0) return 'U';
    if (parts.length === 1) return parts[0].charAt(0).toUpperCase();
    return (parts[0].charAt(0) + parts[parts.length - 1].charAt(0)).toUpperCase();
  }

  loadNotifications(): void {
    this.notificationApi.getAll().subscribe({
      next: (data) => (this.notifications = data),
      error: () => (this.notifications = [])
    });
  }

  markAllAsRead(): void {
    this.notificationApi.markAllAsRead().subscribe(() => this.loadNotifications());
  }

  logout(): void {
    this.authService.logout();
  }
}
