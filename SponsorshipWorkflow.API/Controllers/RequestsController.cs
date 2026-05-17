using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SponsorshipWorkflow.Application.DTOs;
using SponsorshipWorkflow.Application.Interfaces;
using SponsorshipWorkflow.Domain.Entities;

namespace SponsorshipWorkflow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RequestsController : ControllerBase
{
    private readonly IRequestService _requestService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IWorkflowService _workflowService;

    public RequestsController(IRequestService requestService, ICurrentUserService currentUserService, IWorkflowService workflowService)
    {
        _requestService = requestService;
        _currentUserService = currentUserService;
        _workflowService = workflowService;
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<SponsorshipRequest>>> Create([FromBody] CreateRequestDto dto)
    {
        var request = await _requestService.CreateAsync(dto, _currentUserService.UserId);
        return Ok(BaseResponse<SponsorshipRequest>.SuccessResult(request, "Request created successfully"));
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<List<SponsorshipRequest>>>> Get()
    {
        var requests = await _requestService.GetAllAsync(_currentUserService.UserId, _currentUserService.Role);
        return Ok(BaseResponse<List<SponsorshipRequest>>.SuccessResult(requests));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<SponsorshipRequest>>> GetById(Guid id)
    {
        var request = await _requestService.GetByIdAsync(id);
        if (request == null)
            return NotFound(BaseResponse<SponsorshipRequest>.ErrorResult("Request not found"));
        return Ok(BaseResponse<SponsorshipRequest>.SuccessResult(request));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Requestor")]
    public async Task<ActionResult<BaseResponse<SponsorshipRequest>>> Update(Guid id, [FromBody] UpdateRequestDto dto)
    {
        var request = await _requestService.UpdateAsync(id, dto, _currentUserService.UserId);
        return Ok(BaseResponse<SponsorshipRequest>.SuccessResult(request, "Request updated successfully"));
    }

    [HttpPost("{id}/submit")]
    public async Task<ActionResult<BaseResponse>> Submit(Guid id)
    {
        await _workflowService.SubmitAsync(id, _currentUserService.UserId, _currentUserService.Role);
        return Ok(BaseResponse.SuccessResult("Submitted successfully"));
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult<BaseResponse>> Cancel(Guid id)
    {
        await _workflowService.CancelAsync(id, _currentUserService.UserId, _currentUserService.Role);
        return Ok(BaseResponse.SuccessResult("Cancelled successfully"));
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("{id}/manager-approve")]
    public async Task<ActionResult<BaseResponse>> ManagerApprove(Guid id, [FromBody] ApprovalActionDto dto)
    {
        await _workflowService.ManagerApproveAsync(id, dto.Remarks);
        return Ok(BaseResponse.SuccessResult("Approved successfully"));
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("{id}/manager-reject")]
    public async Task<ActionResult<BaseResponse>> ManagerReject(Guid id, [FromBody] ApprovalActionDto dto)
    {
        await _workflowService.ManagerRejectAsync(id, dto.Remarks);
        return Ok(BaseResponse.SuccessResult("Rejected successfully"));
    }

    [Authorize(Roles = "FinanceAdmin")]
    [HttpPost("{id}/finance-approve")]
    public async Task<ActionResult<BaseResponse>> FinanceApprove(Guid id, [FromBody] ApprovalActionDto dto)
    {
        await _workflowService.FinanceApproveAsync(id, dto.Remarks);
        return Ok(BaseResponse.SuccessResult("Approved successfully"));
    }

    [Authorize(Roles = "FinanceAdmin")]
    [HttpPost("{id}/finance-reject")]
    public async Task<ActionResult<BaseResponse>> FinanceReject(Guid id, [FromBody] ApprovalActionDto dto)
    {
        await _workflowService.FinanceRejectAsync(id, dto.Remarks);
        return Ok(BaseResponse.SuccessResult("Rejected successfully"));
    }

    [HttpGet("{id}/history")]
    public async Task<ActionResult<BaseResponse<List<ApprovalHistory>>>> History(Guid id)
    {
        var history = await _requestService.GetHistoryAsync(id);
        return Ok(BaseResponse<List<ApprovalHistory>>.SuccessResult(history));
    }

    [Authorize(Roles = "SystemAdmin")]
    [HttpGet("all-history")]
    public async Task<ActionResult<BaseResponse<List<ApprovalHistory>>>> GetAllHistory()
    {
        var history = await _requestService.GetAllHistoryAsync();
        return Ok(BaseResponse<List<ApprovalHistory>>.SuccessResult(history));
    }
}
