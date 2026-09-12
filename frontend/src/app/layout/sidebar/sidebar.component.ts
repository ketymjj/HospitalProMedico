import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/services/auth.service';
import { UserRole } from '../../core/models/enums';

interface MenuItem {
  label: string;
  icon: string;
  route: string;
  roles?: UserRole[];
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
  @Input() collapsed = false;
  @Output() navigate = new EventEmitter<void>();

  readonly menuItems: MenuItem[] = [
    { label: 'Home', icon: 'home', route: '/dashboard' },
    { label: 'Pacientes', icon: 'people', route: '/patients' },
    { label: 'Médicos', icon: 'medical_services', route: '/doctors' },
    { label: 'Internações', icon: 'local_hospital', route: '/hospitalizations' },
    { label: 'Cirurgias', icon: 'healing', route: '/surgeries' },
    { label: 'Pagamentos', icon: 'payment', route: '/payments', roles: [UserRole.ADMIN, UserRole.FINANCEIRO] },
    { label: 'Planos de Saúde', icon: 'assignment_ind', route: '/health-plans' },
    { label: 'Consultas', icon: 'event_available', route: '/appointments' },
    { label: 'Prontuários', icon: 'description', route: '/medical-records' },
    { label: 'Relatórios', icon: 'bar_chart', route: '/reports' },
    { label: 'Configurações', icon: 'settings', route: '/settings', roles: [UserRole.ADMIN] }
  ];

  constructor(private authService: AuthService) {}

  isVisible(item: MenuItem): boolean {
    if (!item.roles || item.roles.length === 0) return true;
    return this.authService.hasRole(...item.roles);
  }

  onNavigate(): void {
    this.navigate.emit();
  }
}
