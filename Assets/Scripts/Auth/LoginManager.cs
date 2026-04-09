using System;
using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    [Header("Error display")]
    public TextMeshProUGUI displayText;
    public GameObject textPanel;

    [Header("Account fields")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public GameObject loginScreen;
    public int nextScreen;

    [Header("Dependencies")]
    public UserApiClient userApiClient;

    [Header("Objects")]
    public GameObject LoginScreen;
    public GameObject RegisterScreen;
    public GameObject[] Buttons;

    public async void OnLogin()
    {
        if (string.IsNullOrWhiteSpace(emailInput.text) || string.IsNullOrWhiteSpace(passwordInput.text))
        {
            ShowError("Please fill in your email and password.");
            return;
        }

        User user = new User
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };

        IWebRequestReponse loginResponse = await userApiClient.Login(user);

        switch (loginResponse)
        {
            case WebRequestData<string>:
                Debug.Log("Login success!");
                loginScreen.SetActive(false);
                ScreenManager.Instance.SwitchTo(2);
                foreach (var button in Buttons)
                {
                    button.SetActive(false);
                }
                break;

            case WebRequestError errorResponse:
                Debug.Log("Login error: " + errorResponse.ErrorMessage);
                ShowError("Incorrect email or password.");
                break;

            default:
                throw new NotImplementedException(
                    "No implementation for loginResponse of class: " + loginResponse.GetType());
        }
    }

    private void ShowError(string message)
    {
        displayText.text = message;
        textPanel.SetActive(true);
    }

    public void ToRegister()
    {
        RegisterScreen.SetActive(true);
        LoginScreen.SetActive(false);
        ScreenManager.Instance.currentScreen = 1;
    }
}