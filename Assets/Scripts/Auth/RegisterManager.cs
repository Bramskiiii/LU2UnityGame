using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class RegisterManager : MonoBehaviour
{
    [Header("Error display")]
    public TextMeshProUGUI displayText;
    public GameObject textPanel;

    [Header("Account fields")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    [Header("Dependencies")]
    public UserApiClient userApiClient;

    [Header("Objects")]
    public GameObject LoginScreen;
    public GameObject RegisterScreen;
    public GameObject[] Buttons;

    public async void OnRegister()
    {
        if (string.IsNullOrWhiteSpace(emailInput.text) || string.IsNullOrWhiteSpace(passwordInput.text))
        {
            ShowError("Please fill in your email and password.");
            return;
        }

        if (!IsValidPassword(passwordInput.text))
        {
            ShowError("Password must be at least 10 characters and include 1 uppercase, 1 lowercase, 1 number, and 1 special character.");
            return;
        }

        User user = new User
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };

        IWebRequestReponse registerResponse = await userApiClient.Register(user);

        switch (registerResponse)
        {
            case WebRequestData<string>:
                Debug.Log("Register success!");
                LoginScreen.SetActive(true);
                RegisterScreen.SetActive(false);
                ScreenManager.Instance.SwitchTo(0);
                foreach (var button in Buttons)
                {
                    button.SetActive(false);
                }
                break;

            case WebRequestError errorResponse:
                Debug.Log("Register error: " + errorResponse.ErrorMessage);
                ShowError("Registration failed. Please try again.");
                break;

            default:
                throw new NotImplementedException(
                    "No implementation for registerResponse of class: " + registerResponse.GetType());
        }
    }

    private bool IsValidPassword(string pass)
    {
        if (pass.Length >= 10)
        {
            bool hasLower = pass.Any(char.IsLower);
            bool hasUpper = pass.Any(char.IsUpper);
            bool hasDigit = pass.Any(char.IsDigit);
            bool hasNonAlphanumeric = pass.Any(c => !char.IsLetterOrDigit(c));

            return hasLower && hasUpper && hasDigit && hasNonAlphanumeric;
        }
        return false;
    }

    private void ShowError(string message)
    {
        displayText.text = message;
        textPanel.SetActive(true);
    }

    public void ToLogin()
    {
        RegisterScreen.SetActive(false);
        LoginScreen.SetActive(true);
        ScreenManager.Instance.currentScreen = 0;
    }
}