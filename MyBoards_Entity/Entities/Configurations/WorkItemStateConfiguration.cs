using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBoards_Entity.Entities.Configurations
{
    public class WorkItemStateConfiguration : IEntityTypeConfiguration<WorkItemState>
    {
        public void Configure(EntityTypeBuilder<WorkItemState> builder)
        {
            builder.HasData(
                new WorkItemState() { Id = 1, Value = "To do" },
                new WorkItemState() { Id = 2, Value = "Doing" },
                new WorkItemState() { Id = 3, Value = "Done" });
            builder.Property(s => s.Value)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
