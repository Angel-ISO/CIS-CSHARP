
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;

 public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.ToTable("Topic");

            builder.Property(p => p.Id)
                .HasColumnName("Id_Topic")
                .HasColumnType("uniqueidentifier")
                .HasColumnType("binary(16)") 
                .IsRequired();

            builder.Property(p => p.Title)
                .HasColumnName("Title")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();


                builder.Property(p => p.Username)
                .HasColumnName("Username")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnName("Description")
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(p => p.UserId)
                .HasColumnName("UserId")
                .HasColumnType("binary(16)") 
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("datetime")
                .IsRequired();


                



                var topic1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
                var topic2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");

            builder.HasData(
                new Topic
                {
                    Id = topic1Id,
                    Title = "How to implement CQRS",
                    Description = "Discussion on implementing CQRS in a microservices architecture.",
                    Username= "angelito_374",
                    UserId = Guid.Parse("e208b071-d6fc-4117-abae-093dc0420864"),  
                    CreatedAt = DateTime.UtcNow
                },
                new Topic
                {
                    Id = topic2Id,
                    Title = "Best practices for clean architecture",
                    Description = "Best practices for building clean and maintainable software architectures.",
                    Username= "angelito_374",
                    UserId = Guid.Parse("e208b071-d6fc-4117-abae-093dc0420864"),  
                    CreatedAt = DateTime.UtcNow
                }
            );
        }

}