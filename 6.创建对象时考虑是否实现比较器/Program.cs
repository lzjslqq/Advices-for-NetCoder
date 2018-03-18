using System;
using System.Collections.Generic;

namespace _6.创建对象时考虑是否实现比较器
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<Salary> companySalary = new List<Salary>();
            companySalary.Add(new Salary { Name = "Mike", BaseSalary = 3000, Bonus = 1000 });
            companySalary.Add(new Salary { Name = "Joe", BaseSalary = 2000, Bonus = 4000 });
            companySalary.Add(new Salary { Name = "Steve", BaseSalary = 1000, Bonus = 6000 });
            companySalary.Add(new Salary { Name = "Jeff", BaseSalary = 4000, Bonus = 3000 });
            companySalary.Sort();
            foreach (Salary item in companySalary)
            {
                Console.WriteLine("{0}'s BaseSalary is {1},Bonus is {2}", item.Name, item.BaseSalary, item.Bonus);
            }

            Console.WriteLine("自定义比较器：");
            companySalary.Sort(new BonusComparer()); // 提供一个非默认的比较器
            foreach (Salary item in companySalary)
            {
                Console.WriteLine("{0}'s BaseSalary is {1},Bonus is {2}", item.Name, item.BaseSalary, item.Bonus);
            }

            Console.Read();
        }
    }
}