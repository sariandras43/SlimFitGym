import { Component, OnInit } from '@angular/core';
import { ChartDirective } from '../../../chart-directives/chart.directive';
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [ChartDirective],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
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
          font: { weight: '500' }
        }
      }
    },
    scales: {
      x: {
        ticks: { 
          color: this.secondaryText,
          font: { weight: '400' }
        },
        grid: { 
          color: this.gridColor,
          drawBorder: false
        }
      },
      y: {
        ticks: { 
          color: this.secondaryText,
          font: { weight: '400' }
        },
        grid: { 
          color: this.gridColor,
          drawBorder: false
        }
      }
    }
  };

  
  lineChartData = {
    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
    datasets: [{
      label: 'Sales',
      data: [65, 59, 80, 81, 56, 55],
      borderColor: this.primaryColor,
      tension: 0.4,
      backgroundColor: `${this.primaryColor}20`,
      fill: true,
      borderWidth: 2,
      pointBackgroundColor: this.bgSecondary
    }]
  };

  
  barChartData = {
    labels: ['Q1', 'Q2', 'Q3', 'Q4'],
    datasets: [{
      label: 'Revenue',
      data: [542, 480, 600, 793],
      backgroundColor: [
        `${this.tertiaryColor}`,
        `${this.secondaryColor}`
      ],
      borderColor: this.bgSecondary,
      borderWidth: 1,
      borderRadius: 4
    }]
  };

  
  pieChartData = {
    labels: ['Direct', 'Referral', 'Social'],
    datasets: [{
      data: [300, 150, 100],
      backgroundColor: [
        this.primaryColor,
        this.secondaryColor,
        this.tertiaryColor
      ],
      borderColor: this.bgSecondary,
      borderWidth: 2,
      hoverOffset: 8
    }]
  };

  
  doughnutChartData = {
    labels: ['Completed', 'Pending', 'Cancelled'],
    datasets: [{
      data: [45, 30, 25],
      backgroundColor: [
        `${this.primaryColor}`,
        `${this.secondaryColor}`,
        `${this.tertiaryColor}`
      ],
      borderColor: this.bgSecondary,
      borderWidth: 2,
      spacing: 4
    }]
  };


  constructor() { }
  ngOnInit() { }
}