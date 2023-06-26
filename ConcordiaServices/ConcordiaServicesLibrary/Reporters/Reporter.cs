namespace ConcordiaServicesLibrary.Reporters;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using ConcordiaDBLibrary;
using ConcordiaDBLibrary.Gateways.Abstract;
using ConcordiaDBLibrary.Gateways.Classes;
using ConcordiaDBLibrary.Models.Classes;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.FormulaParsing;
using System.Xml.Linq;
using System.Drawing;

public class Reporter
{
    private readonly ITrelloEntityGateway<Experiment> _experimentsGway;
    private readonly ITrelloEntityGateway<Scientist> _scientistsGway;
    private readonly ITrelloEntityGateway<State> _statesGway;
    private readonly IEntityGateway<Participant> _participantsGway;

    private readonly string _reportName;
    private readonly string _reportPath;

    public string ReportName { get => _reportName; }
    public string ReportPath { get => _reportPath; }

    public Reporter(ITrelloEntityGateway<Experiment> experimentsGway,
                    ITrelloEntityGateway<Scientist> scientistsGway,
                    ITrelloEntityGateway<State> statesGway,
                    IEntityGateway<Participant> participantsGway,
	                string reportName, string reportPath)
    {
        _experimentsGway = experimentsGway;
        _participantsGway = participantsGway;
        _scientistsGway = scientistsGway;
        _statesGway = statesGway;
        _reportName = reportName;
        _reportPath = reportPath;
    }

    public Reporter(ITrelloEntityGateway<Experiment> experimentsGway,
                    ITrelloEntityGateway<Scientist> scientistsGway,
                    ITrelloEntityGateway<State> statesGway,
                    IEntityGateway<Participant> participantsGway)
     : this(experimentsGway, scientistsGway, statesGway, participantsGway, 
	        $"report_{DateTime.UtcNow.ToOADate()}.xlsx", ReporterSettings.ConcordiaReportsPath())
    { }

    public virtual string Report()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var report = Path.Combine(ReportPath, ReportName);
        // Console.WriteLine($"Report [{report}].");
        using var package = new ExcelPackage(new FileInfo(report));
        if (package.Workbook.Worksheets[ReportName] is not null)
        {
            var existingWorksheet = package.Workbook.Worksheets[ReportName];
            package.Workbook.Worksheets.Delete(existingWorksheet);
        }
        var worksheet = package.Workbook.Worksheets.Add(ReportName);
        var experiments = _experimentsGway.GetAll();
        var states = _statesGway.GetAll();
        // create values for the first section
        // set the values for exsInStart
        var stateStart = states.SingleOrDefault(e => e.Name.Equals(DBSettings.GetStatesNames()[0], StringComparison.OrdinalIgnoreCase));
        var expsInStart = experiments.Count(e => e.StateId == stateStart!.Id);
        // Console.WriteLine($"State [{stateStart}] and experiments count: {expsInStart}.");
        // set the values for exsInWorking
        var stateWorking = states.SingleOrDefault(e => e.Name.Equals(DBSettings.GetStatesNames()[1], StringComparison.OrdinalIgnoreCase));
        var expsInWorking = experiments.Count(e => e.StateId == stateWorking!.Id);
        // Console.WriteLine($"State [{stateWorking}] and experiments experiments count: {expsInWorking}.");
        // set the values for exsInFinish
        var stateFinish = states.SingleOrDefault(e => e.Name.Equals(DBSettings.GetStatesNames()[2], StringComparison.OrdinalIgnoreCase));
        var expsInFinish = experiments.Count(e => e.StateId == stateFinish!.Id);
        // Console.WriteLine($"State [{stateFinish}] and count {expsInFinish}.");
        // set first header
        worksheet.Cells["A1:D1"].Merge = true;
        worksheet.Cells["A1:D1"].Value = "TASKS REPORT";
        worksheet.Cells["A1:D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        worksheet.Cells["A3:D3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        // set width and alignment
        for (int c = 1; c <= 4; c++)
        {
            worksheet.Column(c).Width = 30;
            worksheet.Cells[2, c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }
        // set first section titles
        worksheet.Cells["A2"].Value = "STATES";
        worksheet.Cells["B2"].Value = $"{DBSettings.GetStatesNames()[0]} (DA INIZIARE)";
        worksheet.Cells["C2"].Value = $"{DBSettings.GetStatesNames()[1]} (IN LAVORAZIONE)";
        worksheet.Cells["D2"].Value = $"{DBSettings.GetStatesNames()[2]} (CONCLUSI)";
        // set first section values
        worksheet.Cells["A3"].Value = "TASKS";
        worksheet.Cells["B3"].Value = expsInStart.ToString();
        worksheet.Cells["C3"].Value = expsInWorking.ToString();
        worksheet.Cells["D3"].Value = expsInFinish.ToString();
        // create the values for second section
        var scientists = _scientistsGway.GetAll().ToList();
        var participants = _participantsGway.GetAll().ToList();
        var data = new List<(Scientist Scientist, int Completed, int Total, double? Percentage)>();
        foreach (var scientist in scientists)
        {
            var started = participants.Count(e => e.Experiment!.StateId == stateStart!.Id && e.ScientistId == scientist.Id);
            var working = participants.Count(e => e.Experiment!.StateId == stateWorking!.Id && e.ScientistId == scientist.Id);
            var finished = participants.Count(e => e.Experiment!.StateId == stateFinish!.Id && e.ScientistId == scientist.Id);
            var total = started + working + finished;
            double? percentage = total > 0 ? (double)finished / (double)total * 100 : null;
            // Console.WriteLine($"Started [{started}], Working [{working}], Finished [{finished}].");
            // Console.WriteLine($"Scientist [{scientist}], Completed [{finished}], Total [{total}], Percentage [{percentage}].");
            data.Add((scientist, finished, total, percentage));
        }
        data = data.OrderBy(d => d.Scientist.FullName).OrderByDescending(d => d.Percentage).ToList();
        // set space
        worksheet.Cells["A4:D4"].Merge = true;
        worksheet.Cells["A4:D4"].Value = string.Empty;
        worksheet.Cells["A4:D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        // set second header
        worksheet.Cells["A5:D5"].Merge = true;
        worksheet.Cells["A5:D5"].Value = "SCIENTIST REPORT";
        worksheet.Cells["A5:D5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        // set second section titles
        worksheet.Cells["A6"].Value = "CODE";
        worksheet.Cells["A6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        worksheet.Cells["B6"].Value = "FULL NAME";
        worksheet.Cells["B6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        worksheet.Cells["C6"].Value = "DATA";
        worksheet.Cells["C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        worksheet.Cells["D6"].Value = "PERCENTAGE";
        worksheet.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        // set second section values
        var i = 7;
        foreach ((Scientist scientist, int finished, int total, double? percentage) in data)
        {
            worksheet.Cells[$"A{i}"].Value = scientist.Code;
            worksheet.Cells[$"B{i}"].Value = scientist.FullName;
            worksheet.Cells[$"c{i}"].Value = $"finished {finished} over {total} assigned.";

            if (percentage is null) worksheet.Cells[$"D{i}"].Value = "NULL";
            else worksheet.Cells[$"D{i}"].Value = $"{Math.Round((double)percentage, 2, MidpointRounding.AwayFromZero).ToString("0.00")}%";

            worksheet.Cells[$"D{i}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            string backgroundColor = percentage switch
            {
                double p when p >= 60 => "#008000",
                double p when p >= 30 => "#ffa500",
                double p when p >= 0 => "#FF0000",
                _ => "#808080",
            };
            worksheet.Cells[$"D{i}"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColor));
            i++;
        }
        package.SaveAs(new FileInfo(report));
        return report;
    }
}