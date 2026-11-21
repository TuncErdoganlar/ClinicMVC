using APP.Domain;
using APP.Models;
using CORE.Models;
using CORE.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class BranchService : DbService<Branch>, IService<BranchRequest, BranchResponse>
    {
        public BranchService(DbContext db) : base(db)
        {
        }

        // Includes the doctors of each branch so they can be counted, listed
        // and checked before deleting a branch.
        protected override IQueryable<Branch> DbQuery()
        {
            return base.DbQuery()
                .Include(b => b.Doctors)
                .OrderBy(b => b.Title);
        }

        public IQueryable<BranchResponse> Query()
        {
            return DbQuery().Select(b => new BranchResponse
            {
                Id = b.Id,
                Title = b.Title,
                DoctorCountR = b.Doctors.Count,
                DoctorNamesR = string.Join(", ", b.Doctors
                    .OrderBy(d => d.LastName)
                    .Select(d => d.FirstName + " " + d.LastName))
            });
        }

        public BranchRequest Edit(int id)
        {
            var branchEntity = DbSingle(id);
            if (branchEntity is null)
                return null;
            return new BranchRequest
            {
                Id = branchEntity.Id,
                Title = branchEntity.Title
            };
        }

        public CommandResponse Add(BranchRequest request)
        {
            if (DbQuery().Any(b => b.Title == request.Title.Trim()))
                return Error("Branch with the same title exists!");
            var branchEntity = new Branch
            {
                Title = request.Title.Trim()
            };
            DbAdd(branchEntity);
            return Success(branchEntity.Id, "Branch created successfully.");
        }

        public CommandResponse Update(BranchRequest request)
        {
            if (DbQuery().Any(b => b.Id != request.Id && b.Title == request.Title.Trim()))
                return Error("Branch with the same title exists!");
            var branchEntity = DbSingle(request.Id);
            if (branchEntity is null)
                return Error("Branch not found!");
            branchEntity.Title = request.Title.Trim();
            DbUpdate(branchEntity);
            return Success(branchEntity.Id, "Branch updated successfully.");
        }

        // A branch that still has doctors can't be deleted.
        public CommandResponse Remove(int id)
        {
            var branchEntity = DbSingle(id);
            if (branchEntity is null)
                return Error("Branch not found!");
            if (branchEntity.Doctors.Any())
                return Error("Branch has relational doctors!");
            DbRemove(branchEntity);
            return Success(branchEntity.Id, "Branch deleted successfully.");
        }
    }
}