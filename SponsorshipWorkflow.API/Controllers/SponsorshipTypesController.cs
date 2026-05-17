using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SponsorshipWorkflow.Application.DTOs;
using SponsorshipWorkflow.Application.Interfaces;
using SponsorshipWorkflow.Domain.Entities;

namespace SponsorshipWorkflow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SponsorshipTypesController : ControllerBase
{
    private readonly ISponsorshipTypeService _sponsorshipTypeService;

    public SponsorshipTypesController(ISponsorshipTypeService sponsorshipTypeService)
    {
        _sponsorshipTypeService = sponsorshipTypeService;
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<List<SponsorshipType>>>> GetAll()
    {
        var types = await _sponsorshipTypeService.GetAllAsync();
        return Ok(BaseResponse<List<SponsorshipType>>.SuccessResult(types));
    }

    [Authorize(Roles = "SystemAdmin")]
    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<SponsorshipType>>> GetById(Guid id)
    {
        var type = await _sponsorshipTypeService.GetByIdAsync(id);
        if (type == null)
            return NotFound(BaseResponse<SponsorshipType>.ErrorResult("Sponsorship type not found"));
        return Ok(BaseResponse<SponsorshipType>.SuccessResult(type));
    }

    [Authorize(Roles = "SystemAdmin")]
    [HttpPost]
    public async Task<ActionResult<BaseResponse<SponsorshipType>>> Create([FromBody] CreateSponsorshipTypeDto dto)
    {
        var type = await _sponsorshipTypeService.CreateAsync(dto.Name);
        return Ok(BaseResponse<SponsorshipType>.SuccessResult(type, "Sponsorship type created successfully"));
    }

    [Authorize(Roles = "SystemAdmin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<BaseResponse<SponsorshipType>>> Update(Guid id, [FromBody] UpdateSponsorshipTypeDto dto)
    {
        var type = await _sponsorshipTypeService.UpdateAsync(id, dto.Name);
        return Ok(BaseResponse<SponsorshipType>.SuccessResult(type, "Sponsorship type updated successfully"));
    }

    [Authorize(Roles = "SystemAdmin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse>> Delete(Guid id)
    {
        await _sponsorshipTypeService.DeleteAsync(id);
        return Ok(BaseResponse.SuccessResult("Sponsorship type deleted successfully"));
    }
}