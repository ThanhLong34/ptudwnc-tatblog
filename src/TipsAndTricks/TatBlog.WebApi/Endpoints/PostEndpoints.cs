﻿using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class PostEndpoints
    {
        public static WebApplication MapPostsEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/posts");
            routeGroupBuilder.MapGet("/", GetPosts)
               .WithName("GetPosts")
               .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapGet("/featured/{limit:int}", GetPopularArticle)
                .WithName("GetPopularArticle")
                .Produces<ApiResponse<IList<PostDto>>>();

            routeGroupBuilder.MapGet("/random/{limit:int}", GetRandomPosts)
                .WithName("GetRandomPosts")
                .Produces<ApiResponse<IList<PostDto>>>();

            routeGroupBuilder.MapGet("{id:int}", GetPostDetailById)
                .WithName("GetPostDetailById")
                .Produces<ApiResponse<PostDetail>>();

            routeGroupBuilder.MapGet("/byslug/{slug:regex(^[a-z0-9_-]+$)}", GetPostDetailBySlug)
                .WithName("GetPostDetailBySlug")
                .Produces<ApiResponse<PostDetail>>();

            routeGroupBuilder.MapPost("/", AddPost)
                  .WithName("AddPost")
                  .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                  .Produces(401)
                  .Produces<ApiResponse<PostDto>>();

            routeGroupBuilder.MapPost("{id:int}/picture", SetImagePost)
                .WithName("SetImagePost")
                .Accepts<IFormFile>("multipart/form-data")
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
                .WithName("UpdateAPost")
                .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
                .WithName("DeletePost")
                .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetPosts(
            [AsParameters] PostFilterModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            if (model.Published == null) { model.Published = true; }
            var postQuery = mapper.Map<PostQuery>(model);

            var posts = await blogRepository.GetPagedPostsByQueryAsync<PostDto>(
                posts => posts.ProjectToType<PostDto>(),
                postQuery,
                model);


            var paginationResult = new PaginationResult<PostDto>(posts);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetPopularArticle(int limit, IBlogRepository blogRepository, IMapper mapper)
        {
            var posts = await blogRepository.GetPopularArticlesAsync(limit);

            return Results.Ok(ApiResponse.Success(mapper.Map<IList<PostDto>>(posts)));
        }

        private static async Task<IResult> GetRandomPosts(int limit, IBlogRepository blogRepository, IMapper mapper)
        {
            var posts = await blogRepository.GetRandomPostsAsync(limit);

            return Results.Ok(ApiResponse.Success(mapper.Map<IList<PostDto>>(posts)));
        }

        private static async Task<IResult> GetPostDetailById(int id, IMapper mapper, IBlogRepository blogRepository)
        {
            var post = await blogRepository.GetPostByIdAsync(id, true);

            return post == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy id = {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(post)));
        }

        private static async Task<IResult> GetPostDetailBySlug([FromRoute] string slug, IBlogRepository blogRepository, IMapper mapper)
        {
            var post = await blogRepository.GetPostBySlugAsync(slug, true);

            return post == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy slug '{slug}'"))
                : Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(post)));
        }

        private static async Task<IResult> AddPost(
              PostEditModel model,
              IMapper mapper,
              IBlogRepository blogRepository,
              ICategoryRepository categoryRepository,
              IAuthorRepository authorRepository,
              IMediaManager mediaManager)
        {

            if (await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            if (await authorRepository.GetAuthorByIdAsync(model.AuthorId) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Không tìm thấy tác giả id = {model.AuthorId}"));

            }

            if (await categoryRepository.GetCategoryByIdAsync(model.CategoryId) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Không tìm thấy chủ đề id = {model.CategoryId}"));
            }

            var post = mapper.Map<Post>(model);
            post.PostedDate = DateTime.Now;

            await blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags());

            return Results.Ok(ApiResponse.Success(mapper.Map<PostDto>(post), HttpStatusCode.Created));
        }

        private static async Task<IResult> SetImagePost(int id, IFormFile file, IBlogRepository blogRepository, IMediaManager media)
        {
            var imgUrl = await media.SaveFileAsync(file.OpenReadStream(), file.FileName, file.ContentType);

            if (string.IsNullOrWhiteSpace(imgUrl))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            await blogRepository.SetImageUrlPostAsync(id, imgUrl);
            return Results.Ok(ApiResponse.Success(imgUrl));
        }

        private static async Task<IResult> UpdatePost(int id, PostEditModel model, IBlogRepository blogRepository, IMapper mapper)
        {
            if (await blogRepository.IsPostSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));

            }

            var post = await blogRepository.GetPostByIdAsync(id);
            mapper.Map(model, post);
            post.Id = id;
            post.ModifiedDate = DateTime.Now;

            return await blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags()) != null
                ? Results.Ok(ApiResponse.Success("Post is updated", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find post"));
        }

        private static async Task<IResult> DeletePost(int id, IBlogRepository blogRepository)
        {
            return await blogRepository.DeletePostByIdAsync(id)
                    ? Results.Ok(ApiResponse.Success($"Xóa thành công bài viết có Id = {id}"))
                    : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có Id = {id}"));
        }
    }
}
