using CisAPI.Dtos.Topics;
using Domain.Entities;
using AutoMapper;
using CisAPI.Dtos.Ideas;
using CisAPI.Dtos.Votes;


namespace CisAPI.Profiles;
public class MappingProfiles : Profile
{
    public MappingProfiles()
     { 
        CreateMap<Topic, TopicDto>()
                .ForMember(dest => dest.Ideas, opt => opt.MapFrom(src => src.Ideas))  
                .ReverseMap(); 

        CreateMap<CreateTopicDto, Topic>();

        CreateMap<UpdateTopicDto, Topic>();


        CreateMap<Idea, IdeaDto>()
            .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Votes.Count(v => v.Value == 1)))  
            .ForMember(dest => dest.Dislikes, opt => opt.MapFrom(src => src.Votes.Count(v => v.Value == -1)))  
            .ReverseMap();

        CreateMap<CreateIdeaDto, Idea>();





          
        CreateMap<Vote, VoteDto>()
            .ForMember(dest => dest.IdeaTitle, opt => opt.MapFrom(src => src.Idea != null ? src.Idea.Title : null)) 
            .ForMember(dest => dest.VotedAt, opt => opt.MapFrom(src => src.VotedAt));

        CreateMap<CreateVoteDto, Vote>()
            .ForMember(dest => dest.VotedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}