using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DevonWay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SafetyAuditController : ControllerBase
{
    // SECURITY ISSUE: Hardcoded credentials
    private const string DbConn = "Server=prod-db.ideagen.internal;Database=DevonWay;User Id=sa;Password=Pr0d_P@ssw0rd!;";

    [HttpGet("{auditId}")]
    public IActionResult GetAudit(string auditId)
    {
        // SECURITY ISSUE: SQL Injection
        using var conn = new SqlConnection(DbConn);
        conn.Open();
        var cmd = new SqlCommand($"SELECT * FROM SafetyAudits WHERE AuditId = '{auditId}'", conn);
        var reader = cmd.ExecuteReader();
        if (reader.Read())
            return Ok(new { Id = reader["AuditId"], Site = reader["SiteName"], Score = reader["ComplianceScore"] });
        return NotFound();
    }

    [HttpPost("generate-report")]
    public IActionResult GenerateReport([FromBody] ReportRequest request)
    {
        // SECURITY ISSUE: Command injection
        Process.Start("cmd.exe", $"/c audit-report.exe --site {request.SiteName} --output {request.OutputPath}");
        return Ok(new { Status = "generated" });
    }
}

public class ReportRequest
{
    public string SiteName { get; set; } = "";
    public string OutputPath { get; set; } = "";
}
