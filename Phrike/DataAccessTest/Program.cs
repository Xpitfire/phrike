using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace DataAccessTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Random rg = new Random(1);

                for (int i = 0; i < 10; i++)
                {
                    Test t = new Test()
                    {
                        Time = DateTime.Now
                    };
                    t.Subject = new Subject()
                    {
                        DateOfBirth = DateTime.Now.AddYears(rg.Next(10, 30) * -1),
                        FirstName = RandomString(rg),
                        LastName = RandomString(rg),
                        Gender = (Gender)rg.Next(0, 2)
                    };
                    t.Scenario = new Scenario
                    {
                        Name = RandomString(rg)
                    };
                    unitOfWork.TestRepository.Insert(t);
                    unitOfWork.Save();

                    for (int j = 0; j < 100; j++)
                    {
                        t.PositionData.Add(new PositionData()
                        {
                            Time = DateTime.Now,
                            X = rg.Next(),
                            Y = rg.Next(),
                            Z = rg.Next(),
                            Pitch = (float)rg.NextDouble(),
                            Roll = (float)rg.NextDouble(),
                            Yaw = (float)rg.NextDouble()
                        });
                    }
                }
                
                unitOfWork.Save();
                
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Subject s = unitOfWork.SubjectRepository.Get().FirstOrDefault();
                if (s != null)
                {
                    Console.WriteLine(s.FirstName + " " + s.LastName);
                }
            }
        }

        static string RandomString(Random rg, int minLen = 2, int maxLen = 16)
        {
            int length = rg.Next(minLen, maxLen);
            StringBuilder sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                sb.Append((char) rg.Next('a', 'z'));
            }

            return sb.ToString();
        }
    }
}
