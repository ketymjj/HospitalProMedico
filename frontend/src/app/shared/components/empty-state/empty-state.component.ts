import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-empty-state',
  standalone: true,
  imports: [MatIconModule],
  template: `
    <div class="hpm-empty-state">
      <mat-icon>{{ icon }}</mat-icon>
      <strong>{{ title }}</strong>
      <span>{{ message }}</span>
    </div>
  `
})
export class EmptyStateComponent {
  @Input() icon = 'inbox';
  @Input() title = 'Nenhum registro encontrado';
  @Input() message = 'Não há dados para exibir no momento.';
}
