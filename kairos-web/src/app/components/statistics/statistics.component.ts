import { Component, OnInit } from '@angular/core';
import Chart from 'chart.js/auto';import { ChartConfiguration, ChartOptions, ChartType } from 'chart.js';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit {
  // Weekly session line chart
  public weeklyLineChartData: ChartConfiguration<'line'>['data'] = {
    labels: [
      'Lundi',
      'Mardi',
      'Mercredi',
      'Jeudi',
      'Vendredi',
      'Samedi',
      'Dimanche',
    ],
    datasets: [
      {
        data: [15, 5, 45, 120, 35, 0, 0],
        label: 'Durée des sessions',
        fill: true,
        tension: 0.25,
        borderColor: 'rgba(22, 39, 65, 1)',
        backgroundColor: 'rgba(51, 87, 108, 0.85)',
        pointBackgroundColor: 'rgba(51, 87, 108, 1)',
      },
    ],
  };
  public weeklylineChartOptions: ChartOptions<'line'> = {
    responsive: false,
  };
  public weeklyLineChartLegend = true;


  // Session type doughnut
  public typeDoughnutChartLabels: string[] = 
  [
    'Science / Math', 
    'Economie', 
    'Allemand de caca'
  ]
  public typeDoughnutChartDatasets: ChartConfiguration<'doughnut'>['data']['datasets'] = 
  [
    {
      data: [5, 7, 9], 
      label: "Serie A"
    }
  ]
  public typeDoughnutChartOptions: ChartConfiguration<'doughnut'>['options'] = {
    responsive: false,
  };

  constructor() { }

  ngOnInit(): void {

  }
}
