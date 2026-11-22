using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Globalization;

namespace APP.Domain
{
    // Provides a factory for creating Db instances at design time (migrations, scaffolding)
    // and seeds the database tables with initial data.
    public class DbFactory : IDesignTimeDbContextFactory<Db>
    {
        const string CONNECTIONSTRING = "data source=DB.db";

        public Db CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Db>();
            optionsBuilder.UseSqlite(CONNECTIONSTRING);
            return new Db(optionsBuilder.Options);
        }

        public void SeedDb()
        {
            var db = CreateDbContext(null);

            // Delete existing data: relational (child) tables first, then main tables.
            db.DoctorPatients.RemoveRange(db.DoctorPatients.ToList());
            db.Patients.RemoveRange(db.Patients.ToList());
            db.Doctors.RemoveRange(db.Doctors.ToList());
            db.Branches.RemoveRange(db.Branches.ToList());
            db.UserRoles.RemoveRange(db.UserRoles.ToList());
            db.Roles.RemoveRange(db.Roles.ToList());
            db.Users.RemoveRange(db.Users.ToList());
            db.Statuses.RemoveRange(db.Statuses.ToList());
            db.SaveChanges();

            // Reset identity columns (for SQLite) so IDs start from 1 again.
            foreach (var table in new[] { "DoctorPatients", "Patients", "Doctors", "Branches",
                "UserRoles", "Roles", "Users", "Statuses" })
            {
                db.Database.ExecuteSql($"UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME={table}");
            }

            // Roles:
            db.Roles.Add(new Role { Name = "Admin" });
            db.Roles.Add(new Role { Name = "User" });
            db.SaveChanges();

            // Statuses with users:
            db.Statuses.Add(new Status
            {
                Title = "Active",
                Users = new List<User>
                {
                    new User
                    {
                        UserName = "admin",
                        Password = "admin",
                        RegistrationDate = DateTime.Parse("11/18/2026 10:00:00", new CultureInfo("en-US")),
                        UserRoles = new List<UserRole>
                        {
                            new UserRole { RoleId = db.Roles.Single(r => r.Name == "Admin").Id }
                        }
                    },
                    new User
                    {
                        UserName = "user",
                        Password = "user",
                        RegistrationDate = DateTime.Parse("11/18/2026 10:05:00", new CultureInfo("en-US")),
                        UserRoles = new List<UserRole>
                        {
                            new UserRole { RoleId = db.Roles.Single(r => r.Name == "User").Id }
                        }
                    }
                }
            });
            db.Statuses.Add(new Status { Title = "Inactive" });
            db.SaveChanges();

            // Branches with doctors:
            db.Branches.Add(new Branch
            {
                Title = "Cardiology",
                Doctors = new List<Doctor>
                {
                    new Doctor { FirstName = "Ayşe", LastName = "Kaya", IsExpert = true },
                    new Doctor { FirstName = "Mehmet", LastName = "Demir", IsExpert = false }
                }
            });
            db.Branches.Add(new Branch
            {
                Title = "Oncology",
                Doctors = new List<Doctor>
                {
                    new Doctor { FirstName = "Zeynep", LastName = "Arslan", IsExpert = true }
                }
            });
            db.Branches.Add(new Branch { Title = "Neurology" });
            db.SaveChanges();

            // Patients with doctors (many-to-many through DoctorIds):
            var kayaId = db.Doctors.Single(d => d.LastName == "Kaya").Id;
            var demirId = db.Doctors.Single(d => d.LastName == "Demir").Id;
            var arslanId = db.Doctors.Single(d => d.LastName == "Arslan").Id;
            db.Patients.Add(new Patient
            {
                FirstName = "Ali",
                LastName = "Yılmaz",
                Gender = Genders.Man,
                BirthDate = new DateTime(1968, 3, 12),
                Height = 176,
                Weight = 84.5m,
                UserId = db.Users.Single(u => u.UserName == "user").Id,
                DoctorIds = new List<int> { kayaId, arslanId }
            });
            db.Patients.Add(new Patient
            {
                FirstName = "Elif",
                LastName = "Şahin",
                Gender = Genders.Woman,
                BirthDate = new DateTime(1985, 9, 30),
                Height = 164,
                Weight = 58,
                DoctorIds = new List<int> { demirId }
            });
            db.Patients.Add(new Patient
            {
                FirstName = "Can",
                LastName = "Öztürk",
                Gender = Genders.Man,
                BirthDate = new DateTime(1952, 1, 5),
                DoctorIds = new List<int> { arslanId }
            });
            db.SaveChanges();
        }
    }
}