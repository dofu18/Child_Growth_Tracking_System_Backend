﻿using ApplicationLayer.DTOs.Children;
using ApplicationLayer.DTOs.Childrens;
using ApplicationLayer.Service;
using InfrastructureLayer.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControllerLayer.Controllers
{
    [Route("api/v1/children")]
    //[Authorize]
    public class ChildrenController : ControllerBase
    {
        private readonly IChildrenService _childrenService;
        private readonly ILogger<ChildrenController> _logger;

        public ChildrenController(ILogger<ChildrenController> logger, IChildrenService childrenService)
        {
            _logger = logger;
            _childrenService = childrenService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ChildrenCreateDto dto)
        {
            _logger.LogInformation("Create Children request received");
            return await _childrenService.Create(dto);
        }

        [HttpGet("{parentId}")]
        public async Task<IActionResult> GetChildByParent([FromRoute] Guid parentId)
        {
            _logger.LogInformation("Get all children by parent request received");

            return await _childrenService.GetChildByParent(parentId);
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ChildrenUpdateDto dto)
        {
            return await _childrenService.Update(id, dto);
        }
    }
}
