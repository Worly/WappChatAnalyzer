import { Component, OnInit } from '@angular/core';
import { Unsubscribable } from 'rxjs';
import { FilterService, FilterType } from '../services/filter.service';

@Component({
  selector: 'app-per-picker',
  templateUrl: './per-picker.component.html',
  styleUrls: ['./per-picker.component.css']
})
export class PerPickerComponent implements OnInit {

  perItems = [
    { value: "none", display: "None" },
    { value: "message", display: "Message" },
    { value: "word", display: "Word" },
    { value: "character", display: "Character" }
  ];

  perObject: { value: string, display: string };
  perReciprocal: boolean;

  timeoutId: number = null;

  private subscriptions: Unsubscribable[] = [];

  constructor(private filterService: FilterService) { }

  ngOnInit(): void {
    this.getValuesFromService();
    this.subscriptions.push(this.filterService.subscribeToFilterChanged([FilterType.PER], () => this.getValuesFromService()));
  }

  ngOnDestroy() {
    while (this.subscriptions.length > 0)
      this.subscriptions.pop().unsubscribe();
  }

  private getValuesFromService() {
    this.perObject = this.perItems.find(i => i.value == this.filterService.statisticFilters.per);
    this.perReciprocal = this.filterService.statisticFilters.perReciprocal;
  }

  onPerChanged(value: { value: string, display: string }) {
    this.perObject = value;
    this.startApplyTimeout(500);
  }

  setReciprocal(reciprocal: boolean) {
    this.perReciprocal = reciprocal;
    this.startApplyTimeout(100);
  }

  startApplyTimeout(ms: number) {
    if (this.timeoutId != null)
      clearTimeout(this.timeoutId);
    this.timeoutId = <number><unknown>setTimeout(() => {
      this.timeoutId = null;
      this.apply();
    }, ms);
  }

  displayFunc(item): string {
    if (!this.perReciprocal || item.value == "none")
      return item.display;
    else
      return item.display + "s";
  }

  private apply() {
    this.filterService.applyPer(this.perObject.value, this.perReciprocal);
  }

}
