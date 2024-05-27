namespace TaskDisplayer
{
    enum TaskPriority
    {
        Low,
        Medium,
        High
    }
    class Task
    {
        public string Description
        {get; set;}
        public TaskPriority Priority 
        {get; set;}
        public DateTime DueDate
        {get; set;}
        public bool IsHighlighted
        {get; set;}

        // Constructor task initialisation
        public Task(string description, TaskPriority priority, DateTime dueDate, bool isHighlighted)
        {
            if (dueDate < DateTime.Now)
            {
                throw new ArgumentException("Due date cannot be in the past.");
            }
            Description = description;
            Priority = priority;
            DueDate = dueDate;
            IsHighlighted = isHighlighted;
        }

        // Printing the task description
        public override string ToString()
        {
            return $"{Description} --- {Priority}  --- {DueDate:dd-MM-yyyy}";
        }
    }

    class TaskCategory
    {
        // Properties
        public string Name{get; }
        public List<Task> Tasks{get;}
        // Constructor
        public TaskCategory(string name)
        {
            Name = name;
            Tasks = new List<Task>();
        }

        // Add new tasks
        public void AddTask(string description, TaskPriority priority, DateTime dueDate, bool isHighlighted)
        {
            var task = new Task(description, priority, dueDate, isHighlighted);
            Tasks.Add(task);
        }

        // Delete tasks
        public void RemoveTask(int taskIndex)
        {
            if (taskIndex >= 0 && taskIndex < Tasks.Count)
            {
                var removedTask = Tasks[taskIndex];
                Tasks.RemoveAt(taskIndex);
                Console.WriteLine($"Deleted task: {removedTask}");
            }
            else
            {
                Console.WriteLine("Invalid task index. Task not deleted.");
            }
        }
        // CHANGES TASK PRIORITY
        public void ChangeTaskPriority(int taskIndex, TaskPriority newPriority)
        {
            if (taskIndex >= 0 && taskIndex < Tasks.Count)
            {
                var task = Tasks[taskIndex];
                task.Priority=newPriority;
            }
            else
            {
                Console.WriteLine("Invalid task index. Task priority not changed.");
            }
        }

        // Highlight important task in red
        public void TaskHighlightedInRed(int taskIndex)
        {
            if (taskIndex>=0 && taskIndex<Tasks.Count)
            {
                var task = Tasks[taskIndex];
                task.IsHighlighted = !task.IsHighlighted;
            }
            else
            {
                Console.WriteLine("Invalid task index. Task highlight not changed");
            }
        }

        // Change the importance of the tasks within 1 category
        public void ChangeImportance(int currentIndex, int newIndex)
        {
            Task task = Tasks[currentIndex];
            Tasks.Remove(task);
            Tasks.Insert(newIndex, task);
        }
    }

    class TaskManager
    {
        static private Dictionary<string, TaskCategory> categories;
        // Constructor initialised default planner stuff
        public TaskManager()
        {
            categories= new Dictionary<string, TaskCategory>()
            {
                ["personal"] = new TaskCategory("personal"),
                ["family"] = new TaskCategory("family"),
                ["work"] = new TaskCategory("work")
            };
        }

        public static string ReadString(string prompt)
        {
            Console.WriteLine(prompt);
            Console.Write(">> ");
            return Console.ReadLine() ?? "";
        }


        // Prints out the tasks
        public void PrintTask()
        {
            Console.Clear();
            int max = MaximumLength();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(new string(' ', 12) + "CATEGORIES");

            string formattingLine = new string(' ', 12);

            foreach (var category in categories.Keys)
            {
                formattingLine += new string('-', 47);
            }
            Console.WriteLine(formattingLine);
            Console.Write("{0,10} |", "item #");

            // Print category header
            foreach (var category in categories.Keys)
            {
                Console.Write("{0,46}|", category);
            }
            Console.WriteLine();
            Console.Write(new string(' ', 0) + formattingLine); 
            Console.WriteLine();

            // Print tasks
            for (int i = 0; i <max; i++)
            {
                Console.Write("{0,12}|", i + 1);
                foreach (var category in categories.Values)
                {
                    if (category.Tasks.Count > i)
                    {
                        Task? task = category.Tasks[i];
                        // Highlight if asked
                        if (task.IsHighlighted)
                        {
                            Console.ForegroundColor=ConsoleColor.Red;
                        }
                        Console.Write("{0,46}|", task);
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else
                    {
                        Console.Write("{0,46}|", "N/A");
                    }
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        // Find the maximum number of task in category 
        static int MaximumLength()
        {
            // lambda expression
            // return tasks.Values.Max(list => list.Count);

            int maxCount = 0;
            foreach (var key in categories.Keys) // Loop over all the keys in the categories dictionary.
            {
                // Get the category associated with the key
                TaskCategory? category = categories[key];
                // If the number of tasks in the current category is greater than maxCount,
                // update maxCount to the current category's task count.
                if (category.Tasks.Count > maxCount)
                {
                    maxCount=category.Tasks.Count;
                }
            }
            return maxCount;
        }
        // Create new category
        public void AddNewCategory(string categoryName)
        {
            if (!categories.ContainsKey(categoryName))
            {
                categories[categoryName] = new TaskCategory(categoryName);
            }
        }

        // Delete category
        public void DeleteCategory(string categoryName)
        {
            if (categories.ContainsKey(categoryName))
            {
                categories.Remove(categoryName);
            }
        }

        // Add tasks
        public void AddTask(string categoryName, string description, TaskPriority priority, DateTime dueDate, bool isHighlighted)
        {
            if (categories.ContainsKey(categoryName))
            {
                categories[categoryName].AddTask(description, priority, dueDate, isHighlighted);
            }
            else{
                Console.WriteLine("Invalid category. Task has not added.");
            }
        }

        // Remove tasks
        public void RemoveTask(string categoryName, int taskIndex)
        {
            if (categories.ContainsKey(categoryName))
            {
                categories[categoryName].RemoveTask(taskIndex - 1);
            }
            else{
                Console.WriteLine("Invalid category. Task has not removed.");
            }
        }

        // Change the priority of task
        public void ChangeTaskPriority(string categoryName, int taskIndex, TaskPriority newPriority)
        {
            if (categories.ContainsKey(categoryName))
            {
                categories[categoryName].ChangeTaskPriority(taskIndex-1, newPriority);
            }
            else
            {
                Console.WriteLine("Invalid category. The priority of task has not changed");
            }
        }

        // Highlight the task
        public void HighlightTask(string categoryName, int taskIndex)
        {
            if (categories.ContainsKey(categoryName))
            {
                categories[categoryName].TaskHighlightedInRed(taskIndex-1);
            }
        }

        // Move a task from one to another
        public void MoveTaskTo(string currentCategory, int taskIndex, string transferedCategory)
        {
            if (categories.ContainsKey(currentCategory) && categories.ContainsKey(transferedCategory))
            {
                categories[transferedCategory].AddTask(categories[currentCategory].Tasks[taskIndex-1].Description, categories[currentCategory].Tasks[taskIndex-1].Priority,
categories[currentCategory].Tasks[taskIndex-1].DueDate, categories[currentCategory].Tasks[taskIndex-1].IsHighlighted);
                categories[currentCategory].RemoveTask(taskIndex-1);
            }
        }

        // Move task within a category
        public void MoveTaskWithinCategory(string categoryName, int currentIndex, int newIndex)
        {
            if (categories.ContainsKey(categoryName))
            {
                categories[categoryName].ChangeImportance(currentIndex - 1, newIndex - 1);
            }
        }
    }
    
    class Program
    {
        enum MenuOption
        {
            AddACategory,
            DeleteAnExistingCategory,
            AddATask,
            RemoveATask,
            ChangeTaskPriority,
            MoveATask,
            HighlightATask, 
            ChangeImportance,
            Quit
        }
        static MenuOption ReadUserOption(out MenuOption option)
        {
            int optionValue;
            do
            {
                Console.WriteLine("Task Planner options:\n1. Add a category\n2. Delete an existing category\n3. Add a task to a category\n4. Delete an existing task\n5. Change task priority\n6. Move a task from one category to another\n7. Highlight a task\n8. Change the Importance of tasks within one category\n9. Quit");
                Console.WriteLine("Enter an option: ");
                optionValue = Convert.ToInt32(Console.ReadLine());
            } while (optionValue<1 || optionValue >8);
            option = (MenuOption)optionValue;
            return option;
        }

        // Add a category
        static void AddACategory(TaskManager taskManager)
        {
            string newCategory = TaskManager.ReadString("Enter the name of new category: ");
            taskManager.AddNewCategory(newCategory.ToLower());
        }

        // Delete an existing category
        static void DeleteAnExistingCategory(TaskManager taskManager)
        {
            string categoryToDelete = TaskManager.ReadString("Which categories you want to delete: ");
            taskManager.DeleteCategory(categoryToDelete.ToLower());
        }
        
        // Read the selection of the task priority
        static TaskPriority? ReadTaskPriority()
        {
            Console.WriteLine("Task Priority:");
            Console.WriteLine("1. Low");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. High");
            System.Console.WriteLine("Enter the number (i.e 1, 2, 3):");
            if (int.TryParse(Console.ReadLine(), out int priorityChoice) &&priorityChoice >= 1 && priorityChoice <= 3)
            {
                return (TaskPriority)(priorityChoice-1);
            }
            return null;
        }

        // Read the due date of the task
        static DateTime? ReadDueDate()
        {
            DateTime dueDate;
            bool validDueDate = false;
            do{
                Console.WriteLine("Enter your task's due date (dd-MM-yyyy):");
                if (DateTime.TryParse(Console.ReadLine(), out dueDate))
                {
                    validDueDate =true;
                }
            } while (!validDueDate);
            return dueDate;
        }

        // Add a new task
        static void AddATask(TaskManager taskManager)
        {
            string categoryToAddTask = TaskManager.ReadString("Enter the category you want your task to be allocated: ");
            string newTask = TaskManager.ReadString("Describe your task below (max. 30 symbols).");
            TaskPriority? taskPriority = ReadTaskPriority();
            DateTime? dueDate = ReadDueDate();
            taskManager.AddTask(categoryToAddTask.ToLower(), newTask, taskPriority.Value, dueDate.Value, false);
        }

        // Delete a task
        static void DeleteATask(TaskManager taskManager)
        {
            string categoryToDelete = TaskManager.ReadString("Enter the category you want your task to be deleted:").ToLower();
            int indexOfTaskDeleted;
            if (int.TryParse(TaskManager.ReadString("Enter the task number you want to delete:"), out indexOfTaskDeleted))
            {
                taskManager.RemoveTask(categoryToDelete, indexOfTaskDeleted);
            }
        }

        // Change the task priority
        static void ChangeTaskPriority(TaskManager taskManager)
        {
            string categoryToChangePriority = TaskManager.ReadString("Enter the name of the category to change task priority:");
            int taskIndexToChangePriority;
            if (int.TryParse(TaskManager.ReadString("Enter the task number you want to change priority for:"), out taskIndexToChangePriority))
            {
                TaskPriority? readTaskPriority = ReadTaskPriority();
                taskManager.ChangeTaskPriority(categoryToChangePriority, taskIndexToChangePriority, readTaskPriority.Value);
            }
        }

        // Move a task
        static void MoveATask(TaskManager taskManager)
        {
            string currentCategory = TaskManager.ReadString("Enter the name of the current category:");
            int taskIndexToMove;
            if (int.TryParse(TaskManager.ReadString("Enter the task number you want to move: "), out taskIndexToMove))
            {
                string transferedCategory = TaskManager.ReadString("Enter the name of the transfered category");
                taskManager.MoveTaskTo(currentCategory, taskIndexToMove, transferedCategory);
            }
        }

        // Highlight a task
        static void HighlightATask(TaskManager taskManager)
        {
            string categoryToHaveTaskHighlight = TaskManager.ReadString("Enter the category you want your task to be highlighted: ");
            int taskIndexToHighlight;
            if (int.TryParse(TaskManager.ReadString("Enter the task number you want to highlight: "), out taskIndexToHighlight))
            {
                taskManager.HighlightTask(categoryToHaveTaskHighlight, taskIndexToHighlight);
            }
        }

        static void MoveTaskWithinPriority(TaskManager taskManager)
    {
        string categoryToChangePriority = TaskManager.ReadString("Enter the name of the category to change task priority:");
        int currentIndex;
        if (int.TryParse(TaskManager.ReadString("Enter the current task number: "), out currentIndex))
        {
            int newIndex;
            if (int.TryParse(TaskManager.ReadString("Enter the new position (task number) for the task: "), out newIndex))
            {
                taskManager.MoveTaskWithinCategory(categoryToChangePriority, currentIndex, newIndex);
            }
        }
    }
        

        static void Main(string[] args)
        {
            TaskManager taskManager = new TaskManager();
            while (true)
            {
                Console.Clear();
                taskManager.PrintTask();
                ReadUserOption(out MenuOption option);
                switch (option -1)
                {
                    case MenuOption.AddACategory:
                        AddACategory(taskManager);
                        break;
                    case MenuOption.DeleteAnExistingCategory:
                        DeleteAnExistingCategory(taskManager);
                        break;
                    case MenuOption.AddATask:
                        AddATask(taskManager);
                        break;
                    case MenuOption.RemoveATask:
                        DeleteATask(taskManager);
                        break;
                    case MenuOption.ChangeTaskPriority:
                        ChangeTaskPriority(taskManager);
                        break;
                    case MenuOption.HighlightATask:
                        HighlightATask(taskManager);
                        break;
                    case MenuOption.MoveATask:
                        MoveATask(taskManager);
                        break; 
                    case MenuOption.ChangeImportance:
                        MoveTaskWithinPriority(taskManager);
                        break;
                    case MenuOption.Quit:
                        return;
                    default:
                        Console.WriteLine("Error, Invalid menu choice. Please try again");
                        break;
                }
            }
        }
    }
}
