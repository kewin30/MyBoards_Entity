﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBoards_Entity.Entities.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(u => u.Address)
                   .WithOne(u => u.User)
                   .HasForeignKey<Address>(a => a.UserId);
            builder.HasIndex(u => u.Email);
        }
    }
}
