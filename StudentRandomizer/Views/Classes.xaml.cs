using StudentRandomizer.Models;
using StudentRandomizer.ContentViews;
using System.Xml.Linq;

namespace StudentRandomizer.Views;

public partial class Classes : ContentPage
{
	public Classes()
	{
		InitializeComponent();

        ClassManager.GenerateTodaysLuckyNumber();
    }
    private async void AddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddClass));
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadClasses();
    }
    private void OnDeleteRequested(object sender, StudentClass studentClass)
    {
        if (sender is StudentClassContentView classView)
        {
            ClassesStackLayout.Children.Remove(classView);
        }
    }
    private void LoadClasses()
    {
        ClassesStackLayout.Children.Clear();

        var files = Directory.GetFiles(FileSystem.AppDataDirectory, "*.txt");
        foreach (var file in files)
        {
            var doc = XDocument.Load(file);
            var className = Path.GetFileNameWithoutExtension(file);
            var studentClass = new StudentClass { Name = className, students = new List<Student>() };
            
            var students = doc.Root?.Elements("Student")
            .Select(s => new Student(
                s.Element("Name")?.Value,
                s.Element("Surname")?.Value)
            {
                isPresent = bool.Parse(s.Element("IsPresent")?.Value ?? "false"),
                number = int.Parse(s.Element("Number")?.Value ?? "0"),
                chosenQueueIndex = int.Parse(s.Element("ChosenQueueIndex")?.Value ?? "0")
            })
            .ToList();
            studentClass.students.AddRange(students);

            var classView = new StudentClassContentView(studentClass);
            classView.DeleteRequested += OnDeleteRequested;
            ClassesStackLayout.Children.Add(classView);
        }
    }
}