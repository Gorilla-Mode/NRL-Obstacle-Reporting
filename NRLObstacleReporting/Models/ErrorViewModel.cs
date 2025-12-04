namespace NRLObstacleReporting.Models;

public class ErrorViewModel
{
    public string? RequestId { get; init; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public string? ExceptionType { get; init; }
    public string? ExceptionMessage { get; init; }
    public string? HttpStatusCode { get; set; }
}