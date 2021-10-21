import { Component, OnInit } from '@angular/core';
import { StatisticService } from '../services/statistic.service';
import { FilterService } from '../services/filter.service';
import { CustomStatistic } from '../dtos/customStatistic';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  public customStatistics: CustomStatistic[];

  addingNew: boolean = false;
  newStatistic: CustomStatistic = new CustomStatistic();

  constructor(private statisticService: StatisticService, private filterService: FilterService) { }

  ngOnInit(): void {
    this.load();
  }

  load() {
    this.statisticService.getCustomStatistics().subscribe(r => {
      this.customStatistics = r;
    });
  }

  onNewClick() {
    this.newStatistic = new CustomStatistic();
    this.addingNew = true;
  }

  onPropertyChange(property: string, value: string) {
    this.newStatistic[property] = value;
  }

  save() {
    this.addingNew = false;
    this.statisticService.saveCustomStatistic(this.newStatistic).subscribe(r => {
      this.customStatistics.push(r);
    });
  }

  cancel() {
    this.addingNew = false;
  }

}
