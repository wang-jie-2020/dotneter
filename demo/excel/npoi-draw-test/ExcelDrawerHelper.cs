using System;
using System.Collections.Generic;
using NPOI.OpenXmlFormats.Dml;
using NPOI.OpenXmlFormats.Dml.Chart;
using NPOI.SS.UserModel;
using NPOI.SS.UserModel.Charts;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace npoi_draw
{
    /// <summary>
    ///     绘制Excel的组件通用方法封装
    /// </summary>
    internal static class ExcelDrawerHelper
    {
        /*
         *  ******************************************************************************************
         *                                      NPOI
         *  ******************************************************************************************
         */

        /// <summary>
        ///     指定范围下的单元格值相减
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="beginRow"></param>
        /// <param name="endRow"></param>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <returns></returns>
        public static IEnumerable<double> NumericCellRangeMinus(ISheet sheet, int beginRow, int endRow, int col1, int col2)
        {
            var evaluator = sheet.Workbook.GetCreationHelper().CreateFormulaEvaluator();
            for (int index = beginRow; index <= endRow; index++)
            {
                var cellValue1 = evaluator.Evaluate(sheet.GetRow(index).GetCell(col1));
                var cellValue2 = evaluator.Evaluate(sheet.GetRow(index).GetCell(col2));
                if (cellValue1 != null && cellValue1.CellType == CellType.Numeric
                                       && cellValue2 != null && cellValue2.CellType == CellType.Numeric)
                {
                    yield return cellValue2.NumberValue - cellValue1.NumberValue;
                }
                else
                {
                    yield return double.NaN;
                }
            }
        }


        /// <summary>
        ///     绘制lineChart
        /// </summary>
        /// <typeparam name="TX"></typeparam>
        /// <typeparam name="TY"></typeparam>
        /// <param name="chartInfo"></param>
        /// <returns></returns>
        public static IChart CreateLineChart<TX, TY>(ChartInfo<TX, TY> chartInfo)
        {
            IDrawing drawing = chartInfo.WorkSheet.CreateDrawingPatriarch();

            IClientAnchor anchor = drawing.CreateAnchor(0, 0, 0, 0,
                chartInfo.ChartAddress.FirstColumn,
                chartInfo.ChartAddress.FirstRow,
                chartInfo.ChartAddress.LastColumn,
                chartInfo.ChartAddress.LastRow);

            var chart = (XSSFChart)drawing.CreateChart(anchor);
            chart.SetTitle(chartInfo.Title);

            IChartLegend legend = chart.GetOrCreateLegend();
            legend.Position = LegendPosition.TopRight;


            IChartAxis bottomAxis = chart.ChartAxisFactory.CreateCategoryAxis(AxisPosition.Bottom);
            IValueAxis leftAxis = chart.ChartAxisFactory.CreateValueAxis(AxisPosition.Left);
            leftAxis.Crosses = AxisCrosses.AutoZero;

            var chartData = chart.ChartDataFactory.CreateLineChartData<TX, TY>();

            foreach (var source in chartInfo.ChartSources)
            {
                var series = chartData.AddSeries(source.XSource, source.YSource);
                series.SetTitle(source.Title);
            }

            chart.Plot(chartData, bottomAxis, leftAxis);

            //CategoryAxis,间隔坐标轴点
            //var plotArea = chart.GetCTChart().plotArea;
            //var catAx = plotArea.catAx[0];
            //catAx.tickLblSkip = new CT_Skip() { val = 200 };

            //空值 间隔
            chart.SetCTDispBlanksAs(new CT_DispBlanksAs() { val = ST_DispBlanksAs.span });

            //todo 以下标题的效果不好
            var catAx = ((XSSFChart)chart).GetCTChart().plotArea.catAx[0];
            if (catAx != null)
            {
                if (!String.IsNullOrEmpty(chartInfo.XTitle))
                {
                    if (catAx.title == null) catAx.title = new CT_Title();
                    SetAxisTitle(catAx.title, chartInfo.XTitle);
                }
            }

            var valAx = ((XSSFChart)chart).GetCTChart().plotArea.valAx[0];
            if (valAx != null)
            {
                if (!String.IsNullOrEmpty(chartInfo.YTitle))
                {
                    if (valAx.title == null) valAx.title = new CT_Title();
                    SetAxisTitle(valAx.title, chartInfo.YTitle);
                }
            }

            return chart;
        }

        ///// <summary>
        /////     绘制BarChart
        ///// </summary>
        ///// <typeparam name="TX"></typeparam>
        ///// <typeparam name="TY"></typeparam>
        ///// <param name="chartInfo"></param>
        ///// <returns></returns>
        //public static IChart CreateBarChart<TX, TY>(ChartInfo<TX, TY> chartInfo)
        //{
        //    IDrawing drawing = chartInfo.WorkSheet.CreateDrawingPatriarch();

        //    IClientAnchor anchor = drawing.CreateAnchor(0, 0, 0, 0,
        //        chartInfo.ChartAddress.FirstColumn,
        //        chartInfo.ChartAddress.FirstRow,
        //        chartInfo.ChartAddress.LastColumn,
        //        chartInfo.ChartAddress.LastRow);

        //    var chart = (XSSFChart)drawing.CreateChart(anchor);
        //    chart.SetTitle(chartInfo.Title);

        //    IChartLegend legend = chart.GetOrCreateLegend();
        //    legend.Position = LegendPosition.TopRight;


        //    IChartAxis bottomAxis = chart.ChartAxisFactory.CreateCategoryAxis(AxisPosition.Bottom);
        //    IValueAxis leftAxis = chart.ChartAxisFactory.CreateValueAxis(AxisPosition.Left);
        //    leftAxis.Crosses = AxisCrosses.AutoZero;

        //    var chartData = chart.ChartDataFactory.CreateBarChartData<TX, TY>();

        //    foreach (var source in chartInfo.ChartSources)
        //    {
        //        var series = chartData.AddSeries(source.XSource, source.YSource);
        //        series.SetTitle(source.Title);
        //    }

        //    chart.Plot(chartData, bottomAxis, leftAxis);

        //    //CategoryAxis,间隔坐标轴点
        //    //var plotArea = chart.GetCTChart().plotArea;
        //    //var catAx = plotArea.catAx[0];
        //    //catAx.tickLblSkip = new CT_Skip() { val = 200 };

        //    //空值 间隔
        //    chart.SetCTDispBlanksAs(new CT_DispBlanksAs() { val = ST_DispBlanksAs.span });

        //    //todo 以下标题的效果不好
        //    var catAx = ((XSSFChart)chart).GetCTChart().plotArea.catAx[0];
        //    if (catAx != null)
        //    {
        //        if (!String.IsNullOrEmpty(chartInfo.XTitle))
        //        {
        //            if (catAx.title == null) catAx.title = new CT_Title();
        //            SetAxisTitle(catAx.title, chartInfo.XTitle);
        //        }
        //    }

        //    var valAx = ((XSSFChart)chart).GetCTChart().plotArea.valAx[0];
        //    if (valAx != null)
        //    {
        //        if (!String.IsNullOrEmpty(chartInfo.YTitle))
        //        {
        //            if (valAx.title == null) valAx.title = new CT_Title();
        //            SetAxisTitle(valAx.title, chartInfo.YTitle);
        //        }
        //    }

        //    return chart;
        //}

        /// <summary>
        ///     绘制scatterChart
        /// </summary>
        /// <typeparam name="TX"></typeparam>
        /// <typeparam name="TY"></typeparam>
        /// <param name="chartInfo"></param>
        /// <returns></returns>
        public static XSSFChart CreateScatterChart<TX, TY>(ChartInfo<TX, TY> chartInfo)
        {
            IDrawing drawing = chartInfo.WorkSheet.CreateDrawingPatriarch();

            IClientAnchor anchor = drawing.CreateAnchor(0, 0, 0, 0,
                chartInfo.ChartAddress.FirstColumn,
                chartInfo.ChartAddress.FirstRow,
                chartInfo.ChartAddress.LastColumn,
                chartInfo.ChartAddress.LastRow);

            var chart = (XSSFChart)drawing.CreateChart(anchor);
            chart.SetTitle(chartInfo.Title);

            IChartLegend legend = chart.GetOrCreateLegend();
            legend.Position = LegendPosition.TopRight;

            /*
             * CategoryAxis 类目型的坐标轴,在此项目中多数情况下不适用
             */
            //IChartAxis bottomAxis = chart.ChartAxisFactory.CreateCategoryAxis(AxisPosition.Bottom);
            IChartAxis bottomAxis = chart.ChartAxisFactory.CreateValueAxis(AxisPosition.Bottom);

            IValueAxis leftAxis = chart.ChartAxisFactory.CreateValueAxis(AxisPosition.Left);
            leftAxis.Crosses = AxisCrosses.AutoZero;

            var chartData = chart.ChartDataFactory.CreateScatterChartData<TX, TY>();

            foreach (var source in chartInfo.ChartSources)
            {
                var series = chartData.AddSeries(source.XSource, source.YSource);
                series.SetTitle(source.Title);
            }

            chart.Plot(chartData, bottomAxis, leftAxis);

            //CategoryAxis,间隔坐标轴点
            //var plotArea = chart.GetCTChart().plotArea;
            //var catAx = plotArea.catAx[0];
            //catAx.tickLblSkip = new CT_Skip() { val = 200 };

            //空值 间隔
            chart.SetCTDispBlanksAs(new CT_DispBlanksAs() { val = ST_DispBlanksAs.span });

            //todo 以下标题的效果不好
            var valAx = ((XSSFChart)chart).GetCTChart().plotArea.valAx[0];
            if (valAx != null)
            {
                if (!String.IsNullOrEmpty(chartInfo.XTitle))
                {
                    if (valAx.title == null) valAx.title = new CT_Title();
                    SetAxisTitle(valAx.title, chartInfo.XTitle);
                }
            }

            valAx = ((XSSFChart)chart).GetCTChart().plotArea.valAx[1];
            if (valAx != null)
            {
                if (!String.IsNullOrEmpty(chartInfo.YTitle))
                {
                    if (valAx.title == null) valAx.title = new CT_Title();
                    SetAxisTitle(valAx.title, chartInfo.YTitle);
                }
            }

            return chart;
        }

        public static void SetAxisTitle(CT_Title ctTitle, string newTitle)
        {
            CT_Tx ctTx = !ctTitle.IsSetTx() ? ctTitle.AddNewTx() : ctTitle.tx;
            if (ctTx.IsSetStrRef())
                ctTx.UnsetStrRef();
            NPOI.OpenXmlFormats.Dml.Chart.CT_TextBody ctTextBody;
            if (ctTx.IsSetRich())
            {
                ctTextBody = ctTx.rich;
            }
            else
            {
                ctTextBody = ctTx.AddNewRich();
                ctTextBody.AddNewBodyPr();
            }
            CT_TextParagraph ctTextParagraph = ctTextBody.SizeOfPArray() <= 0 ? ctTextBody.AddNewP() : ctTextBody.GetPArray(0);
            if (ctTextParagraph.SizeOfRArray() > 0)
                ctTextParagraph.GetRArray(0).t = newTitle;
            else if (ctTextParagraph.SizeOfFldArray() > 0)
                ctTextParagraph.GetFldArray(0).t = newTitle;
            else
                ctTextParagraph.AddNewR().t = newTitle;
        }
    }

    public class ChartInfo : ChartInfo<double, double>
    {

    }

    public class ChartDataSourceInfo : ChartDataSourceInfo<double, double>
    {

    }

    public class ChartInfo<TX, TY>
    {
        public ISheet WorkSheet { get; set; }

        public string Title { get; set; }

        public CellRangeAddress ChartAddress { get; set; }

        public List<ChartDataSourceInfo<TX, TY>> ChartSources { get; set; } = new List<ChartDataSourceInfo<TX, TY>>();

        public string XTitle { get; set; }

        public string YTitle { get; set; }
    }

    public class ChartDataSourceInfo<TX, TY>
    {
        public string Title { get; set; }

        public IChartDataSource<TX> XSource { get; set; }

        public IChartDataSource<TY> YSource { get; set; }
    }
}
