using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PaymentApi.Models;

namespace PaymentApi.Data
{
    public class PaymentRepository
    {
        private readonly IMongoCollection<Payment> _payments;

        public PaymentRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.Database);
            _payments = database.GetCollection<Payment>("Payments");
        }

        public async Task<List<Payment>> GetAsync() =>
            await _payments.Find(_ => true).ToListAsync();

        public async Task<Payment?> GetByIdAsync(string id) =>
            await _payments.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Payment payment) =>
            await _payments.InsertOneAsync(payment);

        public async Task UpdateStatusAsync(string id, PaymentStatus status) =>
            await _payments.UpdateOneAsync(
                p => p.Id == id,
                Builders<Payment>.Update.Set(p => p.Status, status));
    }

    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
