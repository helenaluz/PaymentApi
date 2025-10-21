using Hangfire;
using Microsoft.AspNetCore.Mvc;
using PaymentApi.Data;
using PaymentApi.Models;
using PaymentApi.Services;


namespace PaymentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentRepository _repo;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly PaymentJobService _jobService;
        private readonly PaymentMetrics _metrics;

        public PaymentsController(PaymentRepository repo, IBackgroundJobClient backgroundJobs, PaymentJobService jobService, PaymentMetrics metrics )
        {
            _repo = repo;
            _backgroundJobs = backgroundJobs;
            _jobService = jobService;
            _metrics = metrics;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _repo.GetAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var payment = await _repo.GetByIdAsync(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Payment payment)
        {
            payment.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            await _repo.CreateAsync(payment);
            _metrics.PaymentCreated();

            _backgroundJobs.Enqueue(() => _jobService.ProcessarPagamento(payment.Id));

            return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment);

        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] PaymentStatus status)
        {
            await _repo.UpdateStatusAsync(id, status);
            return NoContent();
        }
    }
}
