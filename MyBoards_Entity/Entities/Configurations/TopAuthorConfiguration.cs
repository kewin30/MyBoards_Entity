using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBoards_Entity.Entities.ViewModels;

namespace MyBoards_Entity.Entities.Configurations
{
    public class TopAuthorConfiguration : IEntityTypeConfiguration<TopAuthor>
    {
        public void Configure(EntityTypeBuilder<TopAuthor> eb)
        {
            eb.ToView("View_TopAuthors");
            eb.HasNoKey();
        }
    }
}
