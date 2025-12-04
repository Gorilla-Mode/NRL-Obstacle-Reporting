namespace NRLObstacleReporting.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public string? ExceptionType { get; set; }
    public string? ExceptionMessage { get; set; }
}