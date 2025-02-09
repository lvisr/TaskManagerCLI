using System;
using System.ComponentModel.Design;
using System.Data.Common;
using System.IO;
using Newtonsoft.Json;

public class Task 
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public bool Complete { get; set; }

    public Task()
    {
        Complete = false;
    }
}

class Program 
{
    static String[] menuOptions = new string[] {"a", "add", "v", "view", "d", "delete", "m", "mark", "0", "exit"};
    static List<Task> Tasks = new List<Task>();
    private static string filePath = "tasks.json";
    
    static void Main(string[] args)
    {
        bool exitProgram = false;
        string option;

        // read JSON file to List<Task> Tasks
        LoadTasks();

        while(!exitProgram)
        {
            DisplayMenu();
            option = GetMenuInput();

            switch (option)
                {
                    case "a":
                    case "add":
                        AddTask();
                        SaveTasks();
                        break;                    
                    case "v":
                    case "view":
                        DisplayTasks();
                        Console.WriteLine("\nPress enter to continue");
                        Console.ReadLine();
                        break;
                    case "d":
                    case "delete":                        
                        DisplayTasks();
                        Console.WriteLine("\nSelect Task to Delete");
                        DeleteTask(SelectTask());
                        SaveTasks();
                        break;
                    case "m":
                    case "mark":
                        DisplayTasks();
                        Console.WriteLine("\nSelect Task to Mark as Complete");
                        MarkTaskComplete(SelectTask());
                        SaveTasks();
                        break;
                    case "0":
                    case "exit":
                        exitProgram = true;
                        break;
                    default:
                        break;
                }
        }
    }
    static void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("******************************");
        Console.WriteLine("*****    TASK MANAGER    *****\n");
        Console.WriteLine(" A - Add Tasks");
        Console.WriteLine(" V - View Tasks - Pending/To Do List- Complete");
        Console.WriteLine(" D - Delete Tasks");
        Console.WriteLine(" M - Mark Task as Complete");
        Console.WriteLine(" 0 - Exit");
    }
    static string GetMenuInput()
    {
        String? input;
        do
        {
            Console.Write("\nType option A V D M or Exit: ");
            input = Console.ReadLine();
        } while (input is null || !menuOptions.Contains(input.ToLower()));

        return input.ToLower();
    }
    static string GetStringInput()
    {
        String? input;
        do
        {
            input = Console.ReadLine();
        } while (input is null);

        return input;
    }
    static void AddTask()
    {
        Task taskToAdd = new Task();
        
        Console.WriteLine("New task description: ");
        taskToAdd.Description = GetStringInput();

        taskToAdd.Id = Tasks.Count + 1;
        
        Tasks.Add(taskToAdd);

        Console.WriteLine("\nPress enter to continue");
        Console.ReadLine();
    }
    static void SaveTasks()
    {
        File.WriteAllText(filePath, JsonConvert.SerializeObject(Tasks, Formatting.Indented));
    }
    static void LoadTasks()
    {
        if (File.Exists(filePath))
            Tasks = JsonConvert.DeserializeObject<List<Task>>(File.ReadAllText(filePath)) ?? new List<Task>();
        else 
            Console.WriteLine("There are no saved tasks");
    }
    static void DisplayTasks(int taskId = 0)
    {
        if (taskId == 0) // Display all tasks
        {
            Console.WriteLine("\n*****    ALL TASKS    *****");
            Console.WriteLine("\nTask #\tDescription:\n");
            foreach (Task currentTask in Tasks)
            {   
                if (currentTask.Complete)
                    Console.WriteLine($"   {currentTask.Id} \u2713\t{currentTask.Description}");
                else
                    Console.WriteLine($"   {currentTask.Id}\t{currentTask.Description}");
            }
            Console.WriteLine("\nComplete = \u2713");
        }
        else  // Display task with id = taskId
        {
            Console.WriteLine($"Task #{taskId}\t{Tasks.First(Task => Task.Id == taskId).Description}");
        }

    }
    static int SelectTask()
    {
        int taskId;
        string input;
        Console.WriteLine("\nPlease insert a valid task #");
        do
        {
            input = GetStringInput();
        } while (!(int.TryParse(input, out taskId) && 1 <= taskId && taskId <= Tasks.Count));

        return taskId;
    }
    static void DeleteTask(int idToDelete)
    {
        Console.WriteLine($"\nDeleted:");
        DisplayTasks(idToDelete);

        Tasks = Tasks.Where(Task => Task.Id != idToDelete).ToList();

        foreach (Task item in Tasks)
        {
            if (item.Id >= idToDelete)
                item.Id--;
        }
        Console.WriteLine("\nPress enter to continue");
        Console.ReadLine();
    }
    static void MarkTaskComplete(int idToMark)
    {
        foreach (Task item in Tasks)
        {
            if (item.Id == idToMark)
            {
                item.Complete = true;
                Console.WriteLine("\nSuccessfully Completed \u2713");
                DisplayTasks(idToMark);                
            }
        }
        Console.WriteLine("\nPress enter to continue");
        Console.ReadLine();
    }
}