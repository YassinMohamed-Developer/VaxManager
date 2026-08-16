using System;
using System.Collections.Generic;
using System.Text;

namespace Vax.Service.Shared
{
	public static class ValidationError
	{
		public static class AuthError
		{
			public const string InvalidEmail =
						"This is Invalid Email ";

			public const string InvalidCredentials =
				"Invalid credentials for ";

			public const string EmailAlreadyExists =
				"This Email already exists";

			public const string UserNameAlreadyExists =
				"Username already exists.";

			public const string RegistrationSucceeded =
				"Registration completed successfully.";

			public const string PasswordResetFailed =
				"The operation could not be completed.";

			public const string PasswordChanged =
				"Your password has been changed.";

			public const string CheckYourEmail =
				"Please check your email.";

			public const string FailedToCreateUser =
				"Failed to create the user.";

			public const string InvalidToken =
				"Invalid token.";

			public const string GoogleAuthenticationFailed =
				"Google authentication failed.";

			public const string LoginSucceeded =
				"Login successful.";
		}

		public static class PatientError
		{
			public const string InvalidUser =
				"InValid User";

			public const string ProfileAlreadyComplete =
				"This Patient Is Profile is Complete";

			public const string PatientNotFound =
				"Patient is not Found";

			public const string NoPatientFound =
				"No Patient is Found";

			public const string ReservationsNotFound =
				"Reservations Not Found";

			public const string NoVaccineFound =
				"No Vaccine Found";

			public const string NoVaccineCenterFound =
				"No VaccineCenter Found";

			public const string CannotReserveSecondDoseFirst =
				"You Can Not Reserve Second Dose Before First One";

			public const string AlreadyTakenFirstDose =
				"You Already Take The First Dose ";

			public const string MustWaitBetweenDoses =
				"You Must Take Second Dose After {0} Days";

			public const string SecondDoseTaken =
				"You Take The Second Dose";

			public const string CannotReserveSecondDoseNotApproved =
				"Can Not Reserve Second Dose Before First One must be Accepted";

			public const string ReservationCanceled =
				"The Reservation is Canceled";

			public const string ProfileCompleted =
				"Profile Patient is Completed";

			public const string PatientDeletedSuccessfully =
				"This Patient is Deleted Successfully";

			public const string DataRetrieveSuccessfully =
				"Data Retrieve Successfully ";

			public const string PatientReservedSuccessfully =
				"Patient Reserve Successfully ";

			public const string DataUpdatedSuccessfully =
				"Data Is Updated Successfully ";
		}

		public static class VaccineCenterError
		{
			public const string InvalidUser =
				"Invalid User";

			public const string ProfileAlreadyComplete =
				"This VaccineCenter Is Profile is Complete";

			public const string VaccineCenterNotFound =
				"No VaccineCene Not Found";

			public const string VaccineCenterNotFoundAlt =
				"No VaccineCenter is Found";

			public const string VaccineNotFound =
				"No Vaccine is Found";

			public const string VaccinesNotFound =
				"No Vaccines is Found";

			public const string UnauthorizedAccess =
				"Unauthorized Access";

			public const string VaccineCenterNotRegistered =
				"This VaccineCenter Account Not Registered";

			public const string ReservationNotFound =
				"Reservation Not Found";

			public const string ReservationNotAccepted =
				"The Reservation Not Accepted Or This Vaccine Center Account Not Have Reservations";

			public const string ReservationNotRejected =
				"The Reservation Not Rejected Because It Is Accepted. Or This Vaccine Center Account Not Have Reservations";

			public const string VaccineCenterNotFoundAlt2 =
				"Vaccine Center is Not Found";

			public const string NoPatientsWithVaccines =
				"No Patients With Vaccines in this VaccineCenter..";

			public const string ProfileCompleted =
				"The Profile Is Completed";

			public const string VaccineCreated =
				"Vaccine Is Created";

			public const string ProfileDeleted =
				"Profile Is Deleted";

			public const string DataDeletedSuccessfully =
				"Data Is Deleted Successfully";

			public const string DataRetrieveSuccessfully =
				"Data Is Retrieve Successfully";

			public const string DataRetrieveSuccessfullyAlt =
				"Data Is Retrieve Successfully!";

			public const string DataUpdatedSuccessfully =
				"Data Is Updated Successfully ";

			public const string DataUpdatedSuccessfullyAlt =
				"Data Updated Successfully";
		}

		public static class AdminError
		{
			public const string AdminNotFound =
				"Admin Not Found";

			public const string NoPatientsAdded =
				"No Patients Added";

			public const string NoVaccineCenterAdded =
				"No Vaccine Center Is Added";

			public const string DataRetrieveSuccessfully =
				"Data Retrieve Successfully ";
		}
	}
}
