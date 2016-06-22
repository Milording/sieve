using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Sieve.Model;
using Sieve.Services;
using Sieve.Views;

namespace Sieve.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="MainPage"/>
    /// </summary>
    class MainPageViewModel:ViewModelBase
    {

        #region Services
        
        private INavigationService navigationService;
        private IFilterService filterService;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="MainPageViewModel"/>
        /// </summary>
        public MainPageViewModel(INavigationService navigationService,IFilterService filterService)
        {
            this.navigationService = navigationService;
            this.filterService = filterService;

            this.TaskList = new ObservableCollection<TodoTask>();

            this.SortSelectionChangedCommand = new DelegateCommand<ComboBox>(this.OnSortSelectionChangedCommand);
            this.FilterCommand = new DelegateCommand<string>(this.OnFilterCommand);

            Filling();
        }

        #endregion

        #region Bindable bindings

        private ObservableCollection<TodoTask> taskList;

        /// <summary>
        /// Gets or sets list of tasks.
        /// </summary>
        public ObservableCollection<TodoTask> TaskList
        {
            get { return this.taskList; }
            set { this.SetProperty(ref this.taskList, value); }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Invokes when user wants to change sort of list.
        /// </summary>
        public DelegateCommand<ComboBox> SortSelectionChangedCommand { get; private set; }

        /// <summary>
        /// Invokes when user wants to filter list of tasks.
        /// </summary>
        public DelegateCommand<string> FilterCommand { get; private set; }

        #endregion

        #region Command handlers

        /// <summary>
        /// Invokes when sort command is called.
        /// </summary>
        public void OnSortSelectionChangedCommand(ComboBox args)
        {
            switch (args.SelectedIndex)
            {
                case 0:
                    this.TaskList = new ObservableCollection<TodoTask>(this.TaskList.OrderBy(s => s.Content));
                    break;
                case 1:
                    this.TaskList = new ObservableCollection<TodoTask>(this.TaskList.OrderBy(s => s.DueDate));
                    break;
                case 2:
                    this.TaskList = new ObservableCollection<TodoTask>(this.TaskList.OrderBy(s=>s.Priority));
                    break;
            }
        }

        /// <summary>
        /// Invokes when filter command is called.
        /// </summary>
        public void OnFilterCommand(string query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                this.TaskList = this.originalTodoTasks;
                return;
            }
            
            this.TaskList = new ObservableCollection<TodoTask>(this.filterService.Calculate(this.originalTodoTasks.ToList(),query));
        }

        #endregion

        #region Private methods 


        /// <summary>
        /// Filling random tasks.
        /// </summary>
        private async void Filling()
        {
            for (var i = 0; i < 10; i++)
            {
                var task = await CreateRandomTask();

                this.TaskList.Add(task);
                this.originalTodoTasks.Add(task);
            }
            
        }

        /// <summary>
        /// Create a random task.
        /// </summary>
        private async Task<TodoTask> CreateRandomTask()
        {
            var rand = new Random();
            var values = Enum.GetValues(typeof(Priority));

            var task = new TodoTask();
            task.Content = words[rand.Next(0, words.Length - 1)];
            await Task.Delay(5);
            task.DueDate = DateTime.Now.AddDays(rand.Next(2, 10));
            await Task.Delay(5);
            task.Priority = (Priority) values.GetValue(rand.Next(values.Length));
            
            return task;
        }

        #endregion

        #region Private fields

        /// <summary>
        /// Words for random generation.
        /// </summary>
        private string[] words = {"Stroke a cat", "Clean the paper", "Buy a stone", "Smell an air"};

        /// <summary>
        /// Original todolist list which generates for testing
        /// </summary>
        private ObservableCollection<TodoTask> originalTodoTasks = new ObservableCollection<TodoTask>();
        
        
        #endregion

    }
}
