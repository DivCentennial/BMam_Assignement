using MariApps.MS.Training.MSA.EmployeeMS.Business;
using MariApps.MS.Training.MSA.EmployeeMS.Repository.Contracts.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariApps.MS.Training.MSA.EmployeeMS.Test.UnitTest.BusinessTest
{
    [TestClass]
    public class SampleServiceTest
    {
        [TestMethod]
        public void GetDataTest()
        {
            Mock<ISampleRepository> mockSampleRepository = new Mock<ISampleRepository>();
            mockSampleRepository.Setup(x => x.GetData()).Returns("Sample mock data");

            var services = new ServiceCollection();
            services.AddScoped<ISampleService, SampleService>(provider => new SampleService(mockSampleRepository.Object));

            var provider = services.BuildServiceProvider();

            var sampleService = provider.GetRequiredService<ISampleService>();
            var result = sampleService.GetData();

            Assert.AreEqual("Sample mock data", result);
        }
    }
}
