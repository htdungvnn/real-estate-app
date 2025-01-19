using ActionTrackingService.Models;
using MongoDB.Driver;

namespace ActionTrackingService.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<ActionLog> _actionLogs;

        public MongoDbService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDb:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDb:DatabaseName"]);
            _actionLogs = database.GetCollection<ActionLog>("ActionLogs");
        }

        public async Task AddActionLog(ActionLog actionLog)
        {
            await _actionLogs.InsertOneAsync(actionLog);
        }

        public async Task<List<ActionLog>> GetActionLogs() =>
            await _actionLogs.Find(_ => true).ToListAsync();
    }
}