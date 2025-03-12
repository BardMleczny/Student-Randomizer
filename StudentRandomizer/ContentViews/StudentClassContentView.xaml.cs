using StudentRandomizer.Models;
using StudentRandomizer.Views;

namespace StudentRandomizer.ContentViews;

public partial class StudentClassContentView : ContentView
{
	private StudentClass studentClass;
    public StudentClassContentView(StudentClass studentClass)
    {
        InitializeComponent();
        BindingContext = studentClass;
        this.studentClass = studentClass;

        int numberOfPresentStudents = 0;
        foreach(Student student in studentClass.students)
            if(student.isPresent)
                numberOfPresentStudents++;

        NumberOfStudents.Text = $"present students: {numberOfPresentStudents}/{studentClass.students.Count}";
    }

    private async void OnSelectClicked(object sender, EventArgs e)
    {
        ClassManager.ChangeCurrentClass(studentClass.Name);
        await Shell.Current.GoToAsync(nameof(StudentList));
    }
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, $"{studentClass.Name}.txt");

        File.Delete(filePath);
        DeleteRequested?.Invoke(this, studentClass);
    }

    public event EventHandler<StudentClass> DeleteRequested;
}