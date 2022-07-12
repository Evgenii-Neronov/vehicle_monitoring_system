using System.Threading.Tasks;
using ReportsLib.Models.ReportServer;

namespace WpfApp1.Models
{
    public interface IReportInfoBuilder
    {
        Task<ReportInfo> BuildAsync(ReportModel reportModel);
    }
}
