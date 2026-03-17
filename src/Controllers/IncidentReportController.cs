using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DevonWay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidentReportController : ControllerBase
{
    private const string ConnectionString =
        "Server=prod-db.ideagen.internal;Database=DevonWay;User Id=sa;Password=Pr0d_P@ssw0rd!;";

    [HttpGet("{id}")]
    public IActionResult GetReport(string id)
    {
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        var cmd = new SqlCommand($"SELECT * FROM IncidentReports WHERE ReportId = '{id}'", conn);
        var reader = cmd.ExecuteReader();
        if (reader.Read())
            return Ok(new { Id = reader["ReportId"], Title = reader["Title"], Severity = reader["Severity"] });
        return NotFound();
    }

    [HttpPost("export")]
    public IActionResult ExportReport([FromBody] ExportRequest request)
    {
        var process = Process.Start(new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c reportgen.exe --format {request.Format} --output {request.OutputPath}",
            RedirectStandardOutput = true,
            UseShellExecute = false
        });
        process?.WaitForExit();
        return Ok(new { Status = "exported", Path = request.OutputPath });
    }
}

public class ExportRequest
{
    public string Format { get; set; } = "pdf";
    public string OutputPath { get; set; } = "";
}
