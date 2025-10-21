namespace PaymentApi.Events
{
    public class PaymentProcessedEvent
    {
        public string PaymentId { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Metodo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime ProcessadoEm { get; set; } = DateTime.UtcNow;
    }
}
