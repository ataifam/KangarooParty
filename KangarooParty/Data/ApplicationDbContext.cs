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
            //kangaroo's party deletes on kangaroo deletion
            modelBuilder.Entity<Kangaroo>()
                .HasOne(e => e.HostingParty)
                .WithOne(e => e.Host)
                .HasForeignKey<Party>(e => e.HostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            //kangaroo's attending party set to null on party deletion
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

