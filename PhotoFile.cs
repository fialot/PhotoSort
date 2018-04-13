using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSort
{
    class PhotoFile
    {
        public PhotoFile(string path, string folder, TimeSpan timeShift)
        {
            Folder = folder;
            Path = path;
            TimeShift = TimeSpan.Zero;
            Date = DateTime.MinValue;
            RewriteExif = false;
        }

        public PhotoFile(string path, string folder, TimeSpan timeShift, DateTime date, bool rewrite)
        {
            Folder = folder;
            Path = path;
            TimeShift = timeShift;
            Date = date;
            RewriteExif = rewrite;
        }

        public string Folder { get; set; }

        public string Path { get; set; }

        public TimeSpan TimeShift { get; set; }

        public DateTime Date { get; set; }

        public bool RewriteExif { get; set; }
    }
}
