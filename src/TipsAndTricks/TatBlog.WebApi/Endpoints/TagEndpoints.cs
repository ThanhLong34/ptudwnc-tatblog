using Mapster;
using MapsterMapper;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class TagEndpoints
    {
        public static WebApplication MapTagEndpoints(this WebApplication app)
        {

            var routeGroupBuilder = app.MapGroup("api/tags");

            routeGroupBuilder.MapGet("/", GetTags)
              .WithName("GetTags")
              .Produces<ApiResponse<IPagedList<TagItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetTagDetail)
              .WithName("GetTagDetail")
              .Produces<ApiResponse<TagItem>>();

            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}", GetTagBySlug)
              .WithName("GetTagBySlug")
              .Produces<ApiResponse<TagItem>>();

            routeGroupBuilder.MapPost("/", AddTag)
              .WithName("AddTag")
              .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
              .Produces<ApiResponse<TagDto>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateTag)
              .WithName("UpdateTag")
              .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
              .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteTag)
              .WithName("DeleteTag")
              .Produces<ApiResponse<string>>();

            return app;
        }

        //private static async Task<IResult> GetTags([AsParameters] TagFilterModel model, ITagRepository tagRepository)
        //{
        //    var tags = await tagRepository.GetPagedTagsAsync(model, model.Keyword);

        //    var paginationResult = new PaginationResult<TagItem>(tags);
        //    return Results.Ok(ApiResponse.Success(paginationResult));
        //}

        private static async Task<IResult> GetTags(
          [AsParameters] TagFilterModel model,
          IMapper mapper,
          ITagRepository tagRepository)
        {

            var tagQuery = mapper.Map<TagQuery>(model);


            var tags = await tagRepository.GetPagedTagsAsync(
              tagQuery, model,
              tags => tags.ProjectToType<TagItem>());

            var paginationResult = new PaginationResult<TagItem>(tags);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetTagDetail(
          int id,
          ITagRepository tagRepository,
          IMapper mapper)
        {

            var tag = await tagRepository.GetTagByIdAsync(id);

            return tag == null
              ? Results.Ok(ApiResponse.Fail(
                  HttpStatusCode.NotFound,
                  $"Không tìm thấy id = {id}"))
              : Results.Ok(ApiResponse.Success(
                  mapper.Map<TagItem>(tag)));
        }


        private static async Task<IResult> GetTagBySlug(
          string slug,
          ITagRepository tagRepository,
          IMapper mapper)
        {

            var tag = await tagRepository.GetTagBySlugAsync(slug);

            return tag == null
              ? Results.Ok(ApiResponse.Fail(
                  HttpStatusCode.NotFound,
                  $"Không tìm thấy thẻ có slug = {slug}"))
              : Results.Ok(ApiResponse.Success(
                  mapper.Map<TagItem>(tag)));
        }


        private static async Task<IResult> AddTag(
          TagEditModel model,
          ITagRepository tagRepository,
          IMapper mapper)
        {

            if (await tagRepository.IsTagSlugExistAsync(model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                  HttpStatusCode.Conflict,
                  $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var tag = mapper.Map<Tag>(model);

            await tagRepository.CreateOrUpdateTagAsync(tag);

            return Results.Ok(
              ApiResponse.Success(
                 mapper.Map<TagDto>(tag),
                 HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateTag(
          int id,
          TagEditModel model,
          ITagRepository tagRepository,
          IMapper mapper)
        {

            var tag = await tagRepository.GetTagByIdAsync(id);

            if (tag == null)
            {
                return Results.Ok(ApiResponse.Fail(
                        HttpStatusCode.NotFound,
                        $"Không tìm thấy thẻ có id = {id}"));
            }

            if (await tagRepository.IsTagSlugExistAsync(model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                  HttpStatusCode.Conflict,
                  $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            mapper.Map(model, tag);
            tag.Id = id;

            return await tagRepository.CreateOrUpdateTagAsync(tag)
                ? Results.Ok($"Thay đổi thẻ có id = {id} thành công")
                : Results.Ok(
                  ApiResponse.Fail(
                    HttpStatusCode.NotFound,
                    $"Không tìm thấy thẻ có id = {id}"));
        }

        private static async Task<IResult> DeleteTag(
          int id,
          ITagRepository tagRepository)
        {
            return await tagRepository.DeleteTagAsync(id)
              ? Results.Ok(ApiResponse.Success(
                  $"Xóa thành công thẻ id = {id}"))
              : Results.Ok(ApiResponse.Fail(
                  HttpStatusCode.NotFound,
                  $"Không tìm thấy thẻ có id = {id}"));

        }
    }
}
