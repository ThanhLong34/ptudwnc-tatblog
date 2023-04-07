using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public async Task<Tag> GetTagAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
        }

        public async Task<IList<TagItem>> GetTagsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .OrderBy(x => x.Name)
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<Tag> GetTagByIdAsync(int tagId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>().FindAsync(tagId);
        }

        public async Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                   .FirstOrDefaultAsync(c => c.UrlSlug.Equals(slug), cancellationToken);
        }

        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .OrderBy(x => x.Name)
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IPagedList<T>> GetPagedTagsAsync<T>(
             TagQuery query,
             IPagingParams pagingParams,
             Func<IQueryable<Tag>, IQueryable<T>> mapper,
             CancellationToken cancellationToken = default)
        {
            IQueryable<Tag> tagQueryable = FilterTags(query);

            return await mapper(tagQueryable)
              .ToPagedListAsync(pagingParams);
        }

        public async Task<IPagedList<Tag>> GetPagedTagsAsync(TagQuery condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterTags(condition).ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Tag.Name), "DESC",
                cancellationToken);
        }

        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .Include(p => p.Posts)
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(name), x => x.Name.Contains(name))
                .Select(x => new TagItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public IQueryable<Tag> FilterTags(
            TagQuery condition)
        {
            return _context.Set<Tag>()
                .Include(t => t.Posts)
                .WhereIf(!string.IsNullOrWhiteSpace(condition.Keyword), t =>
                    t.Name.Contains(condition.Keyword)
                    || t.Description.Contains(condition.Keyword)
                    || t.UrlSlug.Contains(condition.Keyword));
        }

        public async Task<bool> DeleteTagAsync(
            int tagId, CancellationToken cancellationToken = default)
        {
            //var tag = await _context.Set<Tag>().FindAsync(tagId);

            //if (tag == null) return false;

            //_context.Set<Tag>().Remove(tag);
            //return await _context.SaveChangesAsync(cancellationToken) > 0;

            return await _context.Set<Tag>()
                .Where(x => x.Id == tagId)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<bool> CreateOrUpdateTagAsync(
            Tag tag, CancellationToken cancellationToken = default)
        {
            if (tag.Id > 0)
            {
                _context.Set<Tag>().Update(tag);
            }
            else
            {
                _context.Set<Tag>().Add(tag);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> IsTagSlugExistAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>().AnyAsync(c => c.UrlSlug.Equals(slug), cancellationToken);
        }
    }
}
