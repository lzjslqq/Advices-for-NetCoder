using System;

namespace _5.习惯运算符重载
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // 在构建自己的类型时，应考虑类型是否可以用于运算符重载，使开发时更具语义化。
            Salary mikeIncome = new Salary { RMB = 100 };
            Salary johnIncome = new Salary { RMB = 300 };
            Salary allIncome = mikeIncome + johnIncome;

            Console.WriteLine(allIncome);
            Console.Read();
        }
    }

    internal class Salary
    {
        public int RMB { get; set; }

        public static Salary operator +(Salary s1, Salary s2)
        {
            s1.RMB += s2.RMB;
            return s1;
        }

        public override string ToString()
        {
            return string.Format("收入是{0}", RMB);
        }
    }
}