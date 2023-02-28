using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings
{
	public class CategoryMap : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.ToTable("Categories");
			builder.HasKey(a => a.Id);
			builder.Property(a => a.Name).IsRequired().HasMaxLength(50);
			builder.Property(a => a.Description).HasMaxLength(500);
			builder.Property(a => a.UrlSlug).IsRequired().HasMaxLength(50);
			builder.Property(a => a.ShowOnMenu).IsRequired().HasDefaultValue(false);
		}
	}
}
