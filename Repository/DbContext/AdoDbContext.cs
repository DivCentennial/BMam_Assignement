using MariApps.Framework.Core.Abstractions.Contracts;
using MariApps.Framework.DataAccess;
using MariApps.MS.Training.MSA.EmployeeMS.Repository.Contracts.DbContext;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariApps.MS.Training.MSA.EmployeeMS.Repository.DbContext
{
    public class AdoDbContext : BaseDBContext, IAdoDbContext
    {
        public AdoDbContext(string connectionString,
            ILogger logger,
            IAuditLogsOrchestrator auditLog,
            IRequestCorrelationIdAccessor correlationIdAccessor)
            : base(connectionString, logger, auditLog, correlationIdAccessor)
        {

        }
    }
}
