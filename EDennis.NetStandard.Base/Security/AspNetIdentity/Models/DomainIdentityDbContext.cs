﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;

namespace EDennis.NetStandard.Base {
    public class DomainIdentityDbContext : IdentityDbContext<DomainUser,IdentityRole<int>,int> {
        public DomainIdentityDbContext(DbContextOptions<DomainIdentityDbContext> options)
            : base(options) {
        }

        public DbSet<DomainApplication> Applications { get; set; }
        public DbSet<DomainOrganization> Organizations { get; set; }
        public DbSet<DomainApplicationClaim> ApplicationClaims { get; set; }
        public DbSet<DomainOrganizationApplication> OrganizationApplications { get; set; }
        public DbSet<DomainUserHistory> UserHistories { get; set; }

        public DbSet<SearchableDomainUser> SearchableDomainUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.HasSequence<int>("seqAspNetUsers");
            builder.HasSequence<int>("seqAspNetRoles");
            builder.HasSequence<int>("seqAspNetUserClaims");
            builder.HasSequence<int>("seqAspNetRoleClaims");


            builder.Entity<IdentityUserClaim<int>>( e => 
            {
                e.Property(p => p.Id)
                .HasDefaultValueSql("NEXT VALUE FOR seqAspNetUserClaims");
            });

            builder.Entity<IdentityRole<int>>(e =>
            {
                e.Property(p => p.Id)
                .HasDefaultValueSql("NEXT VALUE FOR seqAspNetRoles");
            });

            builder.Entity<IdentityRoleClaim<int>>(e =>
            {
                e.Property(p => p.Id)
                .HasDefaultValueSql("NEXT VALUE FOR seqAspNetRoleClaims");
            });


            builder.Entity<SearchableDomainUser>(e =>
            {
                e.HasNoKey();
                e.ToView("SearchableDomainUser");
            });


            builder.Entity<DomainOrganization>(e =>
            {
                e.ToTable("AspNetOrganizations")
                    .HasKey(p => p.Name);
                e.Property(p => p.Name)
                    .IsUnicode(false)
                    .HasMaxLength(128);
                e.HasData(new DomainOrganization[] {
                    new DomainOrganization { Name = "McDougall's" },
                    new DomainOrganization { Name = "Burger Squire" },
                    new DomainOrganization { Name = "Windy's" }
                });
            });
            builder.Entity<DomainApplication>(e =>
            {
                e.ToTable("AspNetApplications")
                    .HasKey(p => p.Name);
                e.Property(p => p.Name)
                    .IsUnicode(false)
                    .HasMaxLength(128);
                e.HasData(new DomainApplication[] {
                    new DomainApplication { Name = "DataGenie" },
                    new DomainApplication { Name = "InfoMaster" },
                });
            });

            builder.Entity<DomainOrganizationApplication>(e =>
            {
                e.ToTable("AspNetOrganizationApplications")
                    .HasKey(p => new { p.Organization, p.Application });

                e.Property(p => p.Organization)
                    .IsUnicode(false)
                    .HasMaxLength(128);

                e.Property(p => p.Application)
                    .IsUnicode(false)
                    .HasMaxLength(128);

                e.HasData(new DomainOrganizationApplication[] {
                    new DomainOrganizationApplication {
                        Organization = "McDougall's",
                        Application = "DataGenie"
                    },
                    new DomainOrganizationApplication {
                        Organization = "Burger Squire",
                        Application = "DataGenie"
                    },
                    new DomainOrganizationApplication {
                        Organization = "Burger Squire",
                        Application = "InfoMaster"
                    },
                    new DomainOrganizationApplication {
                        Organization = "Windy's",
                        Application = "InfoMaster"
                    }
                });

            });


            builder.Entity<DomainApplicationClaim>(e =>
            {
                e.ToTable("AspNetApplicationClaims")
                    .HasKey(p => new { p.Application, p.ClaimType, p.ClaimValue });
                e.Property(p => p.Application)
                    .IsUnicode(false)
                    .HasMaxLength(128);
                e.Property(p => p.ClaimType)
                    .IsUnicode(false)
                    .HasMaxLength(128);
                e.Property(p => p.ClaimValue)
                    .IsUnicode(false)
                    .HasMaxLength(128);

                e.HasData(new DomainApplicationClaim[] {
                    new DomainApplicationClaim {
                        Application="DataGenie",
                        ClaimType="app:role",
                        ClaimValue="admin",
                        OrgAdminable=true
                    },
                    new DomainApplicationClaim {
                        Application="DataGenie",
                        ClaimType="app:role",
                        ClaimValue="user",
                        OrgAdminable=true
                    },
                    new DomainApplicationClaim {
                        Application="InfoMaster",
                        ClaimType="app:role",
                        ClaimValue="admin",
                        OrgAdminable=true
                    },
                    new DomainApplicationClaim {
                        Application="InfoMaster",
                        ClaimType="app:role",
                        ClaimValue="readonly",
                        OrgAdminable=true
                    },
                    new DomainApplicationClaim {
                        Application="InfoMaster",
                        ClaimType="app:role",
                        ClaimValue="auditor",
                        OrgAdminable=false
                    }
                });

            });


            builder.Entity<DomainUser>(e =>
            {
                e.Property(p => p.Organization)
                    .IsUnicode(false)
                    .HasMaxLength(128);

                e.Property(p => p.Id)
                    .HasDefaultValueSql("NEXT VALUE FOR seqAspNetUsers");

                var users =
                new DomainUser[] {
                    new DomainUser {
                        Id=-1,
                        UserName = "moe@mcdougalls.com",
                        Email = "moe@mcdougalls.com",
                        EmailConfirmed = true,
                        PhoneNumber = "000-111-2222",
                        Organization = "McDougall's",
                        OrganizationAdmin = true
                    },
                    new DomainUser {
                        Id=-2,
                        UserName = "larry@burgersquire.com",
                        Email = "larry@burgersquire.com",
                        EmailConfirmed = true,
                        PhoneNumber = "111-222-3333",
                        PhoneNumberConfirmed = true,
                        Organization = "Burger Squire",
                        OrganizationAdmin = true
                    },
                    new DomainUser {
                        Id=-3,
                        UserName = "curly@windys.com",
                        Email = "curly@windys.com",
                        EmailConfirmed = true,
                        PhoneNumber = "222-333-4444",
                        Organization = "Windy's",
                        OrganizationAdmin = true
                    },
                    new DomainUser {
                        Id=-4,
                        UserName = "marcia@mcdougalls.com",
                        Email = "marcia@mcdougalls.com",
                        EmailConfirmed = true,
                        PhoneNumber = "000-111-2223",
                        Organization = "McDougall's"
                    },
                    new DomainUser {
                        Id=-5,
                        UserName = "jan@burgersquire.com",
                        Email = "jan@burgersquire.com",
                        EmailConfirmed = true,
                        PhoneNumber = "111-222-3334",
                        PhoneNumberConfirmed = true,
                        Organization = "Burger Squire"
                    },
                    new DomainUser {
                        Id=-6,
                        UserName = "cindy@windys.com",
                        Email = "cindy@windys.com",
                        EmailConfirmed = true,
                        PhoneNumber = "222-333-4445",
                        Organization = "Windy's"
                    },
                    new DomainUser {
                        Id=-7,
                        UserName = "greg@mcdougalls.com",
                        Email = "greg@mcdougalls.com",
                        EmailConfirmed = true,
                        PhoneNumber = "000-111-2224",
                        Organization = "McDougall's"
                    },
                    new DomainUser {
                        Id=-8,
                        UserName = "peter@burgersquire.com",
                        Email = "peter@burgersquire.com",
                        EmailConfirmed = true,
                        PhoneNumber = "111-222-3335",
                        PhoneNumberConfirmed = true,
                        Organization = "Burger Squire"
                    },
                    new DomainUser {
                        Id=-9,
                        UserName = "bobby@windys.com",
                        Email = "bobby@windys.com",
                        EmailConfirmed = true,
                        PhoneNumber = "222-333-4446",
                        Organization = "Windy's",
                        LockoutBegin = new DateTime(2020,1,1),
                        LockoutEnd = new DateTime(2030,12,13)
                    },
                    new DomainUser {
                        Id=-10,
                        UserName = "alice@windys.com",
                        Email = "alice@windys.com",
                        EmailConfirmed = true,
                        PhoneNumber = "222-333-4446",
                        Organization = "Windy's"
                    },
                    new DomainUser {
                        Id=-11,
                        UserName = "sheldon@burgersquire.com",
                        Email = "sheldon@burgersquire.com",
                        EmailConfirmed = true,
                        PhoneNumber = "999-888-7777",
                        Organization = "Burger Squire"
                    }
                };
                foreach(var user in users) {
                    user.PasswordHash = HashPassword("test");
                    user.SecurityStamp = Guid.NewGuid().ToString();
                    user.ConcurrencyStamp = Guid.NewGuid().ToString();
                    user.NormalizedEmail = user.Email.ToUpper();
                    user.NormalizedUserName = user.UserName.ToUpper();
                }

                e.HasData(users);
            });


            builder.Entity<DomainUserHistory>(e =>
            {
                e.ToTable("AspNetUsersHistory")
                    .HasKey(k => new { k.Id, k.DateReplaced });
            });


            builder.Entity<IdentityUserClaim<int>>(e =>
            {
                e.HasData(new IdentityUserClaim<int>[] {
                    new IdentityUserClaim<int> { Id = -9902, UserId = -1, ClaimType = "app:role", ClaimValue = "DataGenie:admin" },
                    new IdentityUserClaim<int> { Id = -9904, UserId = -2, ClaimType = "app:role", ClaimValue = "DataGenie:admin" },
                    new IdentityUserClaim<int> { Id = -9905, UserId = -2, ClaimType = "app:role", ClaimValue = "InfoMaster:admin" },
                    new IdentityUserClaim<int> { Id = -9907, UserId = -3, ClaimType = "app:role", ClaimValue = "InfoMaster:admin" },
                    new IdentityUserClaim<int> { Id = -9908, UserId = -4, ClaimType = "app:role", ClaimValue = "DataGenie:user" },
                    new IdentityUserClaim<int> { Id = -9909, UserId = -5, ClaimType = "app:role", ClaimValue = "DataGenie:user" },
                    new IdentityUserClaim<int> { Id = -9910, UserId = -5, ClaimType = "app:role", ClaimValue = "InfoMaster:readonly" },
                    new IdentityUserClaim<int> { Id = -9911, UserId = -6, ClaimType = "app:role", ClaimValue = "InfoMaster:readonly" },
                    new IdentityUserClaim<int> { Id = -9912, UserId = -7, ClaimType = "app:role", ClaimValue = "DataGenie:user" },
                    new IdentityUserClaim<int> { Id = -9913, UserId = -8, ClaimType = "app:role", ClaimValue = "DataGenie:user" },
                    new IdentityUserClaim<int> { Id = -9914, UserId = -8, ClaimType = "app:role", ClaimValue = "InfoMaster:readonly" },
                    new IdentityUserClaim<int> { Id = -9915, UserId = -9, ClaimType = "app:role", ClaimValue = "InfoMaster:readonly" },
                    new IdentityUserClaim<int> { Id = -9916, UserId = -10, ClaimType = "app:role", ClaimValue = "InfoMaster:auditor" },
                    new IdentityUserClaim<int> { Id = -9917, UserId = -11, ClaimType = "*:role", ClaimValue = "*:admin" }
                });
            });


            }

        /// <summary>
        /// per https://stackoverflow.com/a/20622428
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private static string HashPassword(string password) {
            byte[] salt;
            byte[] buffer2;
            if (password == null) {
                throw new ArgumentNullException("password");
            }
            using (var bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8)) {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }



    }
}