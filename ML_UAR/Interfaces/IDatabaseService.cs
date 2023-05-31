using System.Data;

namespace ML_UAR.Interfaces
{
    public interface IDatabaseService
    {
        IDbConnection GetConnection(string connString = null);
        object Format<T>(HttpRequest req, List<T> data);
        object FormatOnce<T>(List<T> data);
    }
}
