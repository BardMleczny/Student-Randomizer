using StudentRandomizer.ContentViews;
using StudentRandomizer.Models;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudentRandomizer.Views;

public partial class StudentList : ContentPage
{
    List<Student> students;
	public StudentList()
	{
		InitializeComponent();
        LuckyNumber.Text = $"Today's lucky number: {ClassManager.todaysLuckyNumber.ToString()}";
	}
    private async void AddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddStudent));
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadStudents();
    }
    private async void OnRandomClicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button != null)
        {
            await button.ScaleTo(0.95, 100); 
            button.FadeTo(0.5, 100);
            await button.ScaleTo(1, 100);
            button.FadeTo(1, 100);
        }
        List<Student> studentsToExclude = new List<Student>();
        List<Student> studentsCopy = new List<Student>(students);
        foreach (Student student in studentsCopy)
        {
            if (!student.isPresent || student.number == ClassManager.todaysLuckyNumber)
                studentsToExclude.Add(student);
        }

        foreach (Student student in studentsToExclude)
        {
            studentsCopy.Remove(student);
        }

        if (studentsCopy.Count > 0)
        {
            int recentlyAskedStudents = 0;
            foreach (Student student in studentsCopy)
            {
                if(student.chosenQueueIndex != 0)
                    recentlyAskedStudents++;

            }
            Student selectedStudent;
            do
            {
                int selectedStudentIndex = new Random().Next(studentsCopy.Count);
                selectedStudent = studentsCopy[selectedStudentIndex];
            } while (selectedStudent.chosenQueueIndex != 0 && recentlyAskedStudents != studentsCopy.Count);
            

            

            selectedStudent.chosenQueueIndex = 3;
            foreach (Student student in students)
            {
                if (student != selectedStudent && student.chosenQueueIndex > 0)
                {
                    student.chosenQueueIndex -= 1;
                }
            }

            ChosenStudentNumber.Text = $"Number: {selectedStudent.number}";
            ChosenStudentName.Text = $"Name: {selectedStudent.name}";
            ChosenStudentSurname.Text = $"Surname: {selectedStudent.surname}";

            SaveStudentsToXml();

            LoadStudents();
        }
        else
        {
            ChosenStudentNumber.Text = $"Number: no students present";
            ChosenStudentName.Text = $"Name: no students present";
            ChosenStudentSurname.Text = $"Surname: no students present";
        }
    }
    private void SaveStudentsToXml()
    {
        string filePath = ClassManager.currentClass;

        XDocument xmlDoc = XDocument.Load(filePath);

        foreach (var student in students)
        {
            var studentElement = xmlDoc.Root?.Elements("Student")
                .FirstOrDefault(s => s.Element("Number")?.Value == student.number.ToString());

            if (studentElement != null)
            {
                studentElement.Element("ChosenQueueIndex")?.SetValue(student.chosenQueueIndex);
            }
        }

        xmlDoc.Save(filePath);
    }
    private void OnChangeRequested(object sender, Student student)
    {
        if (sender is StudentContentView studentView)
        {
            LoadStudents();
        }
    }
    private void LoadStudents()
    {
        StudentsStackLayout.Children.Clear();

        string filePath = ClassManager.currentClass;


        
        XDocument currentClassFile = XDocument.Load(filePath);

        var students = currentClassFile.Root?.Elements("Student")
            .Select(s => new Student(
                s.Element("Name")?.Value,
                s.Element("Surname")?.Value)
            {
                isPresent = bool.Parse(s.Element("IsPresent")?.Value ?? "false"),
                number = int.Parse(s.Element("Number")?.Value ?? "0"),
                chosenQueueIndex = int.Parse(s.Element("ChosenQueueIndex")?.Value ?? "0")
            })
            .ToList();
        this.students = students;
        if (students != null)
        {
            foreach (var student in students)
            {
                var studentView = new StudentContentView(student);
                studentView.ChangeRequested += OnChangeRequested;
                StudentsStackLayout.Children.Add(studentView);
            }
        }
    }
}