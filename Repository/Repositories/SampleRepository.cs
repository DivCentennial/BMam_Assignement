using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MariApps.MS.Training.MSA.EmployeeMS.Repository.Contracts.DbContext;
using MariApps.MS.Training.MSA.EmployeeMS.Repository.Contracts.Repositories;

namespace MariApps.MS.Training.MSA.EmployeeMS.Repository.Repositories
{
    public class SampleRepository : ISampleRepository
    {
        private readonly IAdoDbContext _dbContext;

        public SampleRepository(IAdoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GetData()
        {
            var result = _dbContext.ExecuteScalar<string>("SELECT 'Sample Data'", System.Data.CommandType.Text);
            return result;
        }
    }
}
