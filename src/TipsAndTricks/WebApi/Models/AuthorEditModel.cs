namespace WebApi.Models
{
    public class AuthorEditModel
    {
        public string Fullname { get; set; }
        public string UrlSlug { get; set; }
        public DateTime JoinedDate { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
    }
}
