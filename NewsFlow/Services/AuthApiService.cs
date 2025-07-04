﻿using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.Communication;
using NewsFlow.Models;
using NewsFlow.Services;

public class AuthApiService
{
    private readonly HttpClient _httpClient;

    public AuthApiService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri($"{AppConfig.ApiBaseUrl}/auth") };
        LoadToken();
    }

    private async void LoadToken()
    {
        var token = await SecureStorage.GetAsync("auth_token");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }


    public async Task<bool> Register(string username, string email, string password)
    {
        var content = new StringContent(JsonSerializer.Serialize(new { UserName = username, Email = email, FullName = username, Password = password }),
            Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("auth/register", content);
        if (response.IsSuccessStatusCode)
            return true;

        var responseContent = await response.Content.ReadAsStringAsync();
        var errors = JsonSerializer.Deserialize<List<IdentityError>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var message = string.Join("\n", errors?.Select(e => e.Description) ?? new[] { "Eroare necunoscută" });
        await Application.Current.MainPage.DisplayAlert("Eroare", message, "OK");

        return false;
    }


    public async Task<string> Login(string email, string password)
    {

        var content = new StringContent(JsonSerializer.Serialize(new { Email = email, Password = password }), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("auth/login", content);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<LoginResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (result?.Token != null)
        {
            await SecureStorage.SetAsync("auth_token", result.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
        }

        return result?.Token;
    }
}
