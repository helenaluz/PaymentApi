using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentApi.Models
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public decimal Valor { get; set; }

        [BsonRepresentation(BsonType.String)]
        public PaymentMethods Metodo { get; set; }

        [BsonRepresentation(BsonType.String)]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pendente;


        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}
