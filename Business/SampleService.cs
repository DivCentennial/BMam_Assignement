using MariApps.MS.Training.MSA.EmployeeMS.Repository.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariApps.MS.Training.MSA.EmployeeMS.Business
{
    public interface ISampleService
    {
        string GetData();
    }

    public class SampleService : ISampleService
    {
        private readonly ISampleRepository _sampleRepository;

        public SampleService(ISampleRepository sampleRepository)
        {
            _sampleRepository = sampleRepository;
        }

        public string GetData()
        {
            var result = _sampleRepository.GetData();

            return result;
        }
    }
}
