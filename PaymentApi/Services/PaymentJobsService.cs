using MassTransit;
using PaymentApi.Data;
using PaymentApi.Events;
using PaymentApi.Models;

namespace PaymentApi.Services
{
    public class PaymentJobService
    {
        private readonly PaymentRepository _repo;
        private readonly ILogger<PaymentJobService> _logger;
        private readonly PaymentMetrics _metrics;
        private readonly IPublishEndpoint _publishEndpoint;
        public PaymentJobService(PaymentRepository repo, ILogger<PaymentJobService> logger, PaymentMetrics metrics, IPublishEndpoint publishEndpoint)
        {
            _repo = repo;
            _logger = logger;
            _metrics = metrics;
            _publishEndpoint = publishEndpoint;
        }

        public async Task ProcessarPagamento(string id)
        {
            _logger.LogInformation("Processando pagamento {PaymentId}", id);

            await Task.Delay(5000); 

            await _repo.UpdateStatusAsync(id, PaymentStatus.Processado);

            await PublishRabitMqEvent(id);

            _metrics.PaymentProcessed();

            _logger.LogInformation("Pagamento {PaymentId} atualizado para {Status}", id, PaymentStatus.Processado);
        }

        private async Task PublishRabitMqEvent(string id)
        {
            var payment = await _repo.GetByIdAsync(id);

            if (payment == null)
            {
                _logger.LogError("Pagamento {PaymentId} não encontrado após atualização de status", id);
                return;
            }

            var evt = new PaymentProcessedEvent
            {
                PaymentId = id,
                Valor = payment.Valor,
                Metodo = payment.Metodo.ToString(),
                Status = PaymentStatus.Processado.ToString(),
                ProcessadoEm = DateTime.UtcNow
            };

            await _publishEndpoint.Publish(evt);

            _logger.LogInformation("Evento PaymentProcessed publicado no RabbitMQ para {Id}", id);
        }
    }
}
