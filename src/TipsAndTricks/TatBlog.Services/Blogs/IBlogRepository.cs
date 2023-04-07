using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        public Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default);

        public Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default);

        public Task<Author> GetAuthorAsync(string slug, CancellationToken cancellationToken = default);

        public Task<Author> GetAuthorByIdAsync(int authorId);

        public Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default);

        public Task<IList<Post>> GetPostsAsync(
            PostQuery condition,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        public Task<int> CountPostsAsync(
            PostQuery condition, CancellationToken cancellationToken = default);

        //public Task<IList<MonthlyPostCountItem>> CountMonthlyPostsAsync(
        //    int numMonths, CancellationToken cancellationToken = default);

        public Task<Category> GetCategoryAsync(
            string slug, CancellationToken cancellationToken = default);

        public Task<Category> GetCategoryByIdAsync(int categoryId);

        public Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken = default);

        public Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        public Task<Category> CreateOrUpdateCategoryAsync(
            Category category, CancellationToken cancellationToken = default);

        public Task<bool> IsCategorySlugExistedAsync(
            int categoryId, string categorySlug,
            CancellationToken cancellationToken = default);

        public Task<bool> DeleteCategoryAsync(
            int categoryId, CancellationToken cancellationToken = default);

        public Task<Tag> GetTagAsync(
            string slug, CancellationToken cancellationToken = default);

        public Task<IList<TagItem>> GetTagsAsync(
            CancellationToken cancellationToken = default);

        public Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams, CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedTagsAsync<T>(
             TagQuery query,
             IPagingParams pagingParams,
             Func<IQueryable<Tag>, IQueryable<T>> mapper,
             CancellationToken cancellationToken = default);

        public Task<bool> DeleteTagAsync(
            int tagId, CancellationToken cancellationToken = default);

        public Task<bool> CreateOrUpdateTagAsync(
            Tag tag, CancellationToken cancellationToken = default);

        public Task<Post> GetPostAsync(
            string slug,
            CancellationToken cancellationToken = default);

        public Task<Post> GetPostByIdAsync(
            int postId, bool includeDetails = false,
            CancellationToken cancellationToken = default);

        public Task<bool> TogglePublishedFlagAsync(
            int postId, CancellationToken cancellationToken = default);

        public Task<IList<Post>> GetRandomArticlesAsync(
            int numPosts, CancellationToken cancellationToken = default);

        public Task<IPagedList<Post>> GetPagedPostsAsync(
            PostQuery condition,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);

        public Task<IPagedList<T>> GetPagedPostsAsync<T>(
            PostQuery condition,
            IPagingParams pagingParams,
            Func<IQueryable<Post>, IQueryable<T>> mapper);

        public Task<Post> CreateOrUpdatePostAsync(
           Post post, IEnumerable<string> tags,
           CancellationToken cancellationToken = default);

        public Task<bool> IsPostSlugExistedAsync(
           int postId, string slug, CancellationToken cancellationToken = default);

        public Task TogglePostPublishedStatusAsync(int id, CancellationToken cancellationToken = default);

        public Task<bool> DeletePostByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedPostsByQueryAsync<T>(
            Func<IQueryable<Post>, IQueryable<T>> mapper, PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

        Task<IList<Post>> GetRandomPostsAsync(int num, CancellationToken cancellationToken = default);

        Task<Post> GetPostBySlugAsync(string slug, bool includeDetails = false, CancellationToken cancellationToken = default);
        Task<bool> SetImageUrlPostAsync(int postId, string imageUrl, CancellationToken cancellationToken = default);
    }
}
