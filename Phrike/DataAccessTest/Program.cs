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

                for (int i = 0; i < 100; i++)
                {
                    Subject s = new Subject()
                    {
                        CountryCode = RandomString(rg, 2, 2).ToUpper(),
                        DateOfBirth = DateTime.Now,
                        FirstName = RandomString(rg, capital: true),
                        LastName = RandomString(rg, capital: true),
                        Function = RandomString(rg, capital:true),
                        Gender = rg.Next(0, 2) == 0 ? Gender.Male : Gender.Female,
                        Residence = RandomString(rg, capital: true),
                        ServiceRank = RandomString(rg, capital: true)
                    };

                    unitOfWork.SubjectRepository.Insert(s);

                }


                unitOfWork.Save();

            }

            //using (UnitOfWork unitOfWork = new UnitOfWork())
            //{
            //    Subject s = unitOfWork.SubjectRepository.Get().FirstOrDefault();
            //    if (s != null)
            //    {
            //        Console.WriteLine(s.FirstName + " " + s.LastName);
            //    }
            //}
        }

        static string RandomString(Random rg, int minLen = 2, int maxLen = 16, bool capital = false)
        {
            int length = rg.Next(minLen, maxLen);
            StringBuilder sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                sb.Append((char)(capital && i == 0 ? rg.Next('A', 'Z') : rg.Next('a', 'z')));
            }

            return sb.ToString();
        }
    }
}
