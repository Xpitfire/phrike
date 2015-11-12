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
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<OperationPhrikeContext>());
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
}
