﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Application.DTOs.Members;
using OngProject.Application.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace OngProject.Controllers
{
    [ApiController]
    [Route("api/members")]
    public class MembersController : ControllerBase
    {
        private readonly MemberService _service;

        public MembersController(MemberService service)
        {
            _service = service;
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        #region Documentation
        [SwaggerOperation(Summary = "List of all Members", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success. Returns a list of existing Members", typeof(GetMembersDto))]
        [SwaggerResponse(400, "BadRequest. Something went wrong, try again")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user")]
        #endregion
        public async Task<ActionResult<List<GetMembersDto>>> GetAll()
        {
            return await _service.GetMembers();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetMembersDto>> GetById(int id)
        {
            return await _service.GetMemberDetails(id);
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        #region Documentation
        [SwaggerOperation(Summary = "Create Member.",Description = "Requires user privileges.")]
        [SwaggerResponse(200, "Created. Returns id of the created member.", typeof(int))]
        [SwaggerResponse(400, "BadRequest. Object not created, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        #endregion
        public async Task<ActionResult<int>> Create(CreateMemberDto memberDto)
        {
            return await _service.CreateMember(memberDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CreateMemberDto memberDto)
        {
            await _service.UpdateMember(id, memberDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.SoftDeleteMember(id);

            return NoContent();
        }
    }
}