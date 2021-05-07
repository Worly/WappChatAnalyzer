import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { BasicInfoTotal } from '../dtos/basicInfo';
import { AfterAttach, BeforeDetach } from '../services/attach-detach-hooks.service';
import { DataService } from '../services/data.service';
import { FilterService } from '../services/filter.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy, AfterAttach, BeforeDetach {

  public basicInfoTotal: BasicInfoTotal;

  public isLoading: boolean;

  private subscriptions: Subscription[] = [];

  constructor(private dataService: DataService, private filterService: FilterService) { }

  ngOnInit(): void {
    this.subscribeAll();
    this.load();
  }

  ngOnDestroy() {
    this.unsubscribeAll();
  }

  ngAfterAttach() {
    this.subscribeAll();
    this.load();
  }

  ngBeforeDetach() {
    this.unsubscribeAll();
  }

  subscribeAll() {
    this.subscriptions.push(this.filterService.dateFilterChanged.subscribe(() => this.load()));
  }

  unsubscribeAll() {
    while(this.subscriptions.length > 0)
    this.subscriptions.pop().unsubscribe();
  }

  load() {
    this.isLoading = true;
    this.dataService.getBasicInfo().subscribe(r => {
      this.isLoading = false;
      this.basicInfoTotal = r;
    });
  }

}
