using AutoMapper;
using FirebaseAdmin.Auth;
using Maincotech.Adapter;
using Maincotech.ExamAssistant.Dtos;
using Maincotech.ExamAssistant.Services.Models;

namespace Maincotech.ExamAssistant.Services.MapperProfiles
{
    public class EntityToDtoMapperProfile : Profile, IOrderedMapperProfile
    {
        public int Order => 2;

        public EntityToDtoMapperProfile()
        {
            CreateMap<Exam, ExamDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom((src, dest, destMember, context) => src.Reference.Id))
                .ForMember(dest => dest.UpdateOn, opt => opt.MapFrom((src, dest, destMember, context) => src.UpdateOn.ToDateTime()));

            CreateMap<Section, SectionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom((src, dest, destMember, context) => src.Reference.Id));

            CreateMap<QuestionOption, QuestionOptionDto>();
            CreateMap<Question, QuestionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom((src, dest, destMember, context) => src.Reference.Id));

            CreateMap<ExportedUserRecord, AppUserDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.LastSignInTimestamp, opt => opt.MapFrom((src, dest, destMember, context) => src.UserMetaData.LastSignInTimestamp))
                .ForMember(dest => dest.CreationTimestamp, opt => opt.MapFrom((src, dest, destMember, context) => src.UserMetaData.CreationTimestamp));

            //CreateMap<Category, CategoryDto>();

            //CreateMap<Article, ArticleDto>()
            //    .ForMember(dest => dest.Author, opt => opt.MapFrom((src, dest, destMember, context) => src.CreatedBy));

            //CreateMap<Article, LocalizedArticleDto>()
            //    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom((src, dest, destMember, context) => src.Category.Name))
            //    .ForMember(dest => dest.Author, opt => opt.MapFrom((src, dest, destMember, context) => src.CreatedBy))
            //    .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom((src, dest, destMember, context) => src.Comments.IsNotNullOrEmpty() ? src.Comments.Count : 0));

            //CreateMap<Comment, CommentDto>()
            //    .ForMember(dest => dest.Author, opt => opt.MapFrom((src, dest, destMember, context) => src.CreatedBy));

            //CreateMap<ArticleSEO, ArticleSEODto>();

            //CreateMap<CategoryViewModel, Category>()
            //    .ForMember(x => x.Children, opt => opt.Ignore())
            //    .ForMember(x => x.ParentId, opt => opt.MapFrom(
            //        (src, dest, destMember, context) =>
            //        string.IsNullOrEmpty(src.ParentId) ?
            //        new Nullable<Guid>() :
            //        new Guid?(Guid.Parse(src.ParentId))));

            //CreateMap<BlogViewModel, Article>()
            //    .ForMember(x => x.LastModifiedTime, opt => opt.MapFrom((src, dest, destMember, context) => DateTime.UtcNow));
        }
    }
}