using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sieve.Model;

namespace Sieve.Services
{
    /// <summary>
    /// Filter service interface.
    /// </summary>
    interface IFilterService
    {
        /// <summary>
        /// Filters todolist from for query.
        /// </summary>
        /// <param name="originalList">List which we want to filter.</param>
        /// <param name="query">Query.</param>
        /// <returns>Filtered list.</returns>
        List<TodoTask> Calculate(List<TodoTask> originalList, string query);
    }
}
