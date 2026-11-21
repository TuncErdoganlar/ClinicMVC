using APP.Domain;
using APP.Models;
using CORE.Models;
using CORE.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class PatientService : DbService<Patient>, IService<PatientRequest, PatientResponse>
    {
        public PatientService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Patient> DbQuery()
        {
            return base.DbQuery()
                .Include(p => p.User)
                .Include(p => p.DoctorPatients)
                .ThenInclude(dp => dp.Doctor)
                .ThenInclude(d => d.Branch)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName);
        }

        public IQueryable<PatientResponse> Query()
        {
            return DbQuery().Select(p => new PatientResponse
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Gender = p.Gender,
                BirthDate = p.BirthDate,
                Height = p.Height,
                Weight = p.Weight,
                UserId = p.UserId,
                FullNameR = p.FirstName + " " + p.LastName,
                GenderR = p.Gender.ToString(),
                BirthDateR = p.BirthDate.ToShortDateString(),
                AgeR = CalculateAge(p.BirthDate),
                HeightR = p.Height.HasValue ? p.Height.Value.ToString("N1") : string.Empty,
                WeightR = p.Weight.HasValue ? p.Weight.Value.ToString("N1") : string.Empty,
                Bmi = CalculateBmi(p.Height, p.Weight),
                BmiR = FormatBmi(CalculateBmi(p.Height, p.Weight)),
                UserNameR = p.User != null ? p.User.UserName : string.Empty,
                DoctorNamesR = string.Join("<br>", p.DoctorPatients
                    .OrderBy(dp => dp.Doctor.LastName)
                    .Select(dp => "Dr. " + dp.Doctor.FirstName + " " + dp.Doctor.LastName
                        + " (" + dp.Doctor.Branch.Title + ")")),
                DoctorsR = p.DoctorPatients.OrderBy(dp => dp.Doctor.LastName)
                    .Select(dp => new DoctorResponse
                    {
                        Id = dp.Doctor.Id,
                        FullNameR = "Dr. " + dp.Doctor.FirstName + " " + dp.Doctor.LastName,
                        BranchTitleR = dp.Doctor.Branch.Title
                    }).ToList()
            });
        }

        public PatientRequest Edit(int id)
        {
            var patientEntity = DbSingle(id);
            if (patientEntity is null)
                return null;
            return new PatientRequest
            {
                Id = patientEntity.Id,
                FirstName = patientEntity.FirstName,
                LastName = patientEntity.LastName,
                Gender = patientEntity.Gender,
                BirthDate = patientEntity.BirthDate,
                Height = patientEntity.Height,
                Weight = patientEntity.Weight,
                UserId = patientEntity.UserId,
                DoctorIds = patientEntity.DoctorIds
            };
        }

        public CommandResponse Add(PatientRequest request)
        {
            if (DbQuery().Any(p => p.FirstName == request.FirstName.Trim()
                && p.LastName == request.LastName.Trim()
                && p.BirthDate == request.BirthDate))
                return Error("Patient with the same name and birth date exists!");
            var patientEntity = new Patient
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Gender = request.Gender,
                BirthDate = request.BirthDate ?? DateTime.Today,
                Height = request.Height,
                Weight = request.Weight,
                UserId = request.UserId,
                DoctorIds = request.DoctorIds ?? new List<int>()
            };
            DbAdd(patientEntity);
            return Success(patientEntity.Id, "Patient created successfully.");
        }

        public CommandResponse Update(PatientRequest request)
        {
            if (DbQuery().Any(p => p.Id != request.Id
                && p.FirstName == request.FirstName.Trim()
                && p.LastName == request.LastName.Trim()
                && p.BirthDate == request.BirthDate))
                return Error("Patient with the same name and birth date exists!");
            var patientEntity = DbSingle(request.Id);
            if (patientEntity is null)
                return Error("Patient not found!");

            // Delete the old doctor relations, then assign the selected doctors.
            // If no doctor is selected, the patient's doctor list is cleared.
            DbRemove(patientEntity.DoctorPatients);

            patientEntity.FirstName = request.FirstName.Trim();
            patientEntity.LastName = request.LastName.Trim();
            patientEntity.Gender = request.Gender;
            patientEntity.BirthDate = request.BirthDate ?? patientEntity.BirthDate;
            patientEntity.Height = request.Height;
            patientEntity.Weight = request.Weight;
            patientEntity.UserId = request.UserId;
            patientEntity.DoctorIds = request.DoctorIds ?? new List<int>();
            DbUpdate(patientEntity);
            return Success(patientEntity.Id, "Patient updated successfully.");
        }

        public CommandResponse Remove(int id)
        {
            var patientEntity = DbSingle(id);
            if (patientEntity is null)
                return Error("Patient not found!");
            DbRemove(patientEntity.DoctorPatients);
            DbRemove(patientEntity);
            return Success(patientEntity.Id, "Patient deleted successfully.");
        }

        // Static helper methods. Static methods can be used in the LINQ Select projection above:
        // Entity Framework runs them in C# after getting the data from the database.
        public static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
                age--;
            return age;
        }

        public static decimal? CalculateBmi(decimal? height, decimal? weight)
        {
            if (!height.HasValue || !weight.HasValue || height.Value <= 0)
                return null;
            var heightInMeters = height.Value / 100;
            return Math.Round(weight.Value / (heightInMeters * heightInMeters), 1);
        }

        public static string FormatBmi(decimal? bmi)
        {
            if (!bmi.HasValue)
                return string.Empty;
            var category = bmi < 18.5m ? "Underweight"
                : bmi < 25m ? "Normal"
                : bmi < 30m ? "Overweight"
                : "Obese";
            return bmi.Value.ToString("N1") + " (" + category + ")";
        }
    }
}