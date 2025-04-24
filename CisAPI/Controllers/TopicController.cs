using AutoMapper;
using CisAPI.Dtos.Topics;
using CisAPI.Helpers;
using CisAPI.middlewares;
using CisAPI.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CisAPI.Controllers;
 

[Authorize] 

public class TopicController : BaseApiController
{
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserContextService _userContextService;  


        public TopicController(IUnitOfWork unitOfWork, IMapper mapper, UserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<TopicDto>>> Get([FromQuery] Params @params)
    {
        var paginatedTopics = await _unitOfWork.Topics.GetAllAsync(@params.PageIndex, @params.PageSize, @params.Search);
        var topicsList = _mapper.Map<List<TopicDto>>(paginatedTopics.registros);
        return new Pager<TopicDto>(topicsList, paginatedTopics.totalRegistros, @params.PageIndex, @params.PageSize, @params.Search);
    }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(string id)
        {
            var topic = await _unitOfWork.Topics.GetByIdAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<TopicDto>(topic));
        }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Post(CreateTopicDto createTopicDto)
    {
        var userId = _userContextService.GetUserId();
        if (string.IsNullOrEmpty(userId))
            return BadRequest("User ID is required.");

        var username = _userContextService.GetUsername();
        var topic = _mapper.Map<Topic>(createTopicDto);

        topic.UserId = Guid.Parse(userId);
        topic.Username = username;

        _unitOfWork.Topics.Add(topic);
        return CreatedAtAction(nameof(Get), new { id = topic.Id }, new { message = "Topic creado exitosamente" });
    }



    [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AuthorizeOwner("topic")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TopicDto>> Put(string id, [FromBody] UpdateTopicDto updateDto)
        {
            var topic = await _unitOfWork.Topics.GetByIdAsync(id);
                if (topic == null)
                {
                    return NotFound();
                }

            if (updateDto.Title is not null)
                topic.Title = updateDto.Title;
                
            if (updateDto.Description is not null)
                topic.Description = updateDto.Description;

            _unitOfWork.Topics.Update(topic);
            return Ok(_mapper.Map<TopicDto>(topic));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AuthorizeOwner("topic")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var topic = await _unitOfWork.Topics.GetByIdAsync(id);
                if (topic == null)
                {
                    return NotFound();
                }

                _unitOfWork.Topics.Remove(topic);
                return NoContent();
        }
}

