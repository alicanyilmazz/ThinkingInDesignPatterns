using Behavioral.Patterns.Command.BackgroundJob.Commands.Abstracts;
using Behavioral.Patterns.Command.BackgroundJob.Services;

namespace Behavioral.Patterns.Command.BackgroundJob.Commands;

public class GenerateReportCommand : ICommand
{
    private readonly ReportService _reportService;

    private readonly int _customerId;

    public GenerateReportCommand(ReportService reportService,int customerId)
    {
        _reportService = reportService;
        _customerId = customerId;
    }

    public void Execute()
    {
        _reportService.GenerateReport(_customerId);
    }
}
