using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sieve.Model
{
    public class TodoTask
    {
        public string Content { get; set; }

        public DateTime DueDate { get; set; }

        public Priority Priority { get; set; }
    }
}
