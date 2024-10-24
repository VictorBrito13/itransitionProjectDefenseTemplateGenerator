// <auto-generated />
using System;
using ItransitionTemplates.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ItransitionTemplates.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20241012030739_version_4.6")]
    partial class version_46
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("ItransitionTemplates.Models.Admin", b =>
                {
                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("TemplateId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("UserId", "TemplateId");

                    b.HasIndex("TemplateId");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Comment", b =>
                {
                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("TemplateId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("UserId", "TemplateId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Like", b =>
                {
                    b.Property<ulong>("TemplateId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("TemplateId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Question", b =>
                {
                    b.Property<ulong>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<ulong>("QuestionId"));

                    b.Property<string>("QuestionString")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<ulong>("TemplateId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("QuestionId");

                    b.HasIndex("TemplateId");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.QuestionOption", b =>
                {
                    b.Property<ulong>("QuestionOptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<ulong>("QuestionOptionId"));

                    b.Property<string>("Option")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<ulong>("QuestionId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("QuestionOptionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionOption");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Response", b =>
                {
                    b.Property<ulong>("ResponseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<ulong>("ResponseId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<ulong>("QuestionId")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("ResponseString")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("ResponseId");

                    b.HasIndex("QuestionId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Response");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Tag", b =>
                {
                    b.Property<ulong>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<ulong>("TagId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("TagId");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Template", b =>
                {
                    b.Property<ulong>("TemplateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<ulong>("TemplateId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Image_url")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<ulong>("TopicId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("TemplateId");

                    b.HasIndex("TopicId");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Topic", b =>
                {
                    b.Property<ulong>("TopicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<ulong>("TopicId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("TopicId");

                    b.HasIndex("TopicId", "Name")
                        .IsUnique();

                    b.ToTable("Topic");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.User", b =>
                {
                    b.Property<ulong>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<ulong>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("UserId");

                    b.HasIndex(new[] { "Email" }, "IDX_User_Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.UserAllowedToAnswer", b =>
                {
                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("TemplateId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("UserId", "TemplateId");

                    b.HasIndex("TemplateId");

                    b.ToTable("UserAllowedToAnswer");
                });

            modelBuilder.Entity("Tag_has_templates", b =>
                {
                    b.Property<ulong>("TemplateId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("TagId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("TemplateId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("Tag_has_templates", (string)null);
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Admin", b =>
                {
                    b.HasOne("ItransitionTemplates.Models.Template", "Template")
                        .WithMany("Admins")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ItransitionTemplates.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Template");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Comment", b =>
                {
                    b.HasOne("ItransitionTemplates.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Like", b =>
                {
                    b.HasOne("ItransitionTemplates.Models.Template", "Template")
                        .WithMany("Likes")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ItransitionTemplates.Models.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Template");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Question", b =>
                {
                    b.HasOne("ItransitionTemplates.Models.Template", null)
                        .WithMany("Questions")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ItransitionTemplates.Models.QuestionOption", b =>
                {
                    b.HasOne("ItransitionTemplates.Models.Question", null)
                        .WithMany("QuestionOptions")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Response", b =>
                {
                    b.HasOne("ItransitionTemplates.Models.Question", "Question")
                        .WithOne("Response")
                        .HasForeignKey("ItransitionTemplates.Models.Response", "QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ItransitionTemplates.Models.User", "User")
                        .WithMany("responses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Template", b =>
                {
                    b.HasOne("ItransitionTemplates.Models.Topic", "Topic")
                        .WithMany("Templates")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.UserAllowedToAnswer", b =>
                {
                    b.HasOne("ItransitionTemplates.Models.Template", "Template")
                        .WithMany()
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ItransitionTemplates.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Template");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Tag_has_templates", b =>
                {
                    b.HasOne("ItransitionTemplates.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_Tag_has_templates_Tag_TagId");

                    b.HasOne("ItransitionTemplates.Models.Template", null)
                        .WithMany()
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_Tag_has_templates_Template_TemplateId");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Question", b =>
                {
                    b.Navigation("QuestionOptions");

                    b.Navigation("Response")
                        .IsRequired();
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Template", b =>
                {
                    b.Navigation("Admins");

                    b.Navigation("Likes");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.Topic", b =>
                {
                    b.Navigation("Templates");
                });

            modelBuilder.Entity("ItransitionTemplates.Models.User", b =>
                {
                    b.Navigation("Likes");

                    b.Navigation("responses");
                });
#pragma warning restore 612, 618
        }
    }
}
