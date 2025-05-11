using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CisAPI.Dtos.Ideas;
using CisAPI.Helpers;
using CisAPI.Middlewares;
using CisAPI.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CisAPI.Controllers
{
    [Authorize]
    public class IdeaController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserContextService _userContextService;

        public IdeaController(IUnitOfWork unitOfWork, IMapper mapper, UserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<IdeaDto>>> Get([FromQuery] Params @params)
        {
            var paginatedTopics = await _unitOfWork.Ideas.GetAllAsync(@params.PageIndex, @params.PageSize, @params.Search);
            var ideaList = _mapper.Map<List<IdeaDto>>(paginatedTopics.registros);
            return new Pager<IdeaDto>(ideaList, paginatedTopics.totalRegistros, @params.PageIndex, @params.PageSize, @params.Search);
        }

        [HttpGet("topic/{topicId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<IdeaDto>>> GetByTopic(string topicId)
        {
            var ideas = await _unitOfWork.Ideas.GetIdeasByTopicIdAsync(topicId);
            var ideasDto = _mapper.Map<List<IdeaDto>>(ideas);
            return Ok(ideasDto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IdeaDto>> GetById(string id)
        {
            var idea = await _unitOfWork.Ideas.GetByIdAsync(id);
            if (idea == null)
                return NotFound();

            return Ok(_mapper.Map<IdeaDto>(idea));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(CreateIdeaDto createDto)
        {
            var userId = _userContextService.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID is required.");
            var username = _userContextService.GetUsername();

            var idea = _mapper.Map<Idea>(createDto);
            idea.UserId = Guid.Parse(userId);
            idea.Username = username;
            idea.CreatedAt = DateTime.UtcNow;

            _unitOfWork.Ideas.Add(idea);
            return CreatedAtAction(nameof(GetById), new { id = idea.Id }, new { message = "Idea created successfully" });
        }

        [HttpPut("{id}")]
        [AuthorizeOwner("idea")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IdeaDto>> Put(string id, UpdateIdeaDto updateDto)
        {
            var idea = await _unitOfWork.Ideas.GetByIdAsync(id);
            if (idea == null)
                return NotFound();

            if (updateDto.Title is not null)
                idea.Title = updateDto.Title;

            if (updateDto.Content is not null)
                idea.Content = updateDto.Content;

            _unitOfWork.Ideas.Update(idea);

            return Ok(_mapper.Map<IdeaDto>(idea));
        }

        [HttpDelete("{id}")]
        [AuthorizeOwner("idea")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var idea = await _unitOfWork.Ideas.GetByIdAsync(id);
            if (idea == null)
                return NotFound();

            _unitOfWork.Ideas.Remove(idea);

            return NoContent();
        }
    }

}