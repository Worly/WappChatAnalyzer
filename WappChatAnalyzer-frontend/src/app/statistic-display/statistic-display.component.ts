import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as CanvasJS from '../../assets/canvasjs.min';
import { appConfig } from '../app.config';
import { Statistic } from "../dtos/statistic";
import { DataService } from '../services/data.service';

let id = 0;

@Component({
  selector: 'app-statistic-display',
  templateUrl: './statistic-display.component.html',
  styleUrls: ['./statistic-display.component.css']
})
export class StatisticDisplayComponent implements OnInit {
  @Input()
  set statisticName(value: string) {
    this.loadAndShowStatistic(value);
  }

  id: number;

  statistic: Statistic;

  displayNames = {
    "NumberOfMessages": "Number of messages",
    "NumberOfWords": "Number of words",
    "NumberOfCharacters": "Number of characters",
    "NumberOfMedia": "Number of media",
    "NumberOfEmojis": "Number of emojis"
  }

  constructor(private dataService: DataService, private route: ActivatedRoute) {
    this.id = id++;
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      if (params.has("statisticName"))
        this.loadAndShowStatistic(params.get("statisticName"));
    });
  }

  loadAndShowStatistic(statisticName: string) {
    this.dataService.getStatistic(statisticName).subscribe((r: Statistic) => {
      this.statistic = r;
      this.renderTotal(r);
      this.renderGraph(r);
    });
  }

  renderTotal(statistic: Statistic) {
    let dataPoints = [];
    let color = 0;
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
    let colorIndex = 0;
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

    colorIndex = 0;

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

    new CanvasJS.Chart("chartContainerGraph" + this.id, {
      animationEnabled: true,
      exportEnabled: false,
      backgroundColor: "rgba(0,0,0,0)",
      legend: {
        fontFamily: "Raleway"
      },
      toolTip: {
        shared: true,
        reversed: true,
        content: this.toolTipContent
      },
      axisY: {
        labelFontFamily: "Raleway",
        labelFontSize: 16,
      },
      axisX: {
        labelFontFamily: "Raleway",
        labelFontSize: 16,
      },
      axisY2: {
        labelFormatter: function () {
          return " ";
        },
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
    }).render();
  }

  toolTipContent(e) {
    var str = "";

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
      var str1 = "<span style= \"color:" + e.entries[i].dataSeries.color + "\"> " + e.entries[i].dataSeries.name + "</span>: <strong>" + e.entries[i].dataPoint.y + "</strong>(" + percentage.toPrecision(2) + "%)<br/>";

      str = str.concat(str1);
    }
    str = str.concat("<span style= \"color: #FC7536\">Total</span>: <strong>" + total + "</strong><br/>");
    str = "<span>" + (e.entries[0].dataPoint.x).toDateString() + "</span><br/>".concat(str);
    return str;
  }
}
