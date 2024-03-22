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
		}
	}
}
