using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sieve.Model
{
    /// <summary>
    /// TodoTask model.
    /// </summary>
    public class TodoTask
    {
        /// <summary>
        /// Content of task.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Due date of task.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Priority of task.
        /// </summary>
        public Priority Priority { get; set; }
    }
}
