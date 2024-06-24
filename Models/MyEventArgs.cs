using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerContainer.Models
{
    public class MyEventArgs:EventArgs
    {
        public string Name { get; set; }
        public string Subject { get; set; }

        public MyEventArgs(string name, string subject)
        {
            Name = name;
            Subject = subject;
        }
    }
}