using System;
using System.Reflection.Metadata;
using KangarooParty.Models;
using Microsoft.EntityFrameworkCore;
namespace KangarooParty.Data
{
	public class ApplicationDbContext: DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Party>()
                .HasOne(e => e.Host)
                .WithOne(e => e.HostingParty)
                .HasForeignKey<Kangaroo>(e => e.HostingPartyId)
                .IsRequired();

            modelBuilder.Entity<Party>()
                .HasMany(e => e.Attendees)
                .WithOne(e => e.AttendingParty)
                .HasForeignKey(e => e.AttendingPartyId)
                .IsRequired(false);
        }

        public DbSet<Party> Parties { get; set; }
		public DbSet<Kangaroo> Kangaroos  { get; set; }

    }
}

