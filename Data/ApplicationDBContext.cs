using ItransitionTemplates.Models;
using Microsoft.EntityFrameworkCore;

namespace ItransitionTemplates.Data
{
    public class ApplicationDBContext : DbContext {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<UserAllowedToAnswer> UserAllowedToAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //----Set primary keys
            //Like
            modelBuilder.Entity<Like>().HasKey(l => new { l.TemplateId, l.UserId });
            //Admin
            modelBuilder.Entity<Admin>().HasKey(a => new { a.UserId, a.TemplateId });
            //UserAllowedToAnswer
            modelBuilder.Entity<UserAllowedToAnswer>().HasKey(u => new { u.UserId, u.TemplateId });
            //Comment
            modelBuilder.Entity<Comment>().HasKey(c => new { c.UserId, c.TemplateId });

            //----Relationships
            //Topic --> Template (One to Many)
            modelBuilder.Entity<Topic>()
            .HasMany(t => t.Templates)
            .WithOne(t => t.Topic);

            //Tempalte --> Like (One to Many)
            modelBuilder.Entity<Template>()
            .HasMany(t => t.Likes)
            .WithOne(l => l.Template)
            .HasForeignKey(l => l.TemplateId);

            //User --> Like (One to Many)
            modelBuilder.Entity<User>()
            .HasMany(u => u.Likes)
            .WithOne(l => l.User)
            .HasForeignKey(u => u.UserId);

            //Tag --> Template (Many to Many)
            modelBuilder.Entity<Tag>()
            .HasMany(t => t.Templates)
            .WithMany(t => t.Tags)
            .UsingEntity<Dictionary<string, object>>(
                "Tag_has_templates",
                j => j
                .HasOne<Template>()
                .WithMany()
                .HasForeignKey("TemplateId")
                .HasConstraintName("fk_Tag_has_templates_Template_TemplateId")
                .OnDelete(DeleteBehavior.Cascade),
                j => j
                .HasOne<Tag>()
                .WithMany()
                .HasForeignKey("TagId")
                .HasConstraintName("fk_Tag_has_templates_Tag_TagId")
                .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("TemplateId", "TagId");
                    j.ToTable("Tag_has_templates");
                }
            );
        }
    }
}