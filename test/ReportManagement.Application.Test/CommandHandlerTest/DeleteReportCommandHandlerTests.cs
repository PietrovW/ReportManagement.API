using AutoMapper;
using MediatR;
using Moq;
using NUnit.Framework;
using ReportManagement.Application.CommandHandler.V1;
using ReportManagement.Application.Common.V1;
using ReportManagement.Application.Events;
using ReportManagement.Domain.Models;
using ReportManagement.Domain.Repositorys;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReportManagement.Application.Test.CommandHandlerTest
{
    internal class DeleteReportCommandHandlerTests
    {
        [Test]
        public async Task DeleteReportCommand_CustomerDataDeleteOnDatabase()
        {
            //Arange
            var _mediatorMock = new Mock<IMediator>();
            var _readReportRepository = new Mock<IReadReportRepository>();
            var listReportModel = new List<ReportModel?>();
            var idtest = Guid.NewGuid();
            _readReportRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new ReportModel() { Id = idtest, Name = "test" });
            var _writeReportRepositoryMock = new Mock<IWriteReportRepository>();

            var _command = new DeleteReportCommand() { Id= idtest };
          
            var handler = new DeleteReportCommandHandler(_writeReportRepositoryMock.Object, _readReportRepository.Object, _mediatorMock.Object);

            //Act
             await handler.Handle(_command, new System.Threading.CancellationToken());

            //Asert
            _mediatorMock.Verify(x => x.Publish<DeleteReportEvents>(It.IsAny<DeleteReportEvents>(), default(CancellationToken)), Times.Once);
        }
    }
}
