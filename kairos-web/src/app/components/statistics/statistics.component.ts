import {Component, OnInit, ViewChild} from '@angular/core';
import {ChartConfiguration, ChartOptions} from 'chart.js';
import {faArrowTrendDown, faArrowTrendUp} from '@fortawesome/free-solid-svg-icons';
import {NavbarService} from 'src/app/services/navbar/navbar.service';
import {
  IHoursStudiedWeekLabelModel,
  IHoursStudiedWeekModel,
  StatisticsService
} from 'src/app/services/statistics/statistics.service';
import {BaseChartDirective} from "ng2-charts";
import {HttpErrorResponse} from "@angular/common/http";
import {ModalDialogService} from "../../services/modal-dialog/modal-dialog.service";

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit {

  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

  @ViewChild('weeklyChart') weeklyChart?: any;
  @ViewChild('sessionTimeBar') sessionTimeBar?: any;
  @ViewChild('sessionTypeDoughnut') sessionTypeDoughnut?: any;

  faArrowTrend = faArrowTrendUp;
  faArrowTrendDown = faArrowTrendDown;

  public currentRate: number = 0;

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
    responsive: true,
    maintainAspectRatio: false
  };
  public weeklyLineChartLegend = true;


  // Session type doughnut chart
  public typeDoughnutChartLabels: string[] = [
    'Science / Math',
    'Economie',
    'Allemand',
    'Autre'
  ]

  public typeDoughnutChartDatasets: ChartConfiguration<'doughnut'>['data']['datasets'] = [
    {
      data: [5, 7, 9, 11],
      label: "Serie A",
      borderColor: 'rgba(239, 247, 247, 1)',
      backgroundColor: 'rgba(51, 87, 108, 0.85)'
    }
  ]

  public typeDoughnutChartOptions: ChartConfiguration<'doughnut'>['options'] = {
    responsive: true,
    maintainAspectRatio: false
  };


  //Session time bar chart
  public timeBarChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [
      'Science / Math',
      'Economie',
      'Allemand',
      'Autre'
    ],
    datasets: [
      {
        data: [145, 35, 65, 85],
        label: 'Temps de travail / étude',
        borderColor: 'rgba(239, 247, 247, 1)',
        backgroundColor: 'rgba(51, 87, 108, 0.85)'
      }
    ]
  }

  public timeBarChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: true,
    maintainAspectRatio: false
  }

  public timeBarChartLegend = true;
  public timeBarChartPlugins = [];

  public isRateLoaded: boolean = false;
  public isHoursStudiedLoaded: boolean = false;
  public isHoursPerLabelLoaded: boolean = false;

  constructor(public nav: NavbarService,
              private statisticsService: StatisticsService,
              private modalDialogService: ModalDialogService) {
    this.nav.showBackButton();
  }

  ngOnInit(): void {
    this.statisticsService.getCurrentRate().subscribe((rate: number) => {
      this.currentRate = rate;
      this.isRateLoaded = true;
    });

    this.statisticsService.getHoursStudied().subscribe({
        error: (error: HttpErrorResponse) => {
          this.modalDialogService.displayModal(error.error.message)
        },
        next: (hoursStudied: IHoursStudiedWeekModel) => {
          this.weeklyLineChartData.labels = hoursStudied.dayOfWeek;
          this.weeklyLineChartData.datasets[0].data = hoursStudied.hours;

          this.weeklyChart.nativeElement.__ngContext__.directives[0].chart.update()

          this.isHoursStudiedLoaded = true;
        }
      }
    );

    this.statisticsService.getHoursPerLabel().subscribe({
        error: (error: HttpErrorResponse) => {
          this.modalDialogService.displayModal(error.error.message)
        },
        next: (hoursPerLabel: IHoursStudiedWeekLabelModel) => {
          console.log(hoursPerLabel)
          this.timeBarChartData.labels = hoursPerLabel.label;
          this.timeBarChartData.datasets[0].data = hoursPerLabel.hours;

          this.sessionTimeBar.nativeElement.__ngContext__.directives[0].chart.update()
          this.isHoursPerLabelLoaded = true;
        }
      }
    );

  }
}
