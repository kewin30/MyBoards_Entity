using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBoards_Entity.Entities.Configurations
{
    public class IssueConfiguration : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder.Property(wi => wi.Effort)
                   .HasColumnType("decimal(5,2)");
        }
    }
}
