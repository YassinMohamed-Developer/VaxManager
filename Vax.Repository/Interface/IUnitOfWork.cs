using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Entity;

namespace Vax.Repository.Interface
{
	public interface IUnitOfWork
	{
		IGenericRepository<Patient> Patients { get; }

		IGenericRepository<Vaccine> Vaccines { get; }

		IGenericRepository<VaccineCenter> VaccinesCenter { get; }

		IGenericRepository<Reservation> Reservations { get; }

		IGenericRepository<Admin> Admins { get; }

		int Complete();
	}
}
