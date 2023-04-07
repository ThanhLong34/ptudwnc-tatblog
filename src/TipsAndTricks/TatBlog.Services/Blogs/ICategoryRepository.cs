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
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryByIdAsync(int categoryId);
        Task<Category> GetCachedCategoryByIdAsync(int categoryId);
        Task<bool> IsCategorySlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default);
        Task<IPagedList<Category>> GetCategoriesByQuery(
            CategoryQuery condition,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
        Task<Category> FindCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<bool> AddOrEditCategoryAsync(Category newCategory, CancellationToken cancellationToken = default);

        Task<Category> FindCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> IsSlugOfCategoryExist(string slug, CancellationToken cancellationToken = default);

        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams,
            string name = null, CancellationToken cancellationToken = default);

        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken = default);
    }
}
