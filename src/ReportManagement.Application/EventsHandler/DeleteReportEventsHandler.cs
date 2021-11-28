using MediatR;
using MongoDB.Driver;
using ReportManagement.Application.Events;
using ReportManagement.Data.Configurations;
using ReportManagement.Domain.Models;

namespace ReportManagement.Application.EventsHandler
{
    public class DeleteReportEventsHandler : INotificationHandler<DeleteReportEvents>
    {
        private readonly IApplicationMongoDbContext _applicationMongoDbContext;
        public DeleteReportEventsHandler(IApplicationMongoDbContext applicationMongoDbContext)
        {
            _applicationMongoDbContext = applicationMongoDbContext;
        }

        public async Task Handle(DeleteReportEvents notification, CancellationToken cancellationToken)
        {
            IMongoCollection<ReportModel> collation = _applicationMongoDbContext.GetCollection<ReportModel>();

            _applicationMongoDbContext.Add(() => collation.DeleteOneAsync(x=>x.Id== notification.Id, cancellationToken));
            await _applicationMongoDbContext.SaveChanges();
        }
    }
}
