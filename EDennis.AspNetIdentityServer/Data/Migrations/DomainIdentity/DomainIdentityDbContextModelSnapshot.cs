﻿// <auto-generated />
using System;
using EDennis.NetStandard.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EDennis.AspNetIdentityServer.Data.Migrations.DomainIdentity
{
    [DbContext(typeof(DomainIdentityDbContext))]
    partial class DomainIdentityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("Relational:Sequence:.seqAspNetRoleClaims", "'seqAspNetRoleClaims', '', '1', '1', '', '', 'Int32', 'False'")
                .HasAnnotation("Relational:Sequence:.seqAspNetRoles", "'seqAspNetRoles', '', '1', '1', '', '', 'Int32', 'False'")
                .HasAnnotation("Relational:Sequence:.seqAspNetUserClaims", "'seqAspNetUserClaims', '', '1', '1', '', '', 'Int32', 'False'")
                .HasAnnotation("Relational:Sequence:.seqAspNetUsers", "'seqAspNetUsers', '', '1', '1', '', '', 'Int32', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EDennis.NetStandard.Base.DomainApplication", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128)
                        .IsUnicode(false);

                    b.HasKey("Name");

                    b.ToTable("AspNetApplications");

                    b.HasData(
                        new
                        {
                            Name = "DataGenie"
                        },
                        new
                        {
                            Name = "InfoMaster"
                        });
                });

            modelBuilder.Entity("EDennis.NetStandard.Base.DomainApplicationClaim", b =>
                {
                    b.Property<string>("Application")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128)
                        .IsUnicode(false);

                    b.Property<string>("ClaimType")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128)
                        .IsUnicode(false);

                    b.Property<string>("ClaimValue")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128)
                        .IsUnicode(false);

                    b.Property<bool>("OrgAdminable")
                        .HasColumnType("bit");

                    b.HasKey("Application", "ClaimType", "ClaimValue");

                    b.ToTable("AspNetApplicationClaims");

                    b.HasData(
                        new
                        {
                            Application = "DataGenie",
                            ClaimType = "app:role",
                            ClaimValue = "admin",
                            OrgAdminable = true
                        },
                        new
                        {
                            Application = "DataGenie",
                            ClaimType = "app:role",
                            ClaimValue = "user",
                            OrgAdminable = true
                        },
                        new
                        {
                            Application = "InfoMaster",
                            ClaimType = "app:role",
                            ClaimValue = "admin",
                            OrgAdminable = true
                        },
                        new
                        {
                            Application = "InfoMaster",
                            ClaimType = "app:role",
                            ClaimValue = "readonly",
                            OrgAdminable = true
                        },
                        new
                        {
                            Application = "InfoMaster",
                            ClaimType = "app:role",
                            ClaimValue = "auditor",
                            OrgAdminable = false
                        });
                });

            modelBuilder.Entity("EDennis.NetStandard.Base.DomainOrganization", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128)
                        .IsUnicode(false);

                    b.HasKey("Name");

                    b.ToTable("AspNetOrganizations");

                    b.HasData(
                        new
                        {
                            Name = "McDougall's"
                        },
                        new
                        {
                            Name = "Burger Squire"
                        },
                        new
                        {
                            Name = "Windy's"
                        });
                });

            modelBuilder.Entity("EDennis.NetStandard.Base.DomainOrganizationApplication", b =>
                {
                    b.Property<string>("Organization")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128)
                        .IsUnicode(false);

                    b.Property<string>("Application")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128)
                        .IsUnicode(false);

                    b.HasKey("Organization", "Application");

                    b.ToTable("AspNetOrganizationApplications");

                    b.HasData(
                        new
                        {
                            Organization = "McDougall's",
                            Application = "DataGenie"
                        },
                        new
                        {
                            Organization = "Burger Squire",
                            Application = "DataGenie"
                        },
                        new
                        {
                            Organization = "Burger Squire",
                            Application = "InfoMaster"
                        },
                        new
                        {
                            Organization = "Windy's",
                            Application = "InfoMaster"
                        });
                });

            modelBuilder.Entity("EDennis.NetStandard.Base.DomainUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR seqAspNetUsers");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutBegin")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("Organization")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128)
                        .IsUnicode(false);

                    b.Property<bool>("OrganizationAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("OrganizationConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SuperAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "f9db615d-a76c-417b-8c9a-2d7bbfdb2d37",
                            Email = "moe@mcdougalls.com",
                            EmailConfirmed = true,
                            NormalizedEmail = "MOE@MCDOUGALLS.COM",
                            NormalizedUserName = "MOE@MCDOUGALLS.COM",
                            Organization = "McDougall's",
                            OrganizationAdmin = true,
                            OrganizationConfirmed = false,
                            PasswordHash = "AHQhhv/JbmFkA5O3OJyamWfdtRAt5eC3YuR9FOwmNHwZsgkGX80SckP/C6uLO6AtBg==",
                            PhoneNumber = "000-111-2222",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "8a908948-f7a2-45a4-a2cc-ba4b121d6a62",
                            SuperAdmin = false,
                            TwoFactorEnabled = false,
                            UserName = "moe@mcdougalls.com"
                        },
                        new
                        {
                            Id = -2,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "7ee29c84-9b20-4f23-8c4c-1a7ad68a391e",
                            Email = "larry@burgersquire.com",
                            EmailConfirmed = true,
                            NormalizedEmail = "LARRY@BURGERSQUIRE.COM",
                            NormalizedUserName = "LARRY@BURGERSQUIRE.COM",
                            Organization = "Burger Squire",
                            OrganizationAdmin = true,
                            OrganizationConfirmed = false,
                            PasswordHash = "ALG1OP/NCWjFpC5rSBSDrauVvPA3EbMxby77Uwt8UKpH58agaCXlnIv1cg4xQE/iHA==",
                            PhoneNumber = "111-222-3333",
                            PhoneNumberConfirmed = true,
                            SecurityStamp = "42243ffc-d18c-4b22-890e-bd4c519ddcf7",
                            SuperAdmin = false,
                            TwoFactorEnabled = false,
                            UserName = "larry@burgersquire.com"
                        },
                        new
                        {
                            Id = -3,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "dcb1bd4e-df97-4f43-a60c-9bedd760709c",
                            Email = "curly@windys.com",
                            EmailConfirmed = true,
                            NormalizedEmail = "CURLY@WINDYS.COM",
                            NormalizedUserName = "CURLY@WINDYS.COM",
                            Organization = "Windy's",
                            OrganizationAdmin = true,
                            OrganizationConfirmed = false,
                            PasswordHash = "AEHKZugG3D/te0QUgaQ38qtmApdbvE9TjpS38iG/btctQsuWudaCH3UGTAvv6c5qIA==",
                            PhoneNumber = "222-333-4444",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "83d6a4a8-4ca3-4a32-aecc-f36c13dfcb9c",
                            SuperAdmin = false,
                            TwoFactorEnabled = false,
                            UserName = "curly@windys.com"
                        },
                        new
                        {
                            Id = -4,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "cd4b8ee6-2df0-4bfe-881e-2ed03b93ed51",
                            Email = "marcia@mcdougalls.com",
                            EmailConfirmed = true,
                            NormalizedEmail = "MARCIA@MCDOUGALLS.COM",
                            NormalizedUserName = "MARCIA@MCDOUGALLS.COM",
                            Organization = "McDougall's",
                            OrganizationAdmin = false,
                            OrganizationConfirmed = false,
                            PasswordHash = "AHc7eMal2Ju+KxGC+DNX5ykpx/dPdmhq4z4mPoJ3cEL1BYu78szo+EqmDdmqSpGE4Q==",
                            PhoneNumber = "000-111-2223",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "0732f0a2-1ddc-44e1-91bf-eba6a70d0c9a",
                            SuperAdmin = false,
                            TwoFactorEnabled = false,
                            UserName = "marcia@mcdougalls.com"
                        },
                        new
                        {
                            Id = -5,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "b64813f6-221c-4a26-8ef8-cf6f380c9db2",
                            Email = "jan@burgersquire.com",
                            EmailConfirmed = true,
                            NormalizedEmail = "JAN@BURGERSQUIRE.COM",
                            NormalizedUserName = "JAN@BURGERSQUIRE.COM",
                            Organization = "Burger Squire",
                            OrganizationAdmin = false,
                            OrganizationConfirmed = false,
                            PasswordHash = "AGClgbBglDK154+J/FK9rR0yAzwS8TM029v6bZSEAd/ByzszB4RYa/zBolN34s8F6g==",
                            PhoneNumber = "111-222-3334",
                            PhoneNumberConfirmed = true,
                            SecurityStamp = "c3d92933-c2fc-48e2-bfa9-82819292f2bf",
                            SuperAdmin = false,
                            TwoFactorEnabled = false,
                            UserName = "jan@burgersquire.com"
                        },
                        new
                        {
                            Id = -6,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "dc643442-e6a5-4b13-82ca-687ca7aea13f",
                            Email = "cindy@windys.com",
                            EmailConfirmed = true,
                            NormalizedEmail = "CINDY@WINDYS.COM",
                            NormalizedUserName = "CINDY@WINDYS.COM",
                            Organization = "Windy's",
                            OrganizationAdmin = false,
                            OrganizationConfirmed = false,
                            PasswordHash = "AKonnfUXy2u6fFkWm5wH7KCO7EPylRXBRSQy02rw3y+VawwCLj8gy+aUQL/C9Z3e0g==",
                            PhoneNumber = "222-333-4445",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "0c2894ae-0f6c-4e23-9c80-9628c8ec38a7",
                            SuperAdmin = false,
                            TwoFactorEnabled = false,
                            UserName = "cindy@windys.com"
                        },
                        new
                        {
                            Id = -7,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "88a26bdb-f4c0-46be-98e1-67d9a767dd6f",
                            Email = "greg@mcdougalls.com",
                            EmailConfirmed = true,
                            NormalizedEmail = "GREG@MCDOUGALLS.COM",
                            NormalizedUserName = "GREG@MCDOUGALLS.COM",
                            Organization = "McDougall's",
                            OrganizationAdmin = false,
                            OrganizationConfirmed = false,
                            PasswordHash = "APFsitEgUngOUmkzmJX4FcC0QWUD3j3itJ4kGejCqdNR8FMeNfnay53l9wBJbAjlVw==",
                            PhoneNumber = "000-111-2224",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "33873d4c-7429-428e-b7fd-04e0b859fef7",
                            SuperAdmin = false,
                            TwoFactorEnabled = false,
                            UserName = "greg@mcdougalls.com"
                        },
                        new
                        {
                            Id = -8,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "dd41f3b8-70f7-4e55-92ed-0acb00e64912",
                            Email = "peter@burgersquire.com",
                            EmailConfirmed = true,
                            NormalizedEmail = "PETER@BURGERSQUIRE.COM",
                            NormalizedUserName = "PETER@BURGERSQUIRE.COM",
                            Organization = "Burger Squire",
                            OrganizationAdmin = false,
                            OrganizationConfirmed = false,
                            PasswordHash = "APd0egA0D56GUOXi94zvCXi6V6+oiaAeqemgLjiDYYAx6oVpTM/vJZsY/6qykRTZiQ==",
                            PhoneNumber = "111-222-3335",
                            PhoneNumberConfirmed = true,
                            SecurityStamp = "f71c44eb-51b8-4aed-a92b-a763387e9501",
                            SuperAdmin = false,
                            TwoFactorEnabled = false,
                            UserName = "peter@burgersquire.com"
                        },
                        new
                        {
                            Id = -9,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "65ae7b12-27cb-4bde-8bab-416e57414c9d",
                            Email = "bobby@windys.com",
                            EmailConfirmed = true,
                            LockoutBegin = new DateTimeOffset(new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -5, 0, 0, 0)),
                            LockoutEnd = new DateTimeOffset(new DateTime(2030, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -5, 0, 0, 0)),
                            NormalizedEmail = "BOBBY@WINDYS.COM",
                            NormalizedUserName = "BOBBY@WINDYS.COM",
                            Organization = "Windy's",
                            OrganizationAdmin = false,
                            OrganizationConfirmed = false,
                            PasswordHash = "ALa6v4WiOqr74SpTWqR1nYCBjV4IffezJ9U1lDsnWcBw4cIQFqNkpiK9M5f6XxSgiQ==",
                            PhoneNumber = "222-333-4446",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "6cfffb56-34e9-425f-b4ab-575c66939eb3",
                            SuperAdmin = false,
                            TwoFactorEnabled = false,
                            UserName = "bobby@windys.com"
                        },
                        new
                        {
                            Id = -10,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "89e3aefc-48b6-4c00-a2ea-3e5949438e2e",
                            Email = "alice@windys.com",
                            EmailConfirmed = true,
                            NormalizedEmail = "ALICE@WINDYS.COM",
                            NormalizedUserName = "ALICE@WINDYS.COM",
                            Organization = "Windy's",
                            OrganizationAdmin = false,
                            OrganizationConfirmed = false,
                            PasswordHash = "ABECH/3HNbsodHmwTd7Di9HAIDvmFKfbWynWnaGOn7wVQNyNvjRuaZC7sCBgUYxs/A==",
                            PhoneNumber = "222-333-4446",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "ae61c7b2-80c7-409c-939b-176f24416b8b",
                            SuperAdmin = false,
                            TwoFactorEnabled = false,
                            UserName = "alice@windys.com"
                        },
                        new
                        {
                            Id = -11,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "e242e8fc-d5d0-40fe-b7b8-7dbd570b3167",
                            Email = "sheldon@burgersquire.com",
                            EmailConfirmed = true,
                            NormalizedEmail = "SHELDON@BURGERSQUIRE.COM",
                            NormalizedUserName = "SHELDON@BURGERSQUIRE.COM",
                            Organization = "Burger Squire",
                            OrganizationAdmin = false,
                            OrganizationConfirmed = false,
                            PasswordHash = "AGUebo3/plsz2lMKYn7bZ/BIFk3gQvZ5m3rsbGl9SNvZNVyoa6IGJtaPjIpyfOj2tw==",
                            PhoneNumber = "999-888-7777",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "3d82bce7-da76-4d86-8605-d2594f80180b",
                            SuperAdmin = false,
                            TwoFactorEnabled = false,
                            UserName = "sheldon@burgersquire.com"
                        });
                });

            modelBuilder.Entity("EDennis.NetStandard.Base.DomainUserHistory", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateReplaced")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutBegin")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Organization")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<bool>("OrganizationAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("OrganizationConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ReplacedBy")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("UserClaims")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id", "DateReplaced");

                    b.ToTable("AspNetUsersHistory");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR seqAspNetRoles");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR seqAspNetRoleClaims");

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR seqAspNetUserClaims");

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");

                    b.HasData(
                        new
                        {
                            Id = -9902,
                            ClaimType = "app:role",
                            ClaimValue = "DataGenie:admin",
                            UserId = -1
                        },
                        new
                        {
                            Id = -9904,
                            ClaimType = "app:role",
                            ClaimValue = "DataGenie:admin",
                            UserId = -2
                        },
                        new
                        {
                            Id = -9905,
                            ClaimType = "app:role",
                            ClaimValue = "InfoMaster:admin",
                            UserId = -2
                        },
                        new
                        {
                            Id = -9907,
                            ClaimType = "app:role",
                            ClaimValue = "InfoMaster:admin",
                            UserId = -3
                        },
                        new
                        {
                            Id = -9908,
                            ClaimType = "app:role",
                            ClaimValue = "DataGenie:user",
                            UserId = -4
                        },
                        new
                        {
                            Id = -9909,
                            ClaimType = "app:role",
                            ClaimValue = "DataGenie:user",
                            UserId = -5
                        },
                        new
                        {
                            Id = -9910,
                            ClaimType = "app:role",
                            ClaimValue = "InfoMaster:readonly",
                            UserId = -5
                        },
                        new
                        {
                            Id = -9911,
                            ClaimType = "app:role",
                            ClaimValue = "InfoMaster:readonly",
                            UserId = -6
                        },
                        new
                        {
                            Id = -9912,
                            ClaimType = "app:role",
                            ClaimValue = "DataGenie:user",
                            UserId = -7
                        },
                        new
                        {
                            Id = -9913,
                            ClaimType = "app:role",
                            ClaimValue = "DataGenie:user",
                            UserId = -8
                        },
                        new
                        {
                            Id = -9914,
                            ClaimType = "app:role",
                            ClaimValue = "InfoMaster:readonly",
                            UserId = -8
                        },
                        new
                        {
                            Id = -9915,
                            ClaimType = "app:role",
                            ClaimValue = "InfoMaster:readonly",
                            UserId = -9
                        },
                        new
                        {
                            Id = -9916,
                            ClaimType = "app:role",
                            ClaimValue = "InfoMaster:auditor",
                            UserId = -10
                        },
                        new
                        {
                            Id = -9917,
                            ClaimType = "*:role",
                            ClaimValue = "*:admin",
                            UserId = -11
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("EDennis.NetStandard.Base.DomainUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("EDennis.NetStandard.Base.DomainUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EDennis.NetStandard.Base.DomainUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("EDennis.NetStandard.Base.DomainUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
