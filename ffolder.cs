using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSort
{
    class ffolder
    {
        public ffolder(string name, string path)
        {
            Name = name;
            Path = path;
            TimeShift = TimeSpan.Zero;
        }

        public ffolder(string name, string path, TimeSpan timeShift)
        {
            Name = name;
            Path = path;
            TimeShift = timeShift;
        }

        public string Name { get; set; }

        public string Path { get; set; }

        public TimeSpan TimeShift { get; set; }

    }
        
}
