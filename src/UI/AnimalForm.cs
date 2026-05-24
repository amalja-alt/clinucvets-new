using ClinicVets.Models;
using ClinicVets.Services;

namespace ClinicVets.UI;

public class AnimalForm : Form
{
    private readonly ClinicAppServices _services;
    private readonly TextBox _nameTextBox;
    private readonly TextBox _chipTextBox;
    private readonly ComboBox _categoryComboBox;
    private readonly NumericUpDown _weightInput;
    private readonly DateTimePicker _birthDatePicker;
    private readonly DateTimePicker _vaccinationDatePicker;
    private readonly ComboBox _ownerComboBox;
    private readonly TextBox _searchTextBox;
    private readonly ListBox _searchResultsList;
    private readonly TextBox _detailsTextBox;
    private readonly Label _statusLabel;
    private readonly Button _saveButton;

    public AnimalForm(ClinicAppServices services)
    {
        _services = services;

        UiTheme.ApplyForm(this);
        Text = "ClinicVets - Animals";
        ClientSize = new Size(1120, 720);
        MinimumSize = new Size(940, 640);

        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            Padding = new Padding(24),
            BackColor = UiTheme.Background
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 170));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        Controls.Add(root);

        root.Controls.Add(new Label
        {
            Text = "Animal Management",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = UiTheme.Text,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 0);

        RoundedPanel searchPanel = UiTheme.CreateCard(0, 0, 980, 150, 14);
        searchPanel.Dock = DockStyle.Fill;
        searchPanel.Padding = new Padding(20);
        searchPanel.Margin = new Padding(0, 0, 0, 16);
        root.Controls.Add(searchPanel, 0, 1);

        TableLayoutPanel searchLayout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 2
        };
        searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48));
        searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));
        searchLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
        searchLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        searchPanel.Controls.Add(searchLayout);

        _searchTextBox = CreateDockTextBox();
        _searchTextBox.PlaceholderText = "Search by name or microchip ID";
        searchLayout.Controls.Add(_searchTextBox, 0, 0);

        Button searchButton = UiTheme.CreateSecondaryButton("Search", 0, 0, 100, 36);
        searchButton.Dock = DockStyle.Fill;
        searchButton.Margin = new Padding(12, 0, 8, 8);
        searchButton.Click += (_, _) => SearchAnimals();
        searchLayout.Controls.Add(searchButton, 1, 0);

        Button showAllButton = UiTheme.CreateSecondaryButton("Show All", 0, 0, 100, 36);
        showAllButton.Anchor = AnchorStyles.Left;
        showAllButton.Margin = new Padding(0, 0, 0, 8);
        showAllButton.Click += (_, _) => LoadAllAnimals();
        searchLayout.Controls.Add(showAllButton, 2, 0);

        _searchResultsList = new ListBox
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 8, 12, 0),
            IntegralHeight = false
        };
        _searchResultsList.SelectedIndexChanged += (_, _) => LoadSelectedAnimal();
        searchLayout.Controls.Add(_searchResultsList, 0, 1);
        searchLayout.SetColumnSpan(_searchResultsList, 2);

        _detailsTextBox = new TextBox
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(12, 8, 0, 0),
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            BackColor = Color.FromArgb(248, 250, 252)
        };
        searchLayout.Controls.Add(_detailsTextBox, 2, 1);

        RoundedPanel formPanel = UiTheme.CreateCard(0, 0, 980, 300, 14);
        formPanel.Dock = DockStyle.Fill;
        formPanel.Padding = new Padding(20);
        formPanel.Margin = new Padding(0, 0, 0, 16);
        root.Controls.Add(formPanel, 0, 2);

        TableLayoutPanel formLayout = new()
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 4,
            RowCount = 4
        };
        for (int i = 0; i < 4; i++)
        {
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        }
        formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
        formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 92));
        formPanel.Controls.Add(formLayout);

        Label formTitle = new()
        {
            Text = "Register New Animal",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            ForeColor = UiTheme.Text
        };
        formLayout.Controls.Add(formTitle, 0, 0);
        formLayout.SetColumnSpan(formTitle, 4);

        formLayout.Controls.Add(CreateDockLabel("Name"), 0, 1);
        formLayout.Controls.Add(CreateDockLabel("Microchip ID"), 1, 1);
        formLayout.Controls.Add(CreateDockLabel("Type"), 2, 1);
        formLayout.Controls.Add(CreateDockLabel("Owner"), 3, 1);

        _nameTextBox = CreateDockTextBox();
        _chipTextBox = CreateDockTextBox();
        _categoryComboBox = new ComboBox
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 12, 10),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        _ownerComboBox = new ComboBox
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 0, 10),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        formLayout.Controls.Add(_nameTextBox, 0, 2);
        formLayout.Controls.Add(_chipTextBox, 1, 2);
        formLayout.Controls.Add(_categoryComboBox, 2, 2);
        formLayout.Controls.Add(_ownerComboBox, 3, 2);

        TableLayoutPanel lowerFields = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 2,
            Margin = new Padding(0, 12, 0, 0)
        };
        lowerFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        lowerFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5F));
        lowerFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5F));
        lowerFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
        lowerFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
        formLayout.Controls.Add(lowerFields, 0, 3);
        formLayout.SetColumnSpan(lowerFields, 4);

        lowerFields.Controls.Add(CreateDockLabel("Weight (kg)"), 0, 0);
        lowerFields.Controls.Add(CreateDockLabel("Birth date"), 1, 0);
        lowerFields.Controls.Add(CreateDockLabel("Last vaccination"), 2, 0);
        _weightInput = new NumericUpDown
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 12, 0),
            DecimalPlaces = 1,
            Minimum = 0.1m,
            Maximum = 100,
            Increment = 0.1m,
            Value = 1
        };
        _birthDatePicker = new DateTimePicker
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 12, 0),
            Format = DateTimePickerFormat.Short,
            MaxDate = DateTime.Today,
            MinDate = new DateTime(2000, 1, 1)
        };
        _vaccinationDatePicker = new DateTimePicker
        {
            Dock = DockStyle.Fill,
            Format = DateTimePickerFormat.Short,
            MaxDate = DateTime.Today
        };
        lowerFields.Controls.Add(_weightInput, 0, 1);
        lowerFields.Controls.Add(_birthDatePicker, 1, 1);
        lowerFields.Controls.Add(_vaccinationDatePicker, 2, 1);

        FlowLayoutPanel buttons = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 8, 0, 0)
        };
        root.Controls.Add(buttons, 0, 3);

        _saveButton = UiTheme.CreatePrimaryButton("Save Animal", 0, 0, 140, 42);
        _saveButton.Click += (_, _) => SaveAnimal();
        buttons.Controls.Add(_saveButton);

        Button newButton = UiTheme.CreateSecondaryButton("New / Clear", 0, 0, 130, 42);
        newButton.Click += (_, _) => ClearForm();
        buttons.Controls.Add(newButton);

        Button categoriesButton = UiTheme.CreateSecondaryButton("Manage Categories", 0, 0, 170, 42);
        categoriesButton.Click += (_, _) =>
        {
            using AnimalCategoryForm form = new(_services);
            form.ShowDialog(this);
            ReloadCategories();
        };
        buttons.Controls.Add(categoriesButton);

        Button backButton = UiTheme.CreateSecondaryButton("Back to Dashboard", 0, 0, 170, 42);
        backButton.Click += (_, _) => Close();
        buttons.Controls.Add(backButton);

        _statusLabel = new Label
        {
            Dock = DockStyle.Fill,
            ForeColor = UiTheme.Muted,
            Text = "Add a new animal or search and select one to edit."
        };
        root.Controls.Add(_statusLabel, 0, 4);

        ReloadCategories();
        ReloadOwners();
        LoadAllAnimals();
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

    private static TextBox CreateDockTextBox() => new()
    {
        Dock = DockStyle.Fill,
        Font = new Font("Segoe UI", 10.5F),
        BorderStyle = BorderStyle.FixedSingle,
        Margin = new Padding(0, 0, 12, 10)
    };

    private void ReloadCategories()
    {
        _categoryComboBox.DataSource = Enum.GetValues<AnimalType>();
    }

    private void ReloadOwners()
    {
        var owners = _services.LookupService.GetAllCustomers()
            .Select(customer => new OwnerListItem(customer.Id, $"{customer.FullName} ({customer.IdentityNumber})"))
            .ToList();

        _ownerComboBox.DisplayMember = nameof(OwnerListItem.Display);
        _ownerComboBox.ValueMember = nameof(OwnerListItem.Id);
        _ownerComboBox.DataSource = owners;
    }

    private void SearchAnimals()
    {
        string query = _searchTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(query))
        {
            MessageBox.Show("Enter a name or microchip ID to search.", "Search",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        BindSearchResults(_services.AnimalService.SearchByNameOrChip(query));
    }

    private void LoadAllAnimals() =>
        BindSearchResults(_services.LookupService.GetAllAnimals());

    private void BindSearchResults(IReadOnlyList<Animal> animals)
    {
        _searchResultsList.DataSource = null;
        _searchResultsList.Items.Clear();

        foreach (Animal animal in animals)
        {
            _searchResultsList.Items.Add(new AnimalListItem(animal));
        }

        _statusLabel.ForeColor = UiTheme.Muted;
        _statusLabel.Text = animals.Count == 0
            ? "No animals found."
            : $"{animals.Count} animal(s) found. Select one to view or edit.";
    }

    private void LoadSelectedAnimal()
    {
        if (_searchResultsList.SelectedItem is not AnimalListItem item)
        {
            return;
        }

        Animal animal = item.Animal;
        _nameTextBox.Text = animal.Name;
        _chipTextBox.Text = animal.ChipNumber;
        _categoryComboBox.SelectedItem = animal.Type;
        _weightInput.Value = Math.Clamp(animal.WeightKg, _weightInput.Minimum, _weightInput.Maximum);
        _birthDatePicker.Value = animal.BirthDate.ToDateTime(TimeOnly.MinValue);
        _vaccinationDatePicker.Value = animal.LastVaccinationDate.ToDateTime(TimeOnly.MinValue);
        _ownerComboBox.SelectedValue = animal.OwnerCustomerId;

        _saveButton.Enabled = false;
        _saveButton.Text = "Existing Animal";
        _detailsTextBox.Text = BuildDetailsText(animal);
        _statusLabel.ForeColor = UiTheme.Primary;
        _statusLabel.Text = $"Viewing {animal.Name} (ID {animal.Id}). Add/update is limited to new animals in this integration.";
    }

    private string BuildDetailsText(Animal animal)
    {
        string categoryName = animal.Type.ToString();

        Customer? owner = _services.LookupService.GetAllCustomers()
            .FirstOrDefault(customer => customer.Id == animal.OwnerCustomerId);
        string ownerText = owner is null
            ? "Unknown owner"
            : $"{owner.FullName} | {owner.Phone} | {owner.Email}";

        bool vaccineDue = _services.VaccineAlertService.IsVaccineDue(animal);
        string vaccineStatus = _services.VaccineAlertService.GetAlertMessage(animal);

        return
            $"ID: {animal.Id}\r\n" +
            $"Name: {animal.Name}\r\n" +
            $"Microchip: {animal.ChipNumber}\r\n" +
            $"Type: {categoryName}\r\n" +
            $"Weight: {animal.WeightKg:0.0} kg\r\n" +
            $"Birth date: {animal.BirthDate:dd/MM/yyyy}\r\n" +
            $"Last vaccination: {animal.LastVaccinationDate:dd/MM/yyyy}\r\n" +
            $"Vaccine status: {(vaccineDue ? "Due" : "OK")} — {vaccineStatus}\r\n" +
            $"Owner: {ownerText}";
    }

    private void SaveAnimal()
    {
        if (_categoryComboBox.SelectedItem is not AnimalType type ||
            _ownerComboBox.SelectedValue is not int ownerId)
        {
            MessageBox.Show("Select animal type and owner.", "Validation",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        DateOnly birthDate = DateOnly.FromDateTime(_birthDatePicker.Value);
        DateOnly vaccinationDate = DateOnly.FromDateTime(_vaccinationDatePicker.Value);

        OperationResult<Animal> result = _services.AnimalService.AddAnimal(
            _nameTextBox.Text,
            _chipTextBox.Text,
            type,
            _weightInput.Value,
            birthDate,
            vaccinationDate,
            ownerId);

        if (!result.IsSuccess)
        {
            MessageBox.Show(result.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _statusLabel.ForeColor = UiTheme.Success;
        _statusLabel.Text = $"Animal {result.Value!.Name} added successfully.";

        ClearForm(keepSearch: true);
        LoadAllAnimals();
    }

    private void ClearForm(bool keepSearch = false)
    {
        _nameTextBox.Clear();
        _chipTextBox.Clear();
        _weightInput.Value = 1;
        _birthDatePicker.Value = DateTime.Today.AddYears(-2);
        _vaccinationDatePicker.Value = DateTime.Today;
        if (_categoryComboBox.Items.Count > 0)
        {
            _categoryComboBox.SelectedIndex = 0;
        }

        if (_ownerComboBox.Items.Count > 0)
        {
            _ownerComboBox.SelectedIndex = 0;
        }

        _detailsTextBox.Clear();
        _saveButton.Enabled = true;
        _saveButton.Text = "Save Animal";
        if (!keepSearch)
        {
            _searchTextBox.Clear();
            _searchResultsList.Items.Clear();
        }

        _statusLabel.ForeColor = UiTheme.Muted;
        _statusLabel.Text = "Add a new animal or search and select one to edit.";
    }

    private sealed record OwnerListItem(int Id, string Display);

    private sealed class AnimalListItem
    {
        public AnimalListItem(Animal animal) => Animal = animal;

        public Animal Animal { get; }

        public override string ToString() => $"{Animal.Name} — {Animal.ChipNumber}";
    }
}
