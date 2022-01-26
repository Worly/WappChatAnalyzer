import { Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as CanvasJS from '../../../assets/canvasjs.min';
import { appConfig } from '../../app.config';
import { Statistic } from "../../dtos/statistic";
import { StatisticService } from '../../services/statistic.service';
import { EventService } from '../../services/event.service';
import * as dateFormat from "dateformat";
import { groupBy } from "../../utils";
import { EventInfo } from "../../dtos/event";
import { DateRangeType, FilterService, FilterType, PeriodType } from '../../services/filter.service';
import { AfterAttach, BeforeDetach } from '../../services/attach-detach-hooks.service';
import { Subscription, Unsubscribable } from 'rxjs';

let id = 0;

@Component({
  selector: 'app-statistic-display',
  templateUrl: './statistic-display.component.html',
  styleUrls: ['./statistic-display.component.css']
})
export class StatisticDisplayComponent implements OnInit, OnDestroy, AfterAttach, BeforeDetach {
  private _statisticUrl: string;
  @Input()
  set statisticUrl(value: string) {
    this._statisticUrl = value;
    this.loadAndShowStatistic();
  }

  @ViewChild("chartContainerGraph")
  chartContainerGraph: ElementRef;

  displayName: string;

  isLoading: boolean;

  id: number;

  statistic: Statistic;

  events: { [Key: string]: EventInfo[] } = {};
  eventElements = [];
  eventEmojiSize = 24;

  subscriptions: Unsubscribable[] = [];

  chart;
  pieChart;

  constructor(private statisticService: StatisticService, private eventService: EventService, private route: ActivatedRoute, public filterService: FilterService) {
    this.id = id++;
  }

  ngOnInit() {
    this.subscribeAll();
    this.loadAndShowEvents();
  }

  ngOnDestroy() {
    this.unsubscribeAll();
  }

  ngAfterAttach() {
    this.subscribeAll();
    this.loadAndShowStatistic();
  }

  ngBeforeDetach() {
    this.unsubscribeAll();
  }

  subscribeAll() {
    this.subscriptions.push(this.route.data.subscribe(data => {
      if (data.statisticUrl != null) {
        this._statisticUrl = data.statisticUrl;
        this.loadAndShowStatistic();
      }
      this.displayName = data.displayName;
    }));
    this.subscriptions.push(this.filterService.eventGroupsChanged.subscribe(() => this.loadAndShowEvents()));
    this.subscriptions.push(this.filterService.eventSearchTermChanged.subscribe(() => this.loadAndShowEvents()));
    this.subscriptions.push(this.filterService.subscribeToFilterChanged([FilterType.DATE_RANGE], () => {
      this.loadAndShowEvents();
    }));
    this.subscriptions.push(this.filterService.subscribeToFilterChanged([FilterType.DATE_RANGE, FilterType.GROUPING_PERIOD, FilterType.PER], () => {
      this.loadAndShowStatistic();
    }));
  }

  unsubscribeAll() {
    while (this.subscriptions.length > 0) {
      this.subscriptions.pop().unsubscribe();
    }
  }

  loadAndShowEvents() {
    this.eventService.getEvents().subscribe((r: EventInfo[]) => {
      this.events = groupBy(r, "date");
      this.renderEvents();
    })
  }

  loadAndShowStatistic() {
    this.statistic = null;
    this.isLoading = true;
    if (this.chart != null) {
      this.chart.destroy();
      this.chart = null;
    }
    if (this.pieChart != null) {
      this.pieChart.destroy();
      this.pieChart = null;
    }
    this.eventElements = [];

    this.statisticService.getStatistic(this._statisticUrl).subscribe((r: Statistic) => {
      this.isLoading = false;
      this.statistic = r;
      this.renderTotal(r);
      this.renderGraph(r);
      this.renderEvents();
    });
  }

  renderTotal(statistic: Statistic) {
    let dataPoints = [];
    for (let senderId in statistic.totalBySenders) {
      dataPoints.push({
        name: statistic.senders[senderId].name,
        y: statistic.totalBySenders[senderId],
        color: appConfig.colors[statistic.senders[senderId].name]
      });
    }

    this.pieChart = new CanvasJS.Chart("chartContainerTotal" + this.id, {
      animationEnabled: false,
      exportEnabled: false,
      backgroundColor: "rgba(0,0,0,0)",
      data: [{
        type: "pie",
        startAngle: 90,
        indexLabel: "{name} - {y} (#percent%)",
        indexLabelFontFamily: "Raleway",
        indexLabelFontSize: 17,
        dataPoints: dataPoints
      }]
    });
    this.pieChart.render();
  }

  renderGraph(statistic: Statistic) {
    let data = [];
    for (let senderId in statistic.valuesBySendersOnTimePeriods) {
      let dataSingle = {
        type: "stackedArea100",
        axisYType: "secondary",
        name: statistic.senders[senderId].name,
        color: appConfig.colors[statistic.senders[senderId].name],
        showInLegend: true,
        highlightEnabled: false,
        lineThickness: 0,
        markerType: "none",
        dataPoints: []
      };
      for (let i = 0; i < statistic.valuesBySendersOnTimePeriods[senderId].length; i++) {
        dataSingle.dataPoints.push({
          x: new Date(statistic.timePeriods[i]),
          y: statistic.valuesBySendersOnTimePeriods[senderId][i]
        });
      }
      data.push(dataSingle);
    }

    let dataSingle = {
      type: "column",
      name: "Total",
      color: "#FC7536",
      fillOpacity: 0.8,
      click: e => this.onClickDataPoint(e),
      dataPoints: []
    };
    let stripLines = [];
    let maxStripLines = 20;
    let firstDate = new Date(statistic.timePeriods[0]);
    let lastDate = new Date(statistic.timePeriods[statistic.timePeriods.length - 1]);
    let numberOfPeriodsShowing = this.numberOfPeriodsBetween(firstDate, lastDate, statistic.filter.groupingPeriod);
    let gap = Math.ceil(numberOfPeriodsShowing / maxStripLines);

    for (let i = 0; i < numberOfPeriodsShowing; i += gap) {
      let date = this.addPeriods(firstDate, i, statistic.filter.groupingPeriod)
      stripLines.push({
        value: date,
        label: this.formatDateLabel(date),
        labelPlacement: "outside",
        labelFontColor: "black",
        labelMaxWidth: this.chartContainerGraph.nativeElement.offsetWidth / maxStripLines,
        color: "black",
        labelBackgroundColor: "transparent",
        thickness: 0
      });
    }

    for (let i = 0; i < numberOfPeriodsShowing; i++) {
      let date = this.addPeriods(firstDate, i, statistic.filter.groupingPeriod);
      let index = statistic.timePeriods.findIndex(o => new Date(o).getTime() == date.getTime());
      if (index == -1) {
        dataSingle.dataPoints.push({
          x: date,
          y: 0
        });
      }
      else {
        dataSingle.dataPoints.push({
          x: date,
          y: statistic.totalsOnTimePeriods[index]
        });
      }
    }

    data.push(dataSingle);

    this.chart = new CanvasJS.Chart("chartContainerGraph" + this.id, {
      animationEnabled: false,
      exportEnabled: false,
      backgroundColor: "rgba(0,0,0,0)",
      legend: {
        fontFamily: "Raleway"
      },
      toolTip: {
        shared: true,
        reversed: true,
        content: e => this.toolTipContent(e, this.events)
      },
      axisY: {
        labelFontFamily: "Raleway",
        labelFontSize: 16,
        minimum: 0
      },
      axisX: {
        labelFontFamily: "Raleway",
        labelFormatter: e => "",
        labelFontSize: 16,
        stripLines: stripLines,
        interval: 1,
        intervalType: "day"
      },
      axisY2: {
        labelFormatter: () => " ",
        gridThickness: 0,
        tickLength: 0,
        lineThickness: 0,
        stripLines: [{
          value: 50,
          label: "50%",
          labelAlign: "near",
          labelFontFamily: "Raleway",
          labelFontColor: "black",
          labelFontSize: 20,
          thickness: 2,
          color: "black"
        }]
      },
      data: data
    });
    this.chart.render();
  }

  onClickDataPoint(event) {
    let date = event.dataPoint.x;

    if (this.statistic.filter.groupingPeriod == "date") {
      this.filterService.applyGroupingAndDatePeriodRange("hour", PeriodType.DAY, FilterService.getBackwardsIndexForDate(date, PeriodType.DAY));
    }
    else if (this.statistic.filter.groupingPeriod == "week") {
      this.filterService.applyGroupingAndDatePeriodRange("date", PeriodType.WEEK, FilterService.getBackwardsIndexForDate(date, PeriodType.WEEK));
    }
    else if (this.statistic.filter.groupingPeriod == "month") {
      this.filterService.applyGroupingAndDatePeriodRange("week", PeriodType.MONTH, FilterService.getBackwardsIndexForDate(date, PeriodType.MONTH));
    }
  }

  renderEvents() {
    this.eventElements = [];

    if (this.chart == null)
      return;

    if (this.statistic == null || this.statistic.timePeriods.length == 0 || this.statistic.filter.groupingPeriod == "timeOfDay" || this.statistic.filter.groupingPeriod == "week" || this.statistic.filter.groupingPeriod == "month")
      return;

    var firstDate = this.chart.data[2].dataPoints[0].x;
    var lastDate = this.chart.data[2].dataPoints[this.chart.data[2].dataPoints.length - 1].x;

    this.eventEmojiSize = 0.7 * (this.chart.axisX[0].convertValueToPixel(this.addPeriods(firstDate, 1, this.statistic.filter.groupingPeriod)) - this.chart.axisX[0].convertValueToPixel(firstDate));
    if (this.eventEmojiSize < 18)
      this.eventEmojiSize = 18;
    if (this.eventEmojiSize > 40)
      this.eventEmojiSize = 40;

    let periodCounts = {};

    for (let key in this.events) {
      var date = this.dateToPeriodStart(new Date(key), this.statistic.filter.groupingPeriod);

      if (this.statistic.filter.groupingPeriod == "hour")
        date.setHours(12);

      if (date < firstDate || date > lastDate)
        continue;

      let sortedEvents = this.events[key].sort((a, b) => a.order - b.order);
      for (let i = 0; i < sortedEvents.length; i++) {
        let event = sortedEvents[i];
        var x = this.chart.axisX[0].convertValueToPixel(date);

        let y = -10;
        let yDir = 1;
        if (this.statistic.filter.groupingPeriod != "hour") {
          var chartDataPoint = this.chart.data[2].dataPoints.find(e => e.x.getTime() == date.getTime()) //this.compareDatesWithoutTime(e.x, date));
          if (chartDataPoint == null)
            y = this.chart.axisY[0].convertValueToPixel(0);
          else
            y = this.chart.axisY[0].convertValueToPixel(chartDataPoint.y);
          yDir = -1;
        }

        if (periodCounts[date.toISOString()] == undefined)
          periodCounts[date.toISOString()] = 0;

        this.eventElements.push({
          emoji: event.emoji,
          left: x - this.eventEmojiSize / 2,
          top: y + yDir * (periodCounts[date.toISOString()]++ * this.eventEmojiSize + this.eventEmojiSize + 3)
        });
      }
    }
  }

  compareDatesWithoutTime(first: Date, second: Date): boolean {
    return first.getDate() == second.getDate() &&
      first.getMonth() == second.getMonth() &&
      first.getFullYear() == second.getFullYear();
  }

  toolTipContent(e, events) {
    var str = "";

    if (this.statistic == null)
      return "";

    var date = <Date>e.entries[0].dataPoint.x;

    var total = 0;
    for (var i = e.entries.length - 1; i >= 0; i--) {
      if (e.entries[i].dataSeries.type == "column")
        continue;
      total += e.entries[i].dataPoint.y;
    }

    for (var i = e.entries.length - 1; i >= 0; i--) {
      if (e.entries[i].dataSeries.name == "Total")
        continue;

      var percentage = (e.entries[i].dataPoint.y / total) * 100;
      var str1 = "<span style= \"color:" + e.entries[i].dataSeries.color + "\"> " + e.entries[i].dataSeries.name + "</span>: <strong>" + e.entries[i].dataPoint.y.toLocaleString('en-US', { maximumFractionDigits: 4 }) + "</strong> (" + percentage.toFixed(0) + "%)<br/>";

      str = str.concat(str1);
    }
    let totalEntry = e.entries.find(o => o.dataSeries.name == "Total");
    str = str.concat("<span style= \"color: #FC7536\">Total</span>: <strong>" + totalEntry.dataPoint.y.toLocaleString('en-US', { maximumFractionDigits: 4 }) + "</strong><br/>");

    let dateStr = "";
    if (this.statistic.filter.groupingPeriod == "week") {
      let weekEnd = new Date(date);
      weekEnd.setDate(date.getDate() + 6);
      dateStr = dateFormat(date, "dd/mm") + " - " + dateFormat(weekEnd, "dd/mm/yyyy");
    }
    else if (this.statistic.filter.groupingPeriod == "month") {
      dateStr = dateFormat(date, "mmmm yyyy");
    }
    else if (this.statistic.filter.groupingPeriod == "timeOfDay") {
      dateStr = dateFormat(date, "HH:MM");
    }
    else {
      let dateF = "dddd dd/mm/yyyy";
      if (this.statistic.filter.groupingPeriod == "hour")
        dateF += " HH:MM";
      dateStr = dateFormat(date, dateF);
    }

    str = "<span>" + dateStr + "</span><br/>".concat(str);

    let groups = {};

    for (let key in this.events) {
      var eventDate = this.dateToPeriodStart(new Date(key), this.statistic.filter.groupingPeriod);
      if (this.compareDatesWithoutTime(eventDate, date)) {
        for (let event of this.events[key].sort((a, b) => a.order - b.order)) {
          if (this.statistic.filter.groupingPeriod != "month")
            str = str.concat("<br/><span>" + event.groupName + " " + event.emoji + " " + (event.name != null ? event.name : "") + "</span>");
          else {
            if (groups[event.groupName] == undefined)
              groups[event.groupName] = { emojis: {}, count: 0 };
            groups[event.groupName].count++;
            if (groups[event.groupName].emojis[event.emoji] == undefined)
              groups[event.groupName].emojis[event.emoji] = 0;
            groups[event.groupName].emojis[event.emoji]++;
          }
        }
      }
    }

    if (this.statistic.filter.groupingPeriod == "month") {
      for (let groupName in groups) {
        let mostUsedEmoji;
        for (let emoji in groups[groupName].emojis) {
          if (mostUsedEmoji == undefined || groups[groupName].emojis[emoji] > groups[groupName].emojis[mostUsedEmoji])
            mostUsedEmoji = emoji;
        }

        str = str.concat("<br/><span>" + groupName + " " + mostUsedEmoji + " " + groups[groupName].count + "</span>");
      }
    }

    return str;
  }

  formatDateLabel(e) {
    let format = "";
    let toDate = new Date(this.statistic.filter.toDate);
    let fromDate = new Date(this.statistic.filter.fromDate);
    let filterRangeInMs = <any>toDate - <any>fromDate;
    let filterRangeInHours = filterRangeInMs / 1000 / 60 / 60 + 24;

    let groupingPeriod = this.statistic.filter.groupingPeriod;

    if (groupingPeriod == "date") {
      format = "dd/mm";
      if (toDate.getFullYear() - fromDate.getFullYear() > 0)
        format += "/yyyy";
    }
    else if (groupingPeriod == "timeOfDay") {
      format = "HH:MM";
    }
    else if (groupingPeriod == "hour") {
      format = "";
      if (filterRangeInHours > 24) {
        format += "dd/mm";
        if (toDate.getFullYear() - fromDate.getFullYear() > 0)
          format += "/yyyy";
      }
      format += " HH:MM";
    }
    else if (groupingPeriod == "week") {
      format = "'Week' W";
      if (toDate.getFullYear() - fromDate.getFullYear() > 0)
        format += " yyyy";
    }
    else if (groupingPeriod == "month") {
      format = "mmm";
      if (toDate.getFullYear() - fromDate.getFullYear() > 0)
        format += " yyyy";
    }


    return dateFormat(e, format);
  }

  numberOfPeriodsBetween(fromDate: Date, toDate: Date, groupingPeriod: string): number {
    if (groupingPeriod == "hour" || groupingPeriod == "timeOfDay")
      return Math.round((<any>toDate - <any>fromDate) / 1000 / 60 / 60 + 1);
    else if (groupingPeriod == "date")
      return Math.round((<any>toDate - <any>fromDate) / 1000 / 60 / 60 / 24 + 1);
    else if (groupingPeriod == "week")
      return Math.round((<any>toDate - <any>fromDate) / 1000 / 60 / 60 / 24 / 7 + 1);
    else if (groupingPeriod == "month")
      return Math.round(toDate.getMonth() - fromDate.getMonth() + (toDate.getFullYear() - fromDate.getFullYear()) * 12 + 1);
    else
      throw "groupingPeriod: " + groupingPeriod + " is invalid";
  }

  addPeriods(fromDate: Date, numberOfPeriods: number, groupingPeriod: string): Date {
    var copy = new Date(fromDate);
    if (groupingPeriod == "hour" || groupingPeriod == "timeOfDay")
      copy.setHours(fromDate.getHours() + numberOfPeriods);
    else if (groupingPeriod == "date")
      copy.setDate(fromDate.getDate() + numberOfPeriods);
    else if (groupingPeriod == "week")
      copy.setDate(fromDate.getDate() + 7 * numberOfPeriods);
    else if (groupingPeriod == "month")
      copy.setMonth(fromDate.getMonth() + numberOfPeriods);
    else
      throw "groupingPeriod: " + groupingPeriod + " is invalid";

    return copy;
  }

  dateToPeriodStart(date: Date, groupingPeriod: string): Date {
    var copy = new Date(date);
    copy.setHours(date.getHours(), 0, 0, 0);

    if (groupingPeriod == "date") {
      copy.setHours(0);
    }
    else if (groupingPeriod == "week") {
      var dayOfWeek = (date.getDay() + 6) % 7
      copy.setDate(date.getDate() - dayOfWeek);
      copy.setHours(0);
    }
    else if (groupingPeriod == "month") {
      copy.setDate(1);
      copy.setHours(0);
    }

    return copy;
  }
}
