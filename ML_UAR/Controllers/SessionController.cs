using Dapper;
using Microsoft.AspNetCore.Mvc;
using ML_UAR.Services;
using ML_UAR.Interfaces;
using ML_UAR.Models;

namespace ML_UAR.Controllers
{
    public class SessionController : Controller
    {
        private IDatabaseService _databaseService;
        public SessionController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<SESSION>> GetPersonnalRole(string personnel_code)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var query = $@"EXEC [dbo].[ST_PersonnelSession_GET] '{personnel_code}'";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<SESSION>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    return data.ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }


        }

    }
}
