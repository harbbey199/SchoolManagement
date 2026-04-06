using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    /// Record a payment for a student
    /// </summary>
    [HttpPost("record")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<PaymentResponse>>> RecordPayment(
        [FromBody] RecordPaymentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<PaymentResponse>.ErrorResponse("Invalid input"));

        var result = await _paymentService.RecordPaymentAsync(request);
        if (result == null)
            return BadRequest(ApiResponse<PaymentResponse>.ErrorResponse("Failed to record payment"));

        return Ok(ApiResponse<PaymentResponse>.SuccessResponse(result, "Payment recorded successfully"));
    }

    /// <summary>
    /// Get payment history for a student
    /// </summary>
    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<ApiResponse<PaymentHistoryResponse>>> GetPaymentHistory(int studentId)
    {
        var history = await _paymentService.GetPaymentHistoryAsync(studentId);
        if (history == null)
            return NotFound(ApiResponse<PaymentHistoryResponse>.ErrorResponse("Student not found"));

        return Ok(ApiResponse<PaymentHistoryResponse>.SuccessResponse(history));
    }

    /// <summary>
    /// Update payment status
    /// </summary>
    [HttpPut("{paymentId}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<object>>> UpdatePaymentStatus(
        int paymentId, 
        [FromQuery] int status)
    {
        var result = await _paymentService.UpdatePaymentStatusAsync(paymentId, status);
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Payment not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null, "Payment status updated successfully"));
    }
}
