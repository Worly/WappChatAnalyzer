import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as CanvasJS from '../../assets/canvasjs.min';
import { appConfig } from '../app.config';
import { Statistic } from "../dtos/statistic";
import { DataService } from '../services/data.service';
import { EventService } from '../services/event.service';
import * as dateFormat from "dateformat";
import { groupBy } from "../utils";
import { EventInfo } from "../dtos/event";
import { FilterService } from '../services/filter.service';

let id = 0;

@Component({
  selector: 'app-statistic-display',
  templateUrl: './statistic-display.component.html',
  styleUrls: ['./statistic-display.component.css']
})
export class StatisticDisplayComponent implements OnInit {
  @Input()
  set statisticUrl(value: string) {
    this.loadAndShowStatistic(value);
  }

  displayName: string;

  isLoading: boolean;

  id: number;

  statistic: Statistic;

  events: { [Key: string]: EventInfo[] } = {};
  eventElements = [];
  eventEmojiSize = 24;

  private chart;

  constructor(private dataService: DataService, private eventService: EventService, private route: ActivatedRoute, private filterService: FilterService) {
    this.id = id++;
  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      if (data.statisticUrl != null)
        this.loadAndShowStatistic(data.statisticUrl);
      this.displayName = data.displayName;
    });
    this.loadAndShowEvents();

    this.filterService.eventGroupsChanged.subscribe(() => this.loadAndShowEvents());
  }

  loadAndShowEvents() {
    this.eventService.getEvents().subscribe((r: EventInfo[]) => {
      this.events = groupBy(r, "date");
      this.renderEvents();
    })
  }

  loadAndShowStatistic(statisticUrl: string) {
    this.statistic = null;
    this.isLoading = true;
    this.dataService.getStatistic(statisticUrl).subscribe((r: Statistic) => {
      this.isLoading = false;
      this.statistic = r;
      this.renderTotal(r);
      this.renderGraph(r);
      this.renderEvents();
    });
  }

  renderTotal(statistic: Statistic) {
    let dataPoints = [];
    for (let sender in statistic.totalBySenders) {
      dataPoints.push({
        name: sender,
        y: statistic.totalBySenders[sender],
        color: appConfig.colors[sender]
      });
    }

    new CanvasJS.Chart("chartContainerTotal" + this.id, {
      animationEnabled: true,
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
    }).render();
  }

  renderGraph(statistic: Statistic) {
    let data = [];
    for (let sender in statistic.valuesBySendersOnDates) {
      let dataSingle = {
        type: "stackedArea100",
        axisYType: "secondary",
        name: sender,
        color: appConfig.colors[sender],
        showInLegend: true,
        highlightEnabled: false,
        lineThickness: 0,
        markerType: "none",
        dataPoints: []
      };
      for (let i = 0; i < statistic.valuesBySendersOnDates[sender].length; i++) {
        dataSingle.dataPoints.push({
          x: new Date(statistic.dates[i]),
          y: statistic.valuesBySendersOnDates[sender][i]
        });
      }
      data.push(dataSingle);
    }

    let dataSingle = {
      type: "column",
      name: "Total",
      color: "#FC7536",
      fillOpacity: 0.8,
      dataPoints: []
    };
    for (let i = 0; i < statistic.dates.length; i++) {
      let total = 0;
      for (let sender in statistic.valuesBySendersOnDates)
        total += statistic.valuesBySendersOnDates[sender][i];

      dataSingle.dataPoints.push({
        x: new Date(statistic.dates[i]),
        y: total
      });
    }
    data.push(dataSingle);

    this.chart = new CanvasJS.Chart("chartContainerGraph" + this.id, {
      animationEnabled: true,
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
      },
      axisX: {
        labelFontFamily: "Raleway",
        valueFormatString: "DD/MM/YY",
        labelFontSize: 16,
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

  renderEvents() {
    if (this.chart == null)
      return;

    this.eventElements = [];

    for (let key in this.events) {
      var date = new Date(key);

      for (let i = 0; i < this.events[key].length; i++) {
        let event = this.events[key][i];
        var x = this.chart.axisX[0].convertValueToPixel(date)
        var chartDataPoint = this.chart.data[2].dataPoints.find(e => this.compareDatesWithoutTime(e.x, date));
        if (chartDataPoint == null)
          continue;
        var y = this.chart.axisY[0].convertValueToPixel(chartDataPoint.y);

        this.eventElements.push({
          emoji: event.emoji,
          x: x,
          y: y - i * (this.eventEmojiSize)
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

    var date = <Date>e.entries[0].dataPoint.x

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
      var str1 = "<span style= \"color:" + e.entries[i].dataSeries.color + "\"> " + e.entries[i].dataSeries.name + "</span>: <strong>" + e.entries[i].dataPoint.y.toLocaleString('en-US', { maximumFractionDigits: 2 }) + "</strong> (" + percentage.toFixed(0) + "%)<br/>";

      str = str.concat(str1);
    }
    str = str.concat("<span style= \"color: #FC7536\">Total</span>: <strong>" + total.toLocaleString('en-US', { maximumFractionDigits: 2 }) + "</strong><br/>");
    str = "<span>" + dateFormat(date, "dddd dd/mm/yyyy") + "</span><br/>".concat(str);

    var events = events[dateFormat(date, "isoDate")];
    if (events != null) {
      str = str.concat("<br/>")
      for (let event of events) {
        str = str.concat("<span>" + event.groupName + " " + event.emoji + " " + (event.name != null ? event.name : "") + "</span><br/>");
      }
    }

    return str;
  }
}
