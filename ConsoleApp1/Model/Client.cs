using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Model
{
    public class Client(int id, string name, string secondName, string email)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string SecondName { get; set; } = secondName;
        public string Email { get; set; } = email;
    }
}
