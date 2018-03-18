using System.Collections.Generic;

namespace _6.创建对象时考虑是否实现比较器
{
    public class BonusComparer : IComparer<Salary>
    {
        public int Compare(Salary x, Salary y)
        {
            return x.Bonus.CompareTo(y.Bonus);
        }
    }
}