using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace _7.正确实现浅拷贝和深拷贝
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Employee mike = new Employee
            {
                Age = 18,
                IDCode = "NB123456",
                Department = new Department { Name = "Dep1" }
            };

            var rose = mike.DeepClone() as Employee;
            Console.WriteLine(rose.Age);
            Console.WriteLine(rose.IDCode);
            Console.WriteLine(rose.Department);
            Console.WriteLine("开始改变mike的值");
            mike.IDCode = "NB456789";
            mike.Age = 25;
            mike.Department.Name = "newDepartment";
            Console.WriteLine(mike.Department);
            Console.WriteLine(rose.Age);
            Console.WriteLine(rose.IDCode);
            // department是引用类型，故改变源对象的值也会引起rose的值改变
            Console.WriteLine(rose.Department);

            Console.Read();
        }
    }

    [Serializable]
    internal class Employee : ICloneable
    {
        public string IDCode { get; set; }
        public int Age { get; set; }
        public Department Department { get; set; }

        public object Clone()
        {
            // 简单的浅拷贝
            return this.MemberwiseClone();
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        public Employee DeepClone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as Employee;
            }
        }

        /// <summary>
        /// 浅拷贝
        /// </summary>
        /// <returns></returns>
        public Employee ShallowClone()
        {
            return Clone() as Employee;
        }
    }

    [Serializable]
    internal class Department
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}