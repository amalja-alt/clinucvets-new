using ClinicVets.Models;
using ClinicVets.Services;

namespace ClinicVets.UI;

/// <summary>
/// Registers a new clinic employee with validation.
/// </summary>
public class RegisterEmployeeForm : Form
{
    private readonly ClinicAppServices _services;
    private readonly TextBox _usernameTextBox;
    private readonly TextBox _passwordTextBox;
    private readonly TextBox _employeeNumberTextBox;
    private readonly TextBox _emailTextBox;
    private readonly TextBox _identityTextBox;
    private readonly ComboBox _roleComboBox;
    private readonly Label _statusLabel;

    public RegisterEmployeeForm(ClinicAppServices services)
    {
        _services = services;

        UiTheme.ApplyForm(this);
        Text = "ClinicVets - Register Employee";
        ClientSize = new Size(1280, 820);
        MinimumSize = new Size(980, 760);
        StartPosition = FormStartPosition.CenterParent;
        Paint += (_, e) => UiTheme.PaintGradientBackground(this, e);

        TableLayoutPanel shell = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 1,
            BackColor = Color.Transparent,
            Padding = new Padding(20)
        };
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Controls.Add(shell);

        RoundedPanel card = new()
        {
            Width = 960,
            Height = 740,
            Anchor = AnchorStyles.None,
            BackColor = UiTheme.Card,
            BorderColor = UiTheme.Border,
            BorderSize = 1,
            CornerRadius = 18,
            Padding = new Padding(40, 30, 40, 30)
        };
        shell.Controls.Add(card, 0, 0);

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 112));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 66));
        card.Controls.Add(layout);

        layout.Controls.Add(CreateTopBar(), 0, 0);
        layout.Controls.Add(CreateHeader(), 0, 1);

        TableLayoutPanel fieldsGrid = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 3,
            Margin = new Padding(0, 8, 0, 8)
        };
        fieldsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        fieldsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        fieldsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        fieldsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        fieldsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34F));
        layout.Controls.Add(fieldsGrid, 0, 2);

        _usernameTextBox = CreateTextBox("Enter username");
        _passwordTextBox = CreateTextBox("Enter password", password: true);
        _employeeNumberTextBox = CreateTextBox("e.g., 1234");
        _emailTextBox = CreateTextBox("employee@clinicvets.com");
        _identityTextBox = CreateTextBox("9 digits");
        _roleComboBox = CreateRoleComboBox();

        fieldsGrid.Controls.Add(CreateFieldBlock("Username", "6-8 chars, max 2 digits", _usernameTextBox), 0, 0);
        fieldsGrid.Controls.Add(CreateFieldBlock("Password", "8-10 chars, letter, digit, !/#/$", _passwordTextBox), 1, 0);
        fieldsGrid.Controls.Add(CreateFieldBlock("Employee ID", "Exactly 4 digits", _employeeNumberTextBox), 0, 1);
        fieldsGrid.Controls.Add(CreateFieldBlock("Email Address", "Valid email format", _emailTextBox), 1, 1);
        fieldsGrid.Controls.Add(CreateFieldBlock("Identity Number", "Exactly 9 digits", _identityTextBox), 0, 2);
        fieldsGrid.Controls.Add(CreateFieldBlock("Role", "Veterinarian or Secretary", _roleComboBox), 1, 2);

        _statusLabel = new Label
        {
            Dock = DockStyle.Fill,
            ForeColor = UiTheme.Muted,
            Text = "All fields are validated before saving.",
            TextAlign = ContentAlignment.MiddleCenter,
            Font = UiTheme.SubtitleFont,
            AutoEllipsis = true,
            Margin = new Padding(0, 4, 0, 4)
        };
        layout.Controls.Add(_statusLabel, 0, 3);

        TableLayoutPanel buttons = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Margin = new Padding(0, 10, 0, 0)
        };
        buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36));
        buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64));
        layout.Controls.Add(buttons, 0, 4);

        Button cancelButton = UiTheme.CreateSecondaryButton("Cancel", 0, 0, 220, 48);
        cancelButton.Dock = DockStyle.Fill;
        cancelButton.Margin = new Padding(0, 0, 14, 0);
        cancelButton.Click += (_, _) => Close();
        buttons.Controls.Add(cancelButton, 0, 0);

        Button registerButton = UiTheme.CreatePrimaryButton("Register Employee", 0, 0, 380, 48);
        registerButton.Dock = DockStyle.Fill;
        registerButton.Margin = new Padding(14, 0, 0, 0);
        registerButton.Click += (_, _) => RegisterEmployee();
        buttons.Controls.Add(registerButton, 1, 0);

        AcceptButton = registerButton;
        CancelButton = cancelButton;
    }

    private Control CreateTopBar()
    {
        TableLayoutPanel topBar = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1
        };
        topBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        topBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170));

        Label brand = new()
        {
            Text = "ClinicVets",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 17F, FontStyle.Bold),
            ForeColor = Color.FromArgb(20, 84, 132),
            TextAlign = ContentAlignment.MiddleLeft
        };
        topBar.Controls.Add(brand, 0, 0);

        Button backButton = UiTheme.CreateSecondaryButton("Back to Login", 0, 0, 160, 38);
        backButton.Dock = DockStyle.Fill;
        backButton.Margin = new Padding(0, 6, 0, 6);
        backButton.Click += (_, _) => Close();
        topBar.Controls.Add(backButton, 1, 0);

        return topBar;
    }

    private static Control CreateHeader()
    {
        TableLayoutPanel header = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Margin = new Padding(0, 4, 0, 10)
        };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        header.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
        header.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));

        header.Controls.Add(new Label
        {
            Text = "Register Employee",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 24F, FontStyle.Bold),
            ForeColor = UiTheme.Text,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 0);

        header.Controls.Add(new Label
        {
            Text = "Create a staff account for a Secretary or Veterinarian.",
            Dock = DockStyle.Fill,
            Font = UiTheme.SubtitleFont,
            ForeColor = UiTheme.Muted,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 1);

        return header;
    }

    private static TextBox CreateTextBox(string placeholder, bool password = false)
    {
        return new TextBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 11.5F),
            BorderStyle = BorderStyle.FixedSingle,
            ForeColor = UiTheme.InputText,
            PlaceholderText = placeholder,
            UseSystemPasswordChar = password,
            Margin = new Padding(0, 5, 0, 6)
        };
    }

    private static ComboBox CreateRoleComboBox()
    {
        ComboBox comboBox = new()
        {
            Dock = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 11.5F),
            ForeColor = UiTheme.InputText,
            Margin = new Padding(0, 5, 0, 6)
        };
        comboBox.Items.AddRange(["Veterinarian", "Secretary"]);
        comboBox.SelectedIndex = 1;
        return comboBox;
    }

    private static Control CreateFieldBlock(string labelText, string hintText, Control input)
    {
        TableLayoutPanel block = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Margin = new Padding(0, 0, 24, 10)
        };
        block.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        block.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
        block.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
        block.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

        block.Controls.Add(new Label
        {
            Text = $"{labelText} *",
            Dock = DockStyle.Fill,
            Font = UiTheme.LabelFont,
            ForeColor = UiTheme.Text,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 0);

        block.Controls.Add(input, 0, 1);

        block.Controls.Add(new Label
        {
            Text = hintText,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9F),
            ForeColor = UiTheme.Muted,
            TextAlign = ContentAlignment.TopLeft,
            AutoEllipsis = true
        }, 0, 2);

        return block;
    }

    private void RegisterEmployee()
    {
        StaffRole role = _roleComboBox.SelectedIndex == 0
            ? StaffRole.Veterinarian
            : StaffRole.Secretary;

        OperationResult<Employee> result = _services.EmployeeService.RegisterEmployee(
            _usernameTextBox.Text.Trim(),
            _passwordTextBox.Text,
            _employeeNumberTextBox.Text.Trim(),
            _emailTextBox.Text.Trim(),
            _identityTextBox.Text.Trim(),
            role);

        if (!result.IsSuccess)
        {
            _statusLabel.ForeColor = UiTheme.Danger;
            _statusLabel.Text = result.ErrorMessage;
            return;
        }

        _statusLabel.ForeColor = UiTheme.Success;
        _statusLabel.Text = $"Employee {result.Value!.Username} registered successfully.";
        DialogResult = DialogResult.OK;
        Close();
    }
}
