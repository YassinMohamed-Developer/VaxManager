using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Entity;

namespace Vax.Data.Context
{
	public class Vaxdbcontext : IdentityDbContext<AppUser>
	{
		public Vaxdbcontext(DbContextOptions<Vaxdbcontext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(builder);
		}

		public DbSet<AppUser> AppUsers { get; set; }

		public DbSet<Patient> Patients { get; set; }

		public DbSet<Vaccine> Vaccines { get; set; }

		public DbSet<VaccineCenter> VaccineCenters { get; set; }

		public DbSet<Reservation> Reservations { get; set; }

		public DbSet<Admin> Admins { get; set; }
	}
}
