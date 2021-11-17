﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using OngProject.Application.DTOs.Categories;
using OngProject.Application.Services;
using Swashbuckle.AspNetCore.Annotations;
using OngProject.Application.Helpers.Wrappers;

namespace OngProject.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [SwaggerTag("Create, read, update and delete Categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _service;

        public CategoriesController(CategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        #region Documentation
        [SwaggerOperation(Summary = "List of all Categories", Description = "Require admin privileges")]
        [SwaggerResponse(200, "Success. Returns a list of existing Categories", typeof(GetCategoriesDto))]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user or wrong jwt token")]
        [SwaggerResponse(404, "NotFound. Entity Id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        public async Task<ActionResult<Pagination<GetCategoriesDto>>> GetAll([FromQuery] CategoryQueryDto queryDto)
        {
            return await _service.GetAll(queryDto);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        #region Documentation
        [SwaggerOperation(Summary = "Get Category details by Id", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success. Returns Category details", typeof(GetCategoryDetailsDto))]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        public async Task<ActionResult> GetById([SwaggerParameter("Id")] int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        #region Documentation
        [SwaggerOperation(Summary = "Creates a new Category", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success.")]
        [SwaggerResponse(400, "BadRequest. Object not created, try again")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        public async Task<ActionResult> Post([FromForm] CreateCategoryDto model)
        {
            return Ok(await _service.CreateCategory(model));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        #region Documentation
        [SwaggerOperation(Summary = "Modifies an existing Category", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success.")]
        [SwaggerResponse(400, "BadRequest. Object not modified, try again")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        public async Task<ActionResult> Put([SwaggerParameter("Id")] int id, [FromForm] CreateCategoryDto model)
        {
            await _service.Update(id, model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        #region Documentation
        [SwaggerOperation(Summary = "Soft delete an existing Category", Description = "Requires admin privileges")]
        [SwaggerResponse(204, "Deleted.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        public async Task<ActionResult> Delete([SwaggerParameter("Id")] int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}