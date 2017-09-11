using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WebCore.Models;

namespace WebCore.Migrations
{
    [DbContext(typeof(WebCoreContext))]
    [Migration("20170911210614_Culture_Active_Default_value")]
    partial class Culture_Active_Default_value
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebCore.Models.Culture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("(1)");

                    b.Property<bool>("Default");

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Culture");
                });

            modelBuilder.Entity("WebCore.Models.Translation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CultureId");

                    b.Property<string>("Key");

                    b.Property<DateTime>("Modify_DT")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<string>("Text")
                        .HasMaxLength(1000);

                    b.HasKey("Id");

                    b.HasIndex("CultureId");

                    b.ToTable("Translation");
                });

            modelBuilder.Entity("WebCore.Models.Translation", b =>
                {
                    b.HasOne("WebCore.Models.Culture", "Culture")
                        .WithMany("Translations")
                        .HasForeignKey("CultureId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
