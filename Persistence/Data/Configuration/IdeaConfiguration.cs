using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using iText.StyledXmlParser.Jsoup.Parser;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
   public class IdeaConfiguration : IEntityTypeConfiguration<Idea>
    {
        public void Configure(EntityTypeBuilder<Idea> builder)
        {
            builder.ToTable("Idea");

            builder.Property(p => p.Id)
                .HasColumnName("Id_Idea")
                .HasColumnType("uniqueidentifier")
                .HasColumnType("binary(16)") 
                .IsRequired();

            builder.Property(p => p.TopicId)
                .HasColumnName("TopicId")
                .HasColumnType("binary(16)") 
                .IsRequired();

            builder.Property(p => p.Title)
                .HasColumnName("Title")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnName("Description")
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsRequired();

                builder.Property(p => p.Username)
                .HasColumnName("Username")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.UserId)
                .HasColumnName("UserId")
                .HasColumnType("binary(16)") 
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("datetime")
                .IsRequired();




            // relationship, one topic haves many ideas
            builder.HasOne(p => p.Topic)
                      .WithMany(c => c.Ideas)
                      .HasForeignKey(p => p.TopicId)
                      .OnDelete(DeleteBehavior.Cascade);
                

                var topic1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
                var topic2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");



                var idea1Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
                var idea2Id = Guid.Parse("44444444-4444-4444-4444-444444444444");

            builder.HasData(
                new Idea
                {
                    Id = idea1Id,
                    TopicId = topic1Id, 
                    Title = "Implementing caching in CQRS",
                    Description = "Discussing how caching can improve performance in CQRS systems.",
                    Username= "catriel_72",
                    UserId = Guid.Parse("44416e81-dd00-4336-8401-e1457cd2cf9e"),  
                    CreatedAt = DateTime.UtcNow
                },
                new Idea
                {
                    Id = idea2Id,
                    TopicId = topic1Id, 
                    Title = "Domain events in CQRS",
                    Description = "Exploring the usage of domain events in CQRS systems.",
                    Username= "catriel_72",
                    UserId = Guid.Parse("44416e81-dd00-4336-8401-e1457cd2cf9e"),  
                    CreatedAt = DateTime.UtcNow
                }
            );
        }

    }
