using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
	public class BlogController : Controller
	{
		private readonly IBlogRepository _blogRepository;

		public BlogController(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		public async Task<IActionResult> Index(
			[FromQuery(Name = "k")] string keyword = null,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 10
		)
		{
			// Tao doi tuong chua cac dieu kien truy van
			var postQuery = new PostQuery()
			{
				// Chi lay nhung bai viet co trang thai Published
				PublishedOnly = true,

				// Tim bai viet theo tu khoa
				Keyword = keyword
			};

			// Truy van cac bai viet theo dieu kien da tao
			var postList = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);

			// Luu lai dieu kien  truy van de hien thi trong View
			ViewBag.PostQuery = postQuery;

			ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm:ss");

			// Truyen danh sach bai viet vao View de render ra HTML
			return View(postList);
		}

		public IActionResult About() => View();
		public IActionResult Contact() => View();
		public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");
    }
}
