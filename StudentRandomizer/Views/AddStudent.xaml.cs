using StudentRandomizer.Models;
using System.Xml.Linq;

namespace StudentRandomizer.Views;

public partial class AddStudent : ContentPage
{
	public AddStudent()
	{
		InitializeComponent();
	}

    public async void OnSaveClicked(object sender, EventArgs e)
    {
        string name = StudentName.Text;
        string surname = StudentSurname.Text;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
        {
            await DisplayAlert("Error", "Proszê podaj imiê i nazwisko.", "OK");
            return;
        }
        var student = new Student(name, surname);

        string filePath = ClassManager.currentClass;

        XDocument currentClassFile;
        currentClassFile = XDocument.Load(filePath);

        student.number = currentClassFile.Root?.Elements("Student").Count() + 1 ?? 1;

        var studentElement = new XElement("Student",
            new XElement("Name", student.name),
            new XElement("Surname", student.surname),
            new XElement("IsPresent", student.isPresent),
            new XElement("Number", student.number),
            new XElement("ChosenQueueIndex", student.chosenQueueIndex)
        );

        currentClassFile.Root?.Add(studentElement);

        currentClassFile.Save(filePath);

        await Navigation.PopAsync();
    }
}