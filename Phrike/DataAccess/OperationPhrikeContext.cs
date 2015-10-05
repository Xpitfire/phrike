using DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OperationPhrikeContext : DbContext
    {
        DbSet<PositionData> PositionData;
        DbSet<Survey> Surveys;
        DbSet<SurveyQuestion> SurveyQuestions;
        DbSet<SurveyResult> SurveyResults;
        DbSet<Test> Tests;
        DbSet<Propositus> Propositi;
        DbSet<Video> Videos;
        
        
        public Propositus GetDeineMudda ()
        {
            return (from m in this.Propositi
                    where m.LastName == "Steinke" && m.Sex == Sex.Female
                    select m).SingleOrDefault();
        }
    }
}
