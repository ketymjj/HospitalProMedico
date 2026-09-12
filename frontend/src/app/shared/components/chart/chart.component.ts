import {
  AfterViewInit,
  Component,
  ElementRef,
  HostListener,
  Input,
  OnChanges,
  OnDestroy,
  SimpleChanges,
  ViewChild
} from '@angular/core';
import { Chart, ChartConfiguration, ChartType, registerables } from 'chart.js';

Chart.register(...registerables);

const PALETTE = ['#1e50a2', '#00b4c6', '#1fa971', '#f2a900', '#e04848', '#6b5ce7', '#4f7fd1', '#93a5c4'];

@Component({
  selector: 'app-chart',
  standalone: true,
  template: `<div class="chart-wrapper" #wrapper><canvas #canvas></canvas></div>`,
  styles: [`
    .chart-wrapper { position: relative; height: 260px; width: 100%; }
    canvas { display: block; }
  `]
})
export class ChartComponent implements AfterViewInit, OnChanges, OnDestroy {
  @Input() type: ChartType = 'bar';
  @Input() labels: string[] = [];
  @Input() data: number[] = [];
  @Input() label = '';
  @Input() colors: string[] = PALETTE;
  @Input() horizontal = false;

  @ViewChild('canvas') canvasRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('wrapper') wrapperRef!: ElementRef<HTMLDivElement>;

  private chart?: Chart;
  private resizeTimeout?: ReturnType<typeof setTimeout>;

  ngAfterViewInit(): void {
    this.renderChart();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!changes['labels'] && !changes['data'] && !changes['type']) return;
    if (this.chart) {
      this.updateChart();
    }
  }

  ngOnDestroy(): void {
    this.chart?.destroy();
    if (this.resizeTimeout) clearTimeout(this.resizeTimeout);
  }

  @HostListener('window:resize')
  onWindowResize(): void {
    if (this.resizeTimeout) clearTimeout(this.resizeTimeout);
    this.resizeTimeout = setTimeout(() => this.applyCanvasSize(), 200);
  }

  private applyCanvasSize(): void {
    const wrapper = this.wrapperRef?.nativeElement;
    const canvas = this.canvasRef?.nativeElement;
    if (!wrapper || !canvas) return;

    const width = Math.max(wrapper.clientWidth, 1);
    const height = Math.max(wrapper.clientHeight, 1);

    if (this.chart) {
      this.chart.resize(width, height);
    }
  }

  private renderChart(): void {
    const isMulticolor = this.type === 'doughnut' || this.type === 'pie';
    const wrapper = this.wrapperRef.nativeElement;
    const width = Math.max(wrapper.clientWidth, 1);
    const height = Math.max(wrapper.clientHeight, 1);

    const config: ChartConfiguration = {
      type: this.type,
      data: {
        labels: this.labels,
        datasets: [
          {
            label: this.label,
            data: this.data,
            backgroundColor: isMulticolor ? this.colors : this.colors[0],
            borderColor: isMulticolor ? this.colors : this.colors[0],
            borderRadius: this.type === 'bar' ? 6 : 0,
            tension: 0.35,
            fill: this.type === 'line'
          }
        ]
      },
      options: {
        indexAxis: this.horizontal ? 'y' : 'x',
        responsive: false,
        maintainAspectRatio: false,
        animation: false,
        plugins: {
          legend: { display: isMulticolor, position: 'bottom' },
          tooltip: { enabled: true }
        },
        scales: isMulticolor ? {} : {
          y: { beginAtZero: true, ticks: { precision: 0 } }
        }
      }
    };

    this.canvasRef.nativeElement.width = width;
    this.canvasRef.nativeElement.height = height;

    this.chart = new Chart(this.canvasRef.nativeElement, config);
  }

  private updateChart(): void {
    if (!this.chart) return;
    this.chart.data.labels = this.labels;
    this.chart.data.datasets[0].data = this.data;
    this.chart.update();
  }
}
