using System.Diagnostics.Metrics;

namespace PaymentApi.Services
{
    public class PaymentMetrics
    {
        private readonly Meter _meter;
        private readonly Counter<int> _paymentsCreated;
        private readonly Counter<int> _paymentsProcessed;

        public PaymentMetrics()
        {
            _meter = new Meter("PaymentApi.Metrics", "1.0.0");
            _paymentsCreated = _meter.CreateCounter<int>("payments_created_total");
            _paymentsProcessed = _meter.CreateCounter<int>("payments_processed_total");
        }

        public void PaymentCreated() => _paymentsCreated.Add(1);
        public void PaymentProcessed() => _paymentsProcessed.Add(1);
    }
}
