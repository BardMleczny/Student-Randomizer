using StudentRandomizer.Models;
using System.Xml.Linq;
using System.IO;
using Microsoft.Maui.Controls;

namespace StudentRandomizer.ContentViews;

public partial class StudentContentView : ContentView
{
    public Student student;

    public StudentContentView(Student student)
    {
        BindingContext = student;
        this.student = student;
        InitializeComponent();

        switch(student.chosenQueueIndex)
        {
            case 0:
                ClassLayout.BackgroundColor = new Color(51, 51, 51);
                break;
            case 1:
                ClassLayout.BackgroundColor = new Color(40, 40, 80);
                break;
            case 2:
                ClassLayout.BackgroundColor = new Color(30, 30, 120);
                break;
            case 3:
                ClassLayout.BackgroundColor = new Color(20, 20, 180);
                break;
        }
        
    }
    private void OnPresentChanged(object sender, CheckedChangedEventArgs e)
    {
        bool isPresent = e.Value;

        student.isPresent = isPresent;

        string filePath = ClassManager.currentClass;

        XDocument xmlDoc = XDocument.Load(filePath);

        var studentToUpdate = xmlDoc.Root?.Elements("Student")
            .FirstOrDefault(s => s.Element("Number")?.Value == student.number.ToString());
        studentToUpdate.Element("IsPresent")?.SetValue(isPresent);

        xmlDoc.Save(filePath);

        ChangeRequested?.Invoke(this, student);
    }
    private void OnRemoveClicked(object sender, EventArgs e)
    {
        string filePath = ClassManager.currentClass;
        XDocument xmlDoc = XDocument.Load(filePath);

        var studentToRemove = xmlDoc.Root?.Elements("Student")
            .FirstOrDefault(s => s.Element("Number")?.Value == student.number.ToString());

        if (studentToRemove != null)
        {
            studentToRemove.Remove();

            var subsequentStudents = xmlDoc.Root?.Elements("Student")
                .Where(s => int.Parse(s.Element("Number")?.Value ?? "0") > student.number)
                .ToList();

            if (subsequentStudents != null)
            {
                foreach (var subsequentStudent in subsequentStudents)
                {
                    int currentNumber = int.Parse(subsequentStudent.Element("Number")?.Value ?? "0");
                    subsequentStudent.Element("Number")?.SetValue(currentNumber - 1);
                }
            }

            xmlDoc.Save(filePath);

            ChangeRequested?.Invoke(this, student);
        }
    }
    public event EventHandler<Student> ChangeRequested;
}