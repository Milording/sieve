using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sieve.Model;

namespace Sieve.Services
{
    /// <summary>
    /// Filter service.
    /// </summary>
    class FilterService:IFilterService
    {

        #region Public methods

        /// <summary>
        /// Filters todolist from for query.
        /// </summary>
        /// <param name="originalList">List which we want to filter.</param>
        /// <param name="query">Query.</param>
        /// <returns>Filtered list.</returns>
        public List<TodoTask> Calculate(List<TodoTask> originalList, string query)
        {
            this.originalTodoTasks = new ObservableCollection<TodoTask>(originalList);
            this.filteredList = new ObservableCollection<TodoTask>();

            query = query.Replace("(", " ( ");
            query = query.Replace(")", " ) ");
            query = query.Replace("&", " & ");
            query = query.Replace("|", " | ");

            var queryList = query.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var rpn = GetRpn(queryList);
            CalculateRpn(rpn.ToList());

            return this.filteredList.ToList();
        }
        
        #endregion

        #region Private methods

        /// <summary>
        /// Returns a Reverse Polish notation of query.
        /// </summary>
        private Queue<string> GetRpn(List<string> queryList)
        {
            var output = new Queue<string>();
            var stack = new Stack<string>();

            foreach (var query in queryList)
            {
                if (query != "(" && query != ")" && query != "|" && query != "&")
                {
                    output.Enqueue(query);
                }
                else
                {
                    if (stack.Count == 0)
                        stack.Push(query);
                    else
                    {
                        if (query == ")")
                        {
                            while (stack.Peek() != "(")
                                output.Enqueue(stack.Pop());
                            stack.Pop();
                        }
                        else
                            stack.Push(query);
                    }
                }
            }

            foreach (var s in stack)
            {
                output.Enqueue(s);
            }

            return output;
        }

        /// <summary>
        /// Calculates RPN
        /// </summary>
        /// <param name="prn">RPN</param>
        private void CalculateRpn(List<string> prn)
        {
            bool isFirst = true;

            if (prn.Count == 1)
            {
                OrFilter(prn[0]);
                return;
            }

            while (prn.Count > 0)
            {
                if (prn.Count == 1 && (prn[0] == "|" || prn[0] == "&"))
                    return;

                var andIndex = prn.IndexOf("&");
                var orIndex = prn.IndexOf("|");

                if (andIndex == 1)
                {
                    AndFilter(prn[0]);

                    prn.RemoveRange(0, 2);
                    continue;
                }
                if (orIndex == 1)
                {
                    OrFilter(prn[0]);

                    prn.RemoveRange(0, 2);
                    continue;
                }

                for (int i = 0; i < prn.Count; i++)
                {
                    if (prn[i] == "|")
                    {
                        OrFilter(prn[i - 1]);
                        OrFilter(prn[i - 2]);

                        prn.RemoveRange(i - 2, 3);
                    }
                    else if (prn[i] == "&")
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                            this.filteredList = this.originalTodoTasks;
                        }

                        AndFilter(prn[i - 1]);
                        AndFilter(prn[i - 2]);

                        prn.RemoveRange(i - 2, 3);
                    }
                }
            }
        }

        /// <summary>
        /// Filter for AND operation.
        /// Filters already filtered list.
        /// </summary>
        private void AndFilter(string query)
        {
            var dateRegex = new Regex(@"\d{4}\-\d{2}-\d{2}");
            var priorityRegex = new Regex(@"p[1-4]");
            var matchDate = dateRegex.Match(query);
            var matchPriority = priorityRegex.Match(query);
            if (matchDate.Success)
            {
                var date = DateTime.Parse(query);
                this.filteredList =
                    new ObservableCollection<TodoTask>(
                        this.filteredList.Where(
                            s =>
                                s.DueDate.Year == date.Year && s.DueDate.Month == date.Month &&
                                s.DueDate.Day == date.Day));
            }
            else if (matchPriority.Success)
            {
                this.filteredList =
                    new ObservableCollection<TodoTask>(
                        this.filteredList.Where(s => s.Priority == (Priority)(int)Char.GetNumericValue(query[1])));
            }
            else
            {
                this.filteredList =
                    new ObservableCollection<TodoTask>(this.filteredList.Where(s => s.Content.ToLower().Contains(query)));
            }
        }

        /// <summary>
        /// Filter for OR opeartion.
        /// Filters throught adding from original task list.
        /// </summary>
        private void OrFilter(string query)
        {
            var dateRegex = new Regex(@"\d{4}\-\d{2}-\d{2}");
            var priorityRegex = new Regex(@"p[1-4]");
            var matchDate = dateRegex.Match(query);
            var matchPriority = priorityRegex.Match(query);
            if (matchDate.Success)
            {
                var date = DateTime.Parse(query);
                var orList =
                    new ObservableCollection<TodoTask>(
                        this.originalTodoTasks.Where(
                            s =>
                                s.DueDate.Year == date.Year && s.DueDate.Month == date.Month &&
                                s.DueDate.Day == date.Day));

                foreach (var todoTask in orList)
                {
                    this.filteredList.Add(todoTask);
                }
            }
            else if (matchPriority.Success)
            {
                var orList =
                    new ObservableCollection<TodoTask>(
                        this.originalTodoTasks.Where(s => s.Priority == (Priority)(int)Char.GetNumericValue(query[1])));

                foreach (var todoTask in orList)
                {
                    this.filteredList.Add(todoTask);
                }
            }
            else
            {
                var orList =
                    new ObservableCollection<TodoTask>(
                        this.originalTodoTasks.Where(s => s.Content.ToLower().Contains(query)));

                foreach (var todoTask in orList)
                {
                    this.filteredList.Add(todoTask);
                }
            }

            this.filteredList = new ObservableCollection<TodoTask>(this.filteredList.Distinct());
        }

        #endregion

        #region Private fields

        /// <summary>
        /// Filtered list of tasks.
        /// </summary>
        private ObservableCollection<TodoTask> filteredList;

        /// <summary>
        /// Original todolist list which generates for testing
        /// </summary>
        private ObservableCollection<TodoTask> originalTodoTasks = new ObservableCollection<TodoTask>();


        #endregion
    }
}
