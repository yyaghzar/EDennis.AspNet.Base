﻿using EDennis.AspNet.Base;
using EDennis.AspNet.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Hr.PersonApi.Models {

    /// <summary>
    /// Facilitate database migrations by using a design time factory that references
    /// EDennis.MigrationsExtensions
    /// </summary>
    public class HrContextDesignTimeFactory : SqlServerDbContextDesignTimeFactory<HrContext> { }

    public class HrContext : DbContext {
        public HrContext(DbContextOptions<HrContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder
                .ConfigureTemporalEntity<Person>(p => p.Id, true, true)
                .ConfigureTemporalEntity<Address>(a => a.Id, true, true)
                .ConfigureTemporalEntity<State>(s => s.Code, false, true);

            modelBuilder.Entity<Address>(a =>
                a.HasOne(a=>a.Person)
                .WithMany(p=>p.Addresses)
                .HasForeignKey(a=>a.PersonId)
                .OnDelete(DeleteBehavior.ClientCascade)
            );

        }
    }
}
