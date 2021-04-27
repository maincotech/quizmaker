using AutoMapper;
using Maincotech.Adapter;
using Maincotech.ExamAssistant.Dtos;
using Maincotech.ExamAssistant.Services.Models;

namespace Maincotech.ExamAssistant.Services.MapperProfiles
{
    internal class DtoToEntityMapperProfile : Profile, IOrderedMapperProfile
    {
        public int Order => 2;

        public DtoToEntityMapperProfile()
        {
            CreateMap<FirebaseSettingDto, FirebaseSetting>();
            //CreateMap<ArticleDto, Article>()
            //    .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(
            //        (src, dest, destMember, context) => src.Author ?? AppRuntimeContext.Current.Principal?.Identity?.Name))
            //     .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(
            //        (src, dest, destMember, context) => AppRuntimeContext.Current.Principal?.Identity?.Name))
            //     .ForMember(dest => dest.LastModifiedTime, opt => opt.MapFrom(
            //       (src, dest, destMember, context) => System.DateTime.UtcNow));

            //CreateMap<CommentDto, Comment>()
            //    .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(
            //        (src, dest, destMember, context) => src.Author ?? AppRuntimeContext.Current.Principal?.Identity?.Name))
            //     .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(
            //        (src, dest, destMember, context) => AppRuntimeContext.Current.Principal?.Identity?.Name))
            //     .ForMember(dest => dest.LastModifiedTime, opt => opt.MapFrom(
            //       (src, dest, destMember, context) => System.DateTime.UtcNow));
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