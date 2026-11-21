using APP.Domain;
using APP.Models;
using CORE.Models;
using CORE.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class DoctorService : DbService<Doctor>, IService<DoctorRequest, DoctorResponse>
    {
        public DoctorService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Doctor> DbQuery()
        {
            return base.DbQuery()
                .Include(d => d.Branch)
                .Include(d => d.User)
                .Include(d => d.DoctorPatients)
                .ThenInclude(dp => dp.Patient)
                .OrderBy(d => d.Branch.Title)
                .ThenBy(d => d.LastName)
                .ThenBy(d => d.FirstName);
        }

        public IQueryable<DoctorResponse> Query()
        {
            return DbQuery().Select(d => new DoctorResponse
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                IsExpert = d.IsExpert,
                BranchId = d.BranchId,
                UserId = d.UserId,
                FullNameR = "Dr. " + d.FirstName + " " + d.LastName,
                IsExpertR = d.IsExpert ? "Yes" : "No",
                BranchTitleR = d.Branch.Title,
                UserNameR = d.User != null ? d.User.UserName : string.Empty,
                PatientCountR = d.DoctorPatients.Count,
                PatientNamesR = string.Join("<br>", d.DoctorPatients
                    .OrderBy(dp => dp.Patient.LastName)
                    .Select(dp => dp.Patient.FirstName + " " + dp.Patient.LastName))
            });
        }

        public DoctorRequest Edit(int id)
        {
            var doctorEntity = DbSingle(id);
            if (doctorEntity is null)
                return null;
            return new DoctorRequest
            {
                Id = doctorEntity.Id,
                FirstName = doctorEntity.FirstName,
                LastName = doctorEntity.LastName,
                IsExpert = doctorEntity.IsExpert,
                BranchId = doctorEntity.BranchId,
                UserId = doctorEntity.UserId
            };
        }

        public CommandResponse Add(DoctorRequest request)
        {
            if (DbQuery().Any(d => d.FirstName == request.FirstName.Trim()
                && d.LastName == request.LastName.Trim()
                && d.BranchId == request.BranchId))
                return Error("Doctor with the same name exists in this branch!");
            var doctorEntity = new Doctor
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                IsExpert = request.IsExpert,
                BranchId = request.BranchId ?? 0,
                UserId = request.UserId
            };
            DbAdd(doctorEntity);
            return Success(doctorEntity.Id, "Doctor created successfully.");
        }

        public CommandResponse Update(DoctorRequest request)
        {
            if (DbQuery().Any(d => d.Id != request.Id
                && d.FirstName == request.FirstName.Trim()
                && d.LastName == request.LastName.Trim()
                && d.BranchId == request.BranchId))
                return Error("Doctor with the same name exists in this branch!");
            var doctorEntity = DbSingle(request.Id);
            if (doctorEntity is null)
                return Error("Doctor not found!");
            doctorEntity.FirstName = request.FirstName.Trim();
            doctorEntity.LastName = request.LastName.Trim();
            doctorEntity.IsExpert = request.IsExpert;
            doctorEntity.BranchId = request.BranchId ?? doctorEntity.BranchId;
            doctorEntity.UserId = request.UserId;
            DbUpdate(doctorEntity);
            return Success(doctorEntity.Id, "Doctor updated successfully.");
        }

        // The doctor's relational DoctorPatient records are deleted first,
        // then the doctor (same as the user-roles case in the guide).
        public CommandResponse Remove(int id)
        {
            var doctorEntity = DbSingle(id);
            if (doctorEntity is null)
                return Error("Doctor not found!");
            DbRemove(doctorEntity.DoctorPatients);
            DbRemove(doctorEntity);
            return Success(doctorEntity.Id, "Doctor deleted successfully.");
        }
    }
}