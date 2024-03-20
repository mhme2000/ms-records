using Microsoft.AspNetCore.Mvc;
using Records.Application.Interfaces.Records;
using Records.Domain.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Records.API.Controllers;

[ApiController]
[Route("records")]
public class RecordsController(ICreateRecordUseCase createRecordUseCase,
    IGetDateUserRecordsInfoUseCase getDateUserRecordsInfoUseCase,
    IGetLastMonthlyUserRecordsReports getLastMonthlyUserRecordsReports
) : ControllerBase
{
    private readonly ICreateRecordUseCase _createRecordUseCase = createRecordUseCase;

    private readonly IGetDateUserRecordsInfoUseCase _getDateUserRecordsInfoUseCase = getDateUserRecordsInfoUseCase;

    private readonly IGetLastMonthlyUserRecordsReports _getLastMonthlyUserRecordsReports = getLastMonthlyUserRecordsReports;

    [HttpPost]
    public IActionResult CreateRecord([FromBody] RecordDTO dto)
    {
        var date = _createRecordUseCase.Execute(dto);
        return new ObjectResult(date.ToString()) { StatusCode = (int)HttpStatusCode.Created };
    }

    [HttpGet("user/{userId:guid}/summary")]
    public IActionResult GetDateInfoRecords(Guid userId, [FromQuery][Required] DateOnly date)
    {
            var dayInfoRecords = _getDateUserRecordsInfoUseCase.Execute(userId, date);
        return Ok(dayInfoRecords);
    }

    [HttpGet("user/{userId:guid}/reports/last-monthly")]
    public IActionResult GetMonthlyUserRecordsReports(Guid userId)
    {
        var monthlyUserRecordsReports = _getLastMonthlyUserRecordsReports.Execute(userId);
        return Ok(monthlyUserRecordsReports);
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(DateTime.Now);
    }
}
