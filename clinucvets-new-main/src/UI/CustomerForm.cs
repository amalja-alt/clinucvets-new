using ClinicVets.Models;
using ClinicVets.Services;

namespace ClinicVets.UI;

/// <summary>
/// Customer registration, search, and linked animals display.
/// </summary>
public class CustomerForm : Form
{
    private readonly ClinicAppServices _services;
    private readonly TextBox _fullNameTextBox;
    private readonly TextBox _identityTextBox;
    private readonly TextBox _phoneTextBox;
    private readonly TextBox _emailTextBox;
    private readonly TextBox _searchTextBox;
    private readonly TextBox _detailsTextBox;
    private readonly ListBox _animalsList;
    private readonly Label _statusLabel;
    private readonly Button _addButton;
    private readonly Panel _addPanel;

    public CustomerForm(ClinicAppServices services)
    {
        _services = services;

        UiTheme.ApplyForm(this);
        Text = "ClinicVets - Customers";
        ClientSize = new Size(1040, 680);
        MinimumSize = new Size(900, 620);

        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(24),
            BackColor = UiTheme.Background
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        Controls.Add(root);

        root.Controls.Add(new Label
        {
            Text = "Customer Management",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = UiTheme.Text,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 0);

        bool isSecretary = _services.AuthService.CurrentUser?.Role == StaffRole.Secretary;
        // Assignment requirement: customer management is available only to secretaries.
        if (!isSecretary)
        {
            Shown += (_, _) =>
            {
                MessageBox.Show(ValidationMessages.CustomerManagementSecretaryOnly, "Permission",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            };
        }

        _addPanel = UiTheme.CreateCard(0, 0, 960, 230, 14);
        _addPanel.Dock = DockStyle.Fill;
        _addPanel.Padding = new Padding(20);
        _addPanel.Margin = new Padding(0, 0, 0, 16);
        root.Controls.Add(_addPanel, 0, 1);

        TableLayoutPanel addLayout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 4
        };
        addLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
        addLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22));
        addLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22));
        addLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26));
        addLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        addLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
        addLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
        addLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _addPanel.Controls.Add(addLayout);

        Label addTitle = new()
        {
            Text = "Add New Customer",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            ForeColor = UiTheme.Text
        };
        addLayout.Controls.Add(addTitle, 0, 0);
        addLayout.SetColumnSpan(addTitle, 4);

        addLayout.Controls.Add(CreateDockLabel("Full name"), 0, 1);
        addLayout.Controls.Add(CreateDockLabel("ID number"), 1, 1);
        addLayout.Controls.Add(CreateDockLabel("Phone"), 2, 1);
        addLayout.Controls.Add(CreateDockLabel("Email"), 3, 1);

        _fullNameTextBox = CreateDockTextBox(isSecretary);
        _identityTextBox = CreateDockTextBox(isSecretary);
        _phoneTextBox = CreateDockTextBox(isSecretary);
        _emailTextBox = CreateDockTextBox(isSecretary);
        _fullNameTextBox.Name = "customerFullNameTextBox";
        _identityTextBox.Name = "customerIdentityTextBox";
        _phoneTextBox.Name = "customerPhoneTextBox";
        _emailTextBox.Name = "customerEmailTextBox";
        addLayout.Controls.Add(_fullNameTextBox, 0, 2);
        addLayout.Controls.Add(_identityTextBox, 1, 2);
        addLayout.Controls.Add(_phoneTextBox, 2, 2);
        addLayout.Controls.Add(_emailTextBox, 3, 2);

        _addButton = UiTheme.CreatePrimaryButton("Add Customer", 0, 0, 170, 42);
        _addButton.Name = "addCustomerButton";
        _addButton.Enabled = isSecretary;
        _addButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;
        _addButton.Margin = new Padding(0, 12, 0, 0);
        _addButton.Click += (_, _) => AddCustomer();
        addLayout.Controls.Add(_addButton, 0, 3);

        if (!isSecretary)
        {
            Label secretaryNote = new()
            {
                Text = "Only a secretary can register customers.",
                Location = new Point(170, 202),
                Size = new Size(210, 32),
                ForeColor = UiTheme.Warning,
                Font = UiTheme.SubtitleFont
            };
            addLayout.Controls.Add(secretaryNote, 1, 3);
            addLayout.SetColumnSpan(secretaryNote, 3);
        }

        RoundedPanel searchPanel = UiTheme.CreateCard(0, 0, 960, 260, 14);
        searchPanel.Dock = DockStyle.Fill;
        searchPanel.Padding = new Padding(20);
        searchPanel.Margin = new Padding(0, 0, 0, 16);
        root.Controls.Add(searchPanel, 0, 2);

        TableLayoutPanel searchLayout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 3
        };
        searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        searchLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        searchLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
        searchLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        searchPanel.Controls.Add(searchLayout);

        Label searchTitle = new()
        {
            Text = "Search Customer",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            ForeColor = UiTheme.Text
        };
        searchLayout.Controls.Add(searchTitle, 0, 0);
        searchLayout.SetColumnSpan(searchTitle, 2);

        _searchTextBox = CreateDockTextBox(true);
        _searchTextBox.Name = "customerSearchTextBox";
        _searchTextBox.PlaceholderText = "Search by ID or phone";
        searchLayout.Controls.Add(_searchTextBox, 0, 1);

        Button searchButton = UiTheme.CreateSecondaryButton("Search", 0, 0, 120, 38);
        searchButton.Name = "searchCustomerButton";
        searchButton.Anchor = AnchorStyles.Left;
        searchButton.Margin = new Padding(12, 6, 0, 6);
        searchButton.Click += (_, _) => SearchCustomer();
        searchLayout.Controls.Add(searchButton, 1, 1);

        _detailsTextBox = new TextBox
        {
            Name = "customerDetailsTextBox",
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 8, 12, 0),
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            BackColor = Color.FromArgb(248, 250, 252)
        };
        searchLayout.Controls.Add(_detailsTextBox, 0, 2);

        _animalsList = new ListBox
        {
            Name = "customerAnimalsListBox",
            Dock = DockStyle.Fill,
            Margin = new Padding(12, 8, 0, 0),
            IntegralHeight = false
        };
        searchLayout.Controls.Add(_animalsList, 1, 2);

        FlowLayoutPanel buttons = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft
        };
        root.Controls.Add(buttons, 0, 3);

        Button backButton = UiTheme.CreateSecondaryButton("Back to Dashboard", 0, 0, 170, 42);
        backButton.Name = "backToDashboardButton";
        backButton.Click += (_, _) => Close();
        buttons.Controls.Add(backButton);

        _statusLabel = new Label
        {
            Name = "customerStatusLabel",
            Dock = DockStyle.Left,
            Width = 650,
            ForeColor = UiTheme.Muted,
            Text = isSecretary
                ? "Register owners, then search to view their animals."
                : "Search customers by ID or phone to view linked animals."
        };
        buttons.Controls.Add(_statusLabel);
    }

    private static Label CreateDockLabel(string text) => new()
    {
        Text = text,
        Dock = DockStyle.Fill,
        Font = UiTheme.LabelFont,
        ForeColor = UiTheme.Text,
        TextAlign = ContentAlignment.BottomLeft,
        Margin = new Padding(0, 0, 12, 0)
    };

    private static TextBox CreateDockTextBox(bool enabled) => new()
    {
        Dock = DockStyle.Fill,
        Enabled = enabled,
        Font = new Font("Segoe UI", 10.5F),
        BorderStyle = BorderStyle.FixedSingle,
        Margin = new Padding(0, 0, 12, 0)
    };

    private void AddCustomer()
    {
        Employee? currentUser = _services.AuthService.CurrentUser;
        if (currentUser is null)
        {
            MessageBox.Show(ValidationMessages.NotAuthenticated, "Customers",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        OperationResult<Customer> result = _services.CustomerService.RegisterCustomer(
            currentUser,
            _fullNameTextBox.Text,
            _identityTextBox.Text.Trim(),
            _phoneTextBox.Text.Trim(),
            _emailTextBox.Text.Trim());

        if (!result.IsSuccess)
        {
            _statusLabel.ForeColor = UiTheme.Danger;
            _statusLabel.Text = result.ErrorMessage;
            MessageBox.Show(result.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _statusLabel.ForeColor = UiTheme.Success;
        _statusLabel.Text = $"Customer {result.Value!.FullName} added successfully.";

        _fullNameTextBox.Clear();
        _identityTextBox.Clear();
        _phoneTextBox.Clear();
        _emailTextBox.Clear();

        _searchTextBox.Text = result.Value.IdentityNumber;
        DisplayCustomer(result.Value);
    }

    private void SearchCustomer()
    {
        string query = _searchTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(query))
        {
            MessageBox.Show("Enter an ID number or phone number.", "Search",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        OperationResult<Customer?> result = _services.CustomerService.SearchByIdentityOrPhone(
            _services.AuthService.CurrentUser,
            query);

        if (!result.IsSuccess)
        {
            _statusLabel.ForeColor = UiTheme.Danger;
            _statusLabel.Text = result.ErrorMessage;
            MessageBox.Show(result.ErrorMessage, "Permission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Customer? customer = result.Value;
        if (customer is null)
        {
            _detailsTextBox.Clear();
            _animalsList.Items.Clear();
            _statusLabel.ForeColor = UiTheme.Warning;
            _statusLabel.Text = "No customer found for that ID or phone number.";
            return;
        }

        DisplayCustomer(customer);
    }

    private void DisplayCustomer(Customer customer)
    {
        _detailsTextBox.Text =
            $"Name: {customer.FullName}\r\n" +
            $"ID: {customer.IdentityNumber}\r\n" +
            $"Phone: {customer.Phone}\r\n" +
            $"Email: {customer.Email}";

        _animalsList.Items.Clear();
        OperationResult<IReadOnlyList<Animal>> animalsResult = _services.CustomerService.GetCustomerAnimals(
            _services.AuthService.CurrentUser,
            customer.Id);

        if (!animalsResult.IsSuccess)
        {
            _statusLabel.ForeColor = UiTheme.Danger;
            _statusLabel.Text = animalsResult.ErrorMessage;
            MessageBox.Show(animalsResult.ErrorMessage, "Permission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        IReadOnlyList<Animal> animals = animalsResult.Value!;

        foreach (Animal animal in animals)
        {
            _animalsList.Items.Add($"{animal.Name} ({animal.Type}) - chip {animal.ChipNumber}");
        }

        _statusLabel.ForeColor = UiTheme.Muted;
        _statusLabel.Text = animals.Count == 0
            ? $"{customer.FullName} has no registered animals yet."
            : $"{customer.FullName} has {animals.Count} animal(s).";
    }
}
