using System.Data;
using System.Data.SqlClient;
using ML_UAR.Interfaces;

namespace ML_UAR.Services
{
    public class DatabaseService : IDatabaseService
    {
        private IConfiguration _configuration;
        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection GetConnection(string? connString = null)
        {
            if (connString == null)
            {
                return new SqlConnection(_configuration.GetConnectionString("Default"));
            }
            else
            {
                return new SqlConnection(_configuration.GetConnectionString(connString));
            }
        }

        public object Format<T>(HttpRequest req, List<T> data)
        {
            try
            {
                var draw = req.Form["draw"].FirstOrDefault();
                var start = req.Form["start"].FirstOrDefault();
                var length = req.Form["length"].FirstOrDefault();
                var sortType = req.Form["order[0][dir]"].FirstOrDefault();
                var sortColumnId = req.Form["order[0][column]"].FirstOrDefault();
                var sortColumnName = req.Form["columns[" + sortColumnId + "][data]"].FirstOrDefault();
                var sortColumnDirection = req.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = req.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                return new
                {
                    draw,
                    recordsFiltered = data.Count,
                    recordsTotal = data.Count,
                    data = data.Skip(skip).Take(pageSize),
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object FormatOnce<T>(List<T> data)
        {
            try
            {
                return new
                {
                    draw = 1,
                    recordsFiltered = data.Count,
                    recordsTotal = data.Count,
                    data
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
