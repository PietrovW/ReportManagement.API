using MediatR;
using MongoDB.Driver;
using ReportManagement.Application.Events;
using ReportManagement.Data.Configurations;
using ReportManagement.Domain.Models;

namespace ReportManagement.Application.EventsHandler
{
    public class CreateReportEventsHandler : INotificationHandler<CreateReportEvents>
    {
        private readonly IApplicationMongoDbContext _applicationMongoDbContext;
        public CreateReportEventsHandler(IApplicationMongoDbContext applicationMongoDbContext)
        {
            _applicationMongoDbContext=applicationMongoDbContext;
        }

        public async Task Handle(CreateReportEvents notification, CancellationToken cancellationToken)
        {
            IMongoCollection<ReportModel> collation =_applicationMongoDbContext.GetCollection<ReportModel>();

            _applicationMongoDbContext.Add(() => collation.InsertOneAsync(new ReportModel() { Id= notification.Id , Name = notification.Name}));

            await _applicationMongoDbContext.SaveChanges();
        }
    }
}
