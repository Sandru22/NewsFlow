using Microsoft.Maui.Controls;
using Shiny;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace NewsFlow.Register;

public partial class RegisterPage : ContentPage
{
    private readonly AuthApiService _apiService = new();

    public RegisterPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {

        if (FullNameEntry.Text.IsEmpty() || EmailEntry.Text.IsEmpty() || PasswordEntry.Text.IsEmpty() || ConfirmPasswordEntry.Text.IsEmpty())
        {
            await DisplayAlert("Eroare", "Este necesar să completaţi toate câmpurile!", "OK");
        }
        else
        {
            var success = await _apiService.Register(FullNameEntry.Text, EmailEntry.Text, PasswordEntry.Text);
            if (success)
            {
                await DisplayAlert("Succes", "Utilizator creat!", "OK");
                await Navigation.PopAsync();

            }
        }
    }
    


}