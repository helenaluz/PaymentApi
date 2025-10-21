using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentApi.Models
{
    public enum PaymentMethods
    {
        Pix,
        Cartao,
        Boleto
    }
}
