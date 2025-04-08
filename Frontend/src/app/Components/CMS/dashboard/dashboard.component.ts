import { Component, OnInit } from '@angular/core';
import { ChartDirective } from '../../../chart-directives/chart.directive';
import { StatisticsService } from '../../../Services/statistics.service';
import { EntryModel } from '../../../Models/entryStatistic.model';
import { PurchaseModel } from '../../../Models/purchaseStatistic.model';
import { scales } from 'chart.js';
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [ChartDirective],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  private primaryColor = '#FA824C';
  private secondaryColor = '#48a8a5';
  private tertiaryColor = '#4281A4';
  private dangerColor = '#f25a5a';
  private mainText = '#FAFFFD';
  private secondaryText = '#D4DBD8';
  private bgSecondary = '#2B272D';
  private gridColor = '#352e3866';

  chartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'top' as const,
        labels: {
          color: this.mainText,
          font: { weight: '500' },
        },
      },
    },
    scales: {
      x: {
        type: 'category',
        ticks: {
          color: this.secondaryText,
          font: { weight: '400' },
        },
        grid: {
          color: this.gridColor,
          drawBorder: false,
        },
      },
      y: {
        
        ticks: {
          color: this.secondaryText,
          font: { weight: '400' },
        },
        grid: {
          color: this.gridColor,
          drawBorder: false,
        },
      },
    },
  };

  months = [
    'Jan',
    'Feb',
    'Mar',
    'Apr',
    'May',
    'Jun',
    'Jul',
    'Aug',
    'Sept',
    'Oct',
    'Nov',
    'Dec',
  ];

  lineChartData: { labels: string[]; datasets: any[] } = {
    labels: [
      'Jan',
      'Feb',
      'Mar',
      'Apr',
      'May',
      'Jun',
      'Jul',
      'Aug',
      'Sept',
      'Oct',
      'Nov',
      'Dec',
    ],
    datasets: [],
  };

  linePurchaseData: { labels: string[]; datasets: any[] } = {
    labels: [
      'Jan',
      'Feb',
      'Mar',
      'Apr',
      'May',
      'Jun',
      'Jul',
      'Aug',
      'Sept',
      'Oct',
      'Nov',
      'Dec',
    ],
    datasets: [],
  };

  barChartData: { labels: string[]; datasets: any[] } = {
    labels: this.months,
    datasets: [],
  };

  pieChartData: { labels: string[]; datasets: any[] } = {
    labels: ['Q1', 'Q2', 'Q3', 'Q4'],
    datasets: [],
  };

  constructor(private statisticsService: StatisticsService) {}
  ngOnInit() {
    this.statisticsService.entries$.subscribe({
      next: (e) => {
        this.updateEntryStatistic(e);
      },
    });
    this.statisticsService.purchases$.subscribe({
      next: (p) => {
        this.updatePurchasesStatistic(p);
      },
    });
  }
  updatePurchasesStatistic(purhcases: PurchaseModel[] | undefined) {
    if (!purhcases) return;
    this.barChartData.datasets = [
      {
        label: 'Bevétel',
        data: purhcases.map((p) => p.income),
        backgroundColor: [`${this.tertiaryColor}`],
        borderColor: this.bgSecondary,
        borderWidth: 1,
        borderRadius: 4,
        stack: 'Stack 0',
      }
    ];
    this.pieChartData.datasets = [
      {
        data: [
          purhcases[0].income + purhcases[1].income + purhcases[2].income,
          purhcases[3].income + purhcases[4].income + purhcases[5].income,
          purhcases[6].income + purhcases[7].income + purhcases[8].income,
          purhcases[9].income + purhcases[10].income + purhcases[11].income,
        ],
        backgroundColor: [
          this.primaryColor,
          this.secondaryColor,
          this.tertiaryColor,
          this.dangerColor,
        ],
        borderColor: this.bgSecondary,
        borderWidth: 2,
        hoverOffset: 8,
      },
    ];
    this.linePurchaseData.datasets = [
      {
        label: 'Vásárlás',
        data: purhcases.map((p) => p.count),
        borderColor: this.secondaryColor,
        tension: 0.4,
        backgroundColor: `${this.secondaryColor}20`,
        fill: true,
        borderWidth: 2,
        pointBackgroundColor: this.bgSecondary,
      }
    ]
  }
  updateEntryStatistic(entries: EntryModel[] | undefined) {
    if (!entries) return;
    this.lineChartData.datasets[0] = {
      label: 'Belépés',
      data: entries.map((e) => e.count),
      borderColor: this.primaryColor,
      tension: 0.4,
      backgroundColor: `${this.primaryColor}20`,
      fill: true,
      borderWidth: 2,
      pointBackgroundColor: this.bgSecondary,
    };
  }
}
