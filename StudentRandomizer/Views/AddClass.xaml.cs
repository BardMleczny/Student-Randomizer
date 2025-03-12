using System.Xml.Linq;

namespace StudentRandomizer.Views;

public partial class AddClass : ContentPage
{
	public AddClass()
	{
		InitializeComponent();
	}
    public async void OnSaveClicked(object sender, EventArgs e)
    {
		XDocument newClass = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                           new XElement("Class"));
		newClass.Save(Path.Combine(Path.Combine(FileSystem.AppDataDirectory, ClassName.Text + ".txt")));


        await Navigation.PopAsync();
    }
}