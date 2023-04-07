using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
    public class CategoryQuery
    {
        public int Id { get; set; }
        public bool ShowOnMenu { get; set; }
        public string KeyWord { get; set; }
    }
}
