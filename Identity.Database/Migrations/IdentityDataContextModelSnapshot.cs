﻿// <auto-generated />
using System;
using Identity.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identity.Database.Migrations
{
    [DbContext(typeof(IdentityDataContext))]
    partial class IdentityDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.Database.Extension.AuditEntry", b =>
                {
                    b.Property<string>("AuditEntryID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(255);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("EntitySetName")
                        .HasMaxLength(255);

                    b.Property<string>("EntityTypeName")
                        .HasMaxLength(255);

                    b.Property<int>("State");

                    b.Property<string>("StateName")
                        .HasMaxLength(255);

                    b.HasKey("AuditEntryID");

                    b.ToTable("AuditEntry");
                });

            modelBuilder.Entity("Core.Database.Extension.AuditEntryProperty", b =>
                {
                    b.Property<string>("AuditEntryPropertyID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50);

                    b.Property<string>("AuditEntryID")
                        .HasMaxLength(50);

                    b.Property<string>("NewValueFormatted")
                        .HasColumnName("NewValue");

                    b.Property<string>("OldValueFormatted")
                        .HasColumnName("OldValue");

                    b.Property<string>("PropertyName")
                        .HasMaxLength(255);

                    b.Property<string>("RelationName")
                        .HasMaxLength(255);

                    b.HasKey("AuditEntryPropertyID");

                    b.HasIndex("AuditEntryID");

                    b.ToTable("AuditEntryProperty");
                });

            modelBuilder.Entity("Identity.Database.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Area")
                        .HasMaxLength(150);

                    b.Property<string>("Control")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Creator")
                        .HasMaxLength(150);

                    b.Property<string>("Modifier")
                        .HasMaxLength(150);

                    b.Property<DateTime?>("ModifyTime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int?>("OrderIndex");

                    b.Property<int?>("ParentID");

                    b.Property<string>("Remarks")
                        .HasMaxLength(500);

                    b.Property<int?>("SystemID");

                    b.HasKey("Id");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("Identity.Database.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Creator")
                        .HasMaxLength(150);

                    b.Property<string>("Modifier")
                        .HasMaxLength(150);

                    b.Property<DateTime?>("ModifyTime");

                    b.Property<string>("Name")
                        .HasMaxLength(150);

                    b.Property<string>("Remarks")
                        .HasMaxLength(500);

                    b.Property<int?>("SystemID");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Identity.Database.Entities.RolePermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Creator")
                        .HasMaxLength(150);

                    b.Property<string>("Modifier")
                        .HasMaxLength(150);

                    b.Property<DateTime?>("ModifyTime");

                    b.Property<int>("PermissionId");

                    b.Property<string>("Remarks")
                        .HasMaxLength(500);

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.ToTable("RolePermission");
                });

            modelBuilder.Entity("Identity.Database.Entities.System", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Creator")
                        .HasMaxLength(150);

                    b.Property<string>("Modifier")
                        .HasMaxLength(150);

                    b.Property<DateTime?>("ModifyTime");

                    b.Property<string>("Name")
                        .HasMaxLength(150);

                    b.Property<string>("Remarks")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("System");
                });

            modelBuilder.Entity("Identity.Database.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AliasName")
                        .HasMaxLength(150);

                    b.Property<string>("Company")
                        .HasMaxLength(150);

                    b.Property<int?>("CompanyId");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Creator")
                        .HasMaxLength(150);

                    b.Property<bool>("IsAdmin");

                    b.Property<bool>("IsDisabled");

                    b.Property<DateTime?>("LastLoginDatetime");

                    b.Property<string>("LoginName")
                        .HasMaxLength(150);

                    b.Property<int?>("LoginNumber");

                    b.Property<string>("Modifier")
                        .HasMaxLength(150);

                    b.Property<DateTime?>("ModifyTime");

                    b.Property<string>("Password")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Identity.Database.Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Creator")
                        .HasMaxLength(150);

                    b.Property<string>("Modifier")
                        .HasMaxLength(150);

                    b.Property<DateTime?>("ModifyTime");

                    b.Property<string>("Remarks")
                        .HasMaxLength(500);

                    b.Property<int>("RoleId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Identity.Database.Entities.UserSystem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Creator")
                        .HasMaxLength(150);

                    b.Property<string>("Modifier")
                        .HasMaxLength(150);

                    b.Property<DateTime?>("ModifyTime");

                    b.Property<string>("Remarks")
                        .HasMaxLength(500);

                    b.Property<int>("SystemId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserSystem");
                });

            modelBuilder.Entity("Core.Database.Extension.AuditEntryProperty", b =>
                {
                    b.HasOne("Core.Database.Extension.AuditEntry", "Parent")
                        .WithMany("Properties")
                        .HasForeignKey("AuditEntryID");
                });
#pragma warning restore 612, 618
        }
    }
}
