using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace magicodes_epplus_draw_test
{
    internal class EPPlusExcelDrawerHelper
    {
        public static ExcelLineChart CreateLineChart(EPlusChartInfo chartInfo)
        {
            var chart = (ExcelLineChart)chartInfo.WorkSheet.Drawings.AddChart(Guid.NewGuid().ToString(), eChartType.Line);
            chart.SetPosition(chartInfo.ChartAddress.Start.Row, 0, chartInfo.ChartAddress.Start.Column, 0);
            chart.SetSize(800, 400);
            chart.Legend.Position = eLegendPosition.Right;
            chart.Legend.Add();

            chart.Title.Text = chartInfo.Title;
            chart.Title.Font.Size = 15;
            chart.XAxis.Title.Text = chartInfo.XTitle;
            chart.XAxis.Title.Font.Size = 10;
            chart.YAxis.Title.Text = chartInfo.YTitle;
            chart.YAxis.Title.Font.Size = 10;

            chart.DisplayBlanksAs = eDisplayBlanksAs.Span;
            chart.XAxis.RemoveGridlines(true, true);
            chart.YAxis.RemoveGridlines(true, true);
            foreach (var item in chartInfo.ChartSources)
            {
                var chartSeries = (ExcelLineChartSerie)chart.Series.Add(item.YSource, item.XSource);
                chartSeries.Header = item.Title;
                chartSeries.Smooth = true;
            }

            return chart;
        }

        public static ExcelScatterChart CreateScatterChart(EPlusChartInfo chartInfo)
        {
            var chart = (ExcelScatterChart)chartInfo.WorkSheet.Drawings.AddChart(Guid.NewGuid().ToString(), eChartType.XYScatterSmoothNoMarkers);
            chart.SetPosition(chartInfo.ChartAddress.Start.Row, 0, chartInfo.ChartAddress.Start.Column, 0);
            chart.SetSize(800, 400);
            chart.Legend.Position = eLegendPosition.Right;
            chart.Legend.Add();

            chart.Title.Text = chartInfo.Title;
            chart.Title.Font.Size = 15;
            chart.XAxis.Title.Text = chartInfo.XTitle;
            chart.XAxis.Title.Font.Size = 10;
            chart.YAxis.Title.Text = chartInfo.YTitle;
            chart.YAxis.Title.Font.Size = 10;

            chart.DisplayBlanksAs = eDisplayBlanksAs.Span;
            chart.XAxis.RemoveGridlines(true, true);
            chart.YAxis.RemoveGridlines(true, true);

            foreach (var item in chartInfo.ChartSources)
            {
                var chartSeries = (ExcelScatterChartSerie)chart.Series.Add(item.YSource, item.XSource);
                chartSeries.Header = item.Title;
            }

            return chart;
        }
    }

    public class EPlusChartInfo
    {
        public ExcelWorksheet WorkSheet { get; set; }

        public string Title { get; set; }

        public ExcelRange ChartAddress { get; set; }

        public List<EPlusChartDataSourceInfo> ChartSources { get; set; } = new List<EPlusChartDataSourceInfo>();

        public string XTitle { get; set; }

        public string YTitle { get; set; }
    }

    public class EPlusChartDataSourceInfo
    {
        public string Title { get; set; }

        public ExcelRange XSource { get; set; }

        public ExcelRange YSource { get; set; }
    }
}
