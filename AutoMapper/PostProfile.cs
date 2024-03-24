using AutoMapper;
using Pinterest.DTOs.Post;
using Pinterest.Entities;

namespace Pinterest.AutoMapper
{
	public class PostProfile : Profile
	{
		public PostProfile()
		{
			CreateMap<Post, GetPostDto>();
			CreateMap<AddPostDto, Post>();
			CreateMap<Post, GetPostDetailsDto>()
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
				.ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes));
		}
	}
}
