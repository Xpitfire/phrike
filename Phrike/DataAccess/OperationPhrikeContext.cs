using DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OperationPhrikeContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public OperationPhrikeContext()
            :base("DefaultConnectionString")
        {
            Database.SetInitializer(new OperationPhrikeStrategy());
        }

        public DbSet<PositionData> PositionData { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyResult> SurveyResults { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<AuxilaryData> Videos { get; set; }

        
    }

    class OperationPhrikeStrategy : DropCreateDatabaseAlways<OperationPhrikeContext>
    {
        protected override void Seed(OperationPhrikeContext context)
        {
            context.Scenarios.Add(new Scenario()
            {
                Name = "Balance",
                ExecutionPath = "UnrealData/Balance/Balance.exe",
                Description = "Walk over a slackline between two canyon ledges",

                MinimapPath = "Balance/minimap.png",
                Version = "1.0",

                ZeroX = 1921,
                ZeroY = 257,
                Scale = 1354.0/24000.0
            });

#if DEBUG
            context.Subjects.Add(new Subject()
            {
                FirstName = "Marius",
                LastName = "Dinu",
                DateOfBirth = new DateTime(1988, 7, 3),
                CountryCode = "AT",
                Gender = Gender.Male,
                Residence = "Hagenberg",
                Function = "Developer"
            });
            context.Subjects.Add(new Subject()
            {
                FirstName = "Sandra",
                LastName = "Horner",
                DateOfBirth = new DateTime(1993, 9, 8),
                CountryCode = "AT",
                Gender = Gender.Female,
                Residence = "Hagenberg",
                Function = "Developer"
            });
            context.Subjects.Add(new Subject()
            {
                FirstName = "Wolfgang",
                LastName = "Mayr",
                DateOfBirth = new DateTime(1994, 9, 8),
                CountryCode = "AT",
                Gender = Gender.Male,
                Residence = "Hagenberg",
                Function = "Developer"
            });
            context.Subjects.Add(new Subject()
            {
                FirstName = "Alexander",
                LastName = "Steinke",
                DateOfBirth = new DateTime(1993, 7, 3),
                CountryCode = "AT",
                Gender = Gender.Male,
                Residence = "Linz",
                Function = "Developer"
            });
            context.Subjects.Add(new Subject()
            {
                FirstName = "Gurpreet",
                LastName = "Juttla",
                DateOfBirth = new DateTime(1989, 12, 7),
                CountryCode = "AT",
                Gender = Gender.Male,
                Residence = "Linz",
                Function = "Scrum Master"
            });
            context.Subjects.Add(new Subject()
            {
                FirstName = "Christiano",
                LastName = "Pham",
                DateOfBirth = new DateTime(1993, 5, 5),
                CountryCode = "AT",
                Gender = Gender.Male,
                Residence = "Wien",
                Function = "Developer",
                ServiceRank = "Junior",
            });
            context.Subjects.Add(new Subject()
            {
                FirstName = "Roland",
                LastName = "Kohlendorfer",
                DateOfBirth = new DateTime(1990, 6, 12),
                CountryCode = "AT",
                Gender = Gender.Male,
                Residence = "Klagenfurt",
                Function = "Developer"
            });
            context.Subjects.Add(new Subject()
            {
                FirstName = "Tom",
                LastName = "Neumair",
                DateOfBirth = new DateTime(1986, 2, 27),
                CountryCode = "AT",
                Gender = Gender.Male,
                Residence = "Hagenberg",
                Function = "Magician"
            });
            context.Subjects.Add(new Subject()
            {
                FirstName = "Josef",
                LastName = "Frauscher",
                DateOfBirth = new DateTime(1991, 4, 17),
                CountryCode = "CZ",
                Gender = Gender.Male,
                Residence = "Praha",
                Function = "Developer"
            });
            context.Subjects.Add(new Subject()
            {
                FirstName = "Christian",
                LastName = "Neumüller",
                DateOfBirth = new DateTime(1992, 10, 30),
                CountryCode = "AT",
                Gender = Gender.Male,
                Residence = "Hagenberg",
                Function = "Developer",
                ServiceRank = "Master of Desaster",
            });

            context.SaveChanges();
#endif
            base.Seed(context);
        }
    }
}
