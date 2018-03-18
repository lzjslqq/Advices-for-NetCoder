using System;

namespace _6.创建对象时考虑是否实现比较器
{
    public class Salary : IComparable<Salary>
    {
        public string Name { get; set; }
        public int BaseSalary { get; set; }
        public int Bonus { get; set; }

        // 转型了
        //public int CompareTo(object obj)
        //{
        //    Salary staff = obj as Salary;
        //    if (BaseSalary > staff.BaseSalary)
        //        return 1;
        //    else if (BaseSalary == staff.BaseSalary)
        //        return 0;
        //    else
        //        return -1;

        //    // return BaseSalary.CompareTo(staff.BaseSalary);
        //}

        public int CompareTo(Salary other)
        {
            return BaseSalary.CompareTo(other.BaseSalary);
        }
    }
}