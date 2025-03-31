using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
 public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.ToTable("Vote");

            builder.Property(p => p.Id)
                .HasColumnName("Id_Vote")
                .HasColumnType("uniqueidentifier")
                .HasColumnType("binary(16)") 
                .IsRequired();

            builder.Property(p => p.UserId)
                .HasColumnName("UserId")
                .HasColumnType("binary(16)") 
                .IsRequired();

            

            builder.Property(p => p.Value)
                .HasColumnName("Value")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(p => p.VotedAt)
                .HasColumnName("VotedAt")
                .HasColumnType("datetime")
                .IsRequired();


                builder.Property(p => p.IdeaId)
                .HasColumnName("IdeaId")
                .HasColumnType("binary(16)") 
                .IsRequired();




                builder.HasOne(p => p.Idea)
                      .WithMany(c => c.Votes)
                      .HasForeignKey(p => p.IdeaId)
                      .OnDelete(DeleteBehavior.Cascade);



                      
                 var idea1Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
                 var idea2Id = Guid.Parse("44444444-4444-4444-4444-444444444444");


            builder.HasData(
                new Vote
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Parse("22d73cec-3c5d-4821-af6a-21c3fc3568ca"),
                    Value = 1,  // like
                    VotedAt = DateTime.UtcNow,
                    IdeaId = idea1Id

                },
                new Vote
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Parse("22d73cec-3c5d-4821-af6a-21c3fc3568ca"), 
                    Value = -1,  //dislike
                    VotedAt = DateTime.UtcNow,
                    IdeaId = idea2Id

                }
            );
        }
    }