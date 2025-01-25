using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Context;
using Vax.Data.Entity;
using Vax.Repository.Interface;

namespace Vax.Repository.Implmentation
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly Vaxdbcontext _context;

        public IGenericRepository<Patient> Patients {  get; private set; }

		public IGenericRepository<Vaccine> Vaccines { get; private set; }

		public IGenericRepository<VaccineCenter> VaccinesCenter { get; private set; }

		public IGenericRepository<Reservation> Reservations { get; private set; }

		public IGenericRepository<Admin> Admins { get; private set; }
		public UnitOfWork(Vaxdbcontext context)
        {
			_context = context;
			Patients = new GenericRepository<Patient>(context);
			Vaccines = new GenericRepository<Vaccine>(context);
			VaccinesCenter = new GenericRepository<VaccineCenter>(context);
			Reservations = new GenericRepository<Reservation>(context);
			Admins = new GenericRepository<Admin>(context);
		}

		public int Complete()
		{
			return _context.SaveChanges();
		}
	}
}
