using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ILoveAudi.Models;

namespace ILoveAudi.Migrations
{
    [DbContext(typeof(ILoveAudiContext))]
    [Migration("20170207201259_Cars")]
    partial class Cars
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ILoveAudi.Models.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.ToTable("Cars");
                });
        }
    }
}
