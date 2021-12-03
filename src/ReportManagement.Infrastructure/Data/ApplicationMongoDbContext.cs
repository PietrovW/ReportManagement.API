using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ReportManagement.Data.Configurations;
using ReportManagement.Infrastructure.Data.Settings;

namespace ReportManagement.Infrastructure.Data
{
    public class ApplicationMongoDbContext : IApplicationMongoDbContext
    {
        private  IMongoDatabase _database { get;  set; }
        private MongoOptions mongoSettings { get; set; }=new MongoOptions();

        private readonly List<Func<Task>> _commands = new List<Func<Task>>();
        
        public ApplicationMongoDbContext(IOptions<MongoOptions> settings):base()
        {
            mongoSettings = settings.Value;
            var MongoClient = new MongoClient(mongoSettings.ConnectionString);
            _database = MongoClient.GetDatabase(mongoSettings.DatabaseName);
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : class
        {
            return _database.GetCollection<TEntity>(typeof(TEntity).Name);
        }
        public void Add(Func<Task> func)
        {
            _commands.Add(func);
        }
        public async Task<int> SaveChanges()
        {
            var commandTasks = _commands.Select(c => c());

            await Task.WhenAll(commandTasks);
            return _commands.Count;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
