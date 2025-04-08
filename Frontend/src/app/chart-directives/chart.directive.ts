import { 
    Directive, 
    ElementRef, 
    Input, 
    OnChanges, 
    SimpleChanges 
  } from '@angular/core';
  import { Chart, ChartType, registerables } from 'chart.js';
  
  @Directive({
    selector: '[appChart]',
    standalone: true
  })
  export class ChartDirective implements OnChanges {
    @Input({ required: true }) type!: ChartType;
    @Input({ required: true }) data!: any;
    @Input() options!: any;
    
    private chart: Chart | null = null;
  
    constructor(private el: ElementRef) {
      Chart.register(...registerables);
    }
  
    ngOnChanges(changes: SimpleChanges) {
      if (this.chart) {
        this.chart.destroy();
      }
      
      if (this.data && this.type) {
        this.createChart();
      }
    }
  
    private createChart() {
      this.chart = new Chart(this.el.nativeElement, {
        type: this.type,
        data: this.data,
        options: this.options
      });
    }
  
    ngOnDestroy() {
      this.chart?.destroy();
    }
  }