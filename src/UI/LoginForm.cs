using ClinicVets.Models;
using ClinicVets.Services;

namespace ClinicVets.UI;

/// <summary>
/// GUI login screen for clinic staff members.
/// </summary>
public class LoginForm : Form
{
    private readonly ClinicAppServices _services;
    private readonly TextBox _usernameTextBox = new();
    private readonly TextBox _passwordTextBox = new();
    private readonly Label _errorLabel = new();

    public LoginForm(ClinicAppServices services)
    {
        _services = services;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        UiTheme.ApplyForm(this);
        Text = "ClinicVets - Staff Login";
        ClientSize = new Size(1280, 720);
        MinimumSize = new Size(620, 680);
        StartPosition = FormStartPosition.CenterScreen;
        Paint += (_, e) => UiTheme.PaintGradientBackground(this, e);

        TableLayoutPanel shell = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = Color.Transparent,
            Padding = new Padding(24)
        };
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        shell.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        Controls.Add(shell);

        RoundedPanel loginCard = new()
        {
            Width = 480,
            Height = 610,
            Anchor = AnchorStyles.None,
            BackColor = UiTheme.Card,
            BorderColor = UiTheme.Border,
            BorderSize = 1,
            CornerRadius = 18,
            Padding = new Padding(34, 30, 34, 28)
        };
        shell.Controls.Add(loginCard, 0, 1);

        TableLayoutPanel cardLayout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 10
        };
        cardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
        cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
        cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 82));
        cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 82));
        cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
        cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        loginCard.Controls.Add(cardLayout);

        cardLayout.Controls.Add(CreateLogo(), 0, 0);
        cardLayout.Controls.Add(CreateTitle("ClinicVets", 22F, UiTheme.Text, FontStyle.Bold), 0, 1);
        cardLayout.Controls.Add(CreateTitle("Veterinary Clinic Management System", 10F, UiTheme.Muted, FontStyle.Regular), 0, 2);

        _errorLabel.Name = "errorLabel";
        _errorLabel.Dock = DockStyle.Fill;
        _errorLabel.ForeColor = UiTheme.Danger;
        _errorLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _errorLabel.TextAlign = ContentAlignment.MiddleCenter;
        _errorLabel.AutoEllipsis = true;
        cardLayout.Controls.Add(_errorLabel, 0, 3);

        _usernameTextBox.Name = "usernameTextBox";
        cardLayout.Controls.Add(CreateInputBlock("Username", _usernameTextBox, "Enter your username", false), 0, 4);

        _passwordTextBox.Name = "passwordTextBox";
        cardLayout.Controls.Add(CreateInputBlock("Password", _passwordTextBox, "Enter your password", true), 0, 5);

        Button loginButton = UiTheme.CreatePrimaryButton("Login", 0, 0, 400, 48);
        loginButton.Name = "loginButton";
        loginButton.Dock = DockStyle.Fill;
        loginButton.Margin = new Padding(0, 8, 0, 4);
        loginButton.Click += LoginButton_Click;
        cardLayout.Controls.Add(loginButton, 0, 6);

        Label separator = new()
        {
            Dock = DockStyle.Fill,
            Height = 1,
            BackColor = UiTheme.Border,
            Margin = new Padding(0, 15, 0, 12)
        };
        cardLayout.Controls.Add(separator, 0, 7);

        Label registerHintLabel = new()
        {
            Text = "Need to register a new employee?",
            Dock = DockStyle.Fill,
            Font = UiTheme.SubtitleFont,
            ForeColor = UiTheme.Muted,
            TextAlign = ContentAlignment.MiddleCenter
        };
        cardLayout.Controls.Add(registerHintLabel, 0, 8);

        Button registerButton = UiTheme.CreateSoftPrimaryButton("Register Employee", 0, 0, 400, 48);
        registerButton.Name = "registerEmployeeButton";
        registerButton.Dock = DockStyle.Fill;
        registerButton.Margin = new Padding(0, 4, 0, 0);
        registerButton.Click += RegisterButton_Click;
        cardLayout.Controls.Add(registerButton, 0, 9);

        Label footerLabel = new()
        {
            Text = "Professional Veterinary Care Management",
            Dock = DockStyle.Bottom,
            Height = 30,
            Font = UiTheme.SubtitleFont,
            ForeColor = UiTheme.Muted,
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.Transparent
        };
        Controls.Add(footerLabel);
        footerLabel.BringToFront();

        AcceptButton = loginButton;
    }

    private static Control CreateLogo()
    {
        RoundedPanel iconPanel = new()
        {
            Size = new Size(58, 58),
            Anchor = AnchorStyles.None,
            BackColor = UiTheme.Primary,
            BorderSize = 0,
            CornerRadius = 14
        };
        iconPanel.Controls.Add(new Label
        {
            Text = "CV",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter
        });
        return iconPanel;
    }

    private static Label CreateTitle(string text, float size, Color color, FontStyle style)
    {
        return new Label
        {
            Text = text,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", size, style),
            ForeColor = color,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoEllipsis = true
        };
    }

    private static Control CreateInputBlock(string labelText, TextBox textBox, string placeholder, bool password)
    {
        TableLayoutPanel block = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Margin = new Padding(0, 4, 0, 8)
        };
        block.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        block.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        block.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        Label label = new()
        {
            Text = labelText,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = UiTheme.Text,
            TextAlign = ContentAlignment.MiddleLeft
        };
        block.Controls.Add(label, 0, 0);

        textBox.Dock = DockStyle.Fill;
        textBox.Font = new Font("Segoe UI", 11F);
        textBox.BorderStyle = BorderStyle.FixedSingle;
        textBox.ForeColor = UiTheme.Text;
        textBox.PlaceholderText = placeholder;
        textBox.UseSystemPasswordChar = password;
        textBox.Margin = new Padding(0, 4, 0, 0);
        block.Controls.Add(textBox, 0, 1);

        return block;
    }

    private void LoginButton_Click(object? sender, EventArgs e)
    {
        AuthenticationResult result = _services.AuthService.Login(
            _usernameTextBox.Text.Trim(),
            _passwordTextBox.Text);

        if (!result.IsSuccess)
        {
            _errorLabel.Text = result.ErrorMessage;
            return;
        }

        _errorLabel.Text = string.Empty;
        Hide();

        string welcome = _services.AuthService.CurrentUser is null
            ? "Veterinary Clinic Management"
            : $"Welcome, {_services.AuthService.CurrentUser.Username} ({_services.AuthService.CurrentUser.Role})";

        using Form dashboardForm = result.LoggedInUser?.Role == StaffRole.Veterinarian
            ? new VeterinarianDashboardForm(_services, welcome)
            : new SecretaryDashboardForm(_services, welcome);
        dashboardForm.ShowDialog(this);

        _services.AuthService.Logout();
        _passwordTextBox.Clear();
        Show();
    }

    private void RegisterButton_Click(object? sender, EventArgs e)
    {
        using RegisterEmployeeForm registerForm = new(_services);
        registerForm.ShowDialog(this);
    }
}
