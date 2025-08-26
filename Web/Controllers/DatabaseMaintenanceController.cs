using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/admin/db")]
public class DatabaseMaintenanceController : ControllerBase
{
    private readonly IBackupBusiness _biz;
    private readonly ILogger<DatabaseMaintenanceController> _logger;

    public DatabaseMaintenanceController(IBackupBusiness biz, ILogger<DatabaseMaintenanceController> logger)
    {
        _biz = biz;
        _logger = logger;
    }

    // POST api/admin/db/backup
    [HttpPost("backup")]
    [ProducesResponseType(typeof(BackupResultDto), 200)]
    public async Task<IActionResult> Backup([FromBody] BackupRequestDto? dto, CancellationToken ct)
    {
        try
        {
            var result = await _biz.CreateBackupAsync(dto ?? new BackupRequestDto(), ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Backup failed");
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // POST api/admin/db/restore
    [HttpPost("restore")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Restore([FromBody] RestoreRequestDto dto, CancellationToken ct)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.BackupFilePath))
            return BadRequest(new { message = "BackupFilePath is required" });

        try
        {
            await _biz.RestoreBackupAsync(dto, ct);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Restore failed");
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
