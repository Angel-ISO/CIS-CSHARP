using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CisAPI.Dtos.Votes;
using CisAPI.Helpers;
using CisAPI.Middlewares;
using CisAPI.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CisAPI.Controllers;

[Authorize]
public class VoteController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserContextService _userContextService;

    public VoteController(IUnitOfWork unitOfWork, IMapper mapper, UserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

      [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<VoteDto>>> Get([FromQuery] Params @params)
    {
        var paginatedTopics = await _unitOfWork.Votes.GetAllAsync(@params.PageIndex, @params.PageSize, @params.Search);
        var VotesList = _mapper.Map<List<VoteDto>>(paginatedTopics.registros);
        return new Pager<VoteDto>(VotesList, paginatedTopics.totalRegistros, @params.PageIndex, @params.PageSize, @params.Search);
    }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
        var vote = await _unitOfWork.Votes.GetByIdAsync(id);
        if (vote == null)
        {
            return NotFound("Vote not found.");
        }

        return Ok(_mapper.Map<VoteDto>(vote));
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateVoteDto createVoteDto)
        {
            var userId = _userContextService.GetUserId(); 
            if (string.IsNullOrEmpty(userId)) 
                return BadRequest("User ID is required.");
                
             if (string.IsNullOrEmpty(createVoteDto.IdeaId))
                 return BadRequest("Idea ID is required.");

            if (!Guid.TryParse(userId, out var userGuid))
            return BadRequest("Invalid User ID format.");

            var existingVote = await _unitOfWork.Votes.GetByUserAndIdeaAsync(userGuid, createVoteDto.IdeaId );
            if (existingVote != null)
            {
                return Conflict("User already voted on this idea.");
            }

            var vote = new Vote
            {
                UserId = userGuid,  
                IdeaId = createVoteDto.IdeaId,
                Value = createVoteDto.Value,
                VotedAt = DateTime.UtcNow
            };

            _unitOfWork.Votes.Add(vote);
            return CreatedAtAction(nameof(Post), new { vote.IdeaId, vote.UserId }, "Vote successfully registered");
        }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AuthorizeOwner("vote")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(string id, [FromBody] UpdateVoteDto updateDto)
    {
        var vote = await _unitOfWork.Votes.GetByIdAsync(id);
        if (vote == null)
            return NotFound("Vote not found.");        

        vote.Value = updateDto.Value;
        vote.VotedAt = DateTime.UtcNow;

        _unitOfWork.Votes.Update(vote);
        return Ok("Vote updated.");
    }

    
    [HttpDelete("{id}")]
    [AuthorizeOwner("vote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        var vote = await _unitOfWork.Votes.GetByIdAsync(id);
        if (vote == null)
            return NotFound("Vote not found.");

        _unitOfWork.Votes.Remove(vote);
        return Ok("Vote deleted.");
    }
}