using System;
using System.IO;
using Newtonsoft.Json;

public class Task 
{
    public int Id { get; set; }
    public string? Description { get; set; }
    //public bool Complete { get; set; }
    /*
    ID
    date added
    description
    status - complete/pending // bool complete false
    */
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

        // read file to List<Task> Tasks
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
                        Console.WriteLine($"");
                        break;                    
                    case "v":
                    case "view":
                        DisplayTasks();
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
            Console.Write("\n ");
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
    static void DisplayTasks()
    {
        foreach (Task currentTask in Tasks)
        {
            Console.WriteLine($"\nID: {currentTask.Id}\nDescription: {currentTask.Description}");
        }
        Console.WriteLine("\nPress enter to continue");
        Console.ReadLine();
    }

}