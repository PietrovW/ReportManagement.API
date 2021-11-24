using AutoMapper;
using Moq;
using NUnit.Framework;
using ReportManagement.Application.AutoMapper;
using ReportManagement.Application.Dtos;
using ReportManagement.Application.QuerieHandler;
using ReportManagement.Application.Queries;
using ReportManagement.Domain.Models;
using ReportManagement.Domain.Repositorys;
using System;
using System.Threading.Tasks;

namespace ReportManagement.Application.Test.QuerieHandlerTest
{
    [TestFixture]
    internal class GetReportQueryHandlerTest
    {
        [Test]
        public async Task CreateReportCommand_CustomerDataGetReport()
        {
            //Arange
            var reportRepositoryMoq = new Mock<IReportRepository>();
            Guid testId = Guid.NewGuid();
            ReportModel reportModel = new ReportModel() { Id = testId, Name = "test" };
            reportRepositoryMoq.Setup(f => f.GetById(It.IsAny<Guid>()))
                .Returns(reportModel);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<Profiles>());
            IMapper mapper = config.CreateMapper();
            var query = new GetReportQuery() { Id = testId };
            var handler = new GetReportQueryHandler(mapper, reportRepositoryMoq.Object);

            //Act
            ReportDto resut = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asert
            reportRepositoryMoq.Verify(x => x.GetById(It.IsAny<Guid>()));
        }

    }
}
