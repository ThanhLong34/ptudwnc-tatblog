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
    public interface ITagRepository
    {
        public Task<Tag> GetTagAsync(
            string slug, CancellationToken cancellationToken = default);

        public Task<IList<TagItem>> GetTagsAsync(
            CancellationToken cancellationToken = default);

        public Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams, CancellationToken cancellationToken = default);


        public Task<Tag> GetTagByIdAsync(int tagId, CancellationToken cancellationToken = default);
        public Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedTagsAsync<T>(
             TagQuery query,
             IPagingParams pagingParams,
             Func<IQueryable<Tag>, IQueryable<T>> mapper,
             CancellationToken cancellationToken = default);

        Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams,
            string name = null, CancellationToken cancellationToken = default);

        public Task<bool> DeleteTagAsync(
            int tagId, CancellationToken cancellationToken = default);

        public Task<bool> CreateOrUpdateTagAsync(
            Tag tag, CancellationToken cancellationToken = default);

        public Task<bool> IsTagSlugExistAsync(string slug, CancellationToken cancellationToken = default);
    }
}
