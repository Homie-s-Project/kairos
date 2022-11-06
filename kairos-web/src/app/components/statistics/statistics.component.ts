import { Component, OnInit } from '@angular/core';
import { ChartConfiguration, ChartOptions, ChartType } from 'chart.js';
import { faArrowTrendUp, faArrowTrendDown } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit {
  workRateTxt: string = "Augmentation de 5% ";
  faArrowTrendUp: any = faArrowTrendUp;
  faArrowTrendDown: any = faArrowTrendDown;


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
        data: [35, 5, 45, 120, 35, 65, 10],
        label: 'Durée des sessions',
        fill: true,
        tension: 0.25,
        borderColor: 'rgba(22, 39, 65, 1)',
        backgroundColor: 'rgba(51, 87, 108, 0.85)',
        pointBackgroundColor: 'rgba(51, 87, 108, 1)'
      },
    ],
  };
  public weeklyLineChartOptions: ChartOptions<'line'> = {
    responsive: false,
  };
  public weeklyLineChartLegend = true;


  // Session type doughnut chart
  public typeDoughnutChartLabels: string[] = [
    'Science / Math', 
    'Economie', 
    'Allemand de caca'
  ]

  public typeDoughnutChartDatasets: ChartConfiguration<'doughnut'>['data']['datasets'] = [
    {
      data: [5, 7, 9], 
      label: "Serie A",
      borderColor: 'rgba(239, 247, 247, 1)',
      backgroundColor: 'rgba(51, 87, 108, 0.85)'
    }
  ]

  public typeDoughnutChartOptions: ChartConfiguration<'doughnut'>['options'] = {
    responsive: false,
  };


  //Session time bar chart
  public timeBarChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [
      'Science / Math', 
      'Economie', 
      'Allemand de caca',
      'Science / Math', 
      'Economie', 
      'Allemand de caca'
    ],
    datasets: [
      {
        data: [145, 35, 65, 145, 35, 65],
        label: 'Temps de travail / étude',
        borderColor: 'rgba(239, 247, 247, 1)',
        backgroundColor: 'rgba(51, 87, 108, 0.85)'
      }
    ]
  }

  public timeBarChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: false,
  }

  public timeBarChartLegend = true;
  public timeBarChartPlugins = [];

  constructor() { 
    
  }

  ngOnInit(): void {

  }
}
