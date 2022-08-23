using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easycmd
{
    internal class Cmd
    {
        public string Command { get; set; }
        public string Name { get; set; }

        public Cmd(string command, string name)
        {
            Command = command;
            Name = name;
        }
    }
}
