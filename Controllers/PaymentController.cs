using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAppBackend.Frontend.Models;
using MovieAppBackend.IServices;

namespace MovieAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private IPaymentService _paymentService;
        public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService) 
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost("addpurchaseinfo")]
        public async Task<IActionResult> AddPurchaseDetails(PurchaseInfos purchaseInfos) 
        {
            try
            {
                var result = await _paymentService.AddPurchaseInfos(purchaseInfos);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }
        }
    }
}
