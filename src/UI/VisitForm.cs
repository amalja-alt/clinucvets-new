using ClinicVets.Models;
using ClinicVets.Services;

namespace ClinicVets.UI;

public class VisitForm : Form
{
    private readonly ClinicAppServices _services;
    private readonly ComboBox _animalComboBox;
    private readonly TextBox _diagnosisTextBox;
    private readonly TextBox _treatmentNotesTextBox;
    private readonly TextBox _veterinarianTextBox;
    private readonly DateTimePicker _visitDatePicker;
    private readonly ComboBox _medicineComboBox;
    private readonly ListBox _selectedMedicinesList;
    private readonly Label _totalPriceLabel;
    private readonly Label _vaccineAlertLabel;
    private readonly List<int> _selectedMedicineIds = [];

    public VisitForm(ClinicAppServices services)
    {
        _services = services;

        UiTheme.ApplyForm(this);
        Text = "ClinicVets - Visit & Treatment";
        ClientSize = new Size(820, 600);
        MinimumSize = new Size(680, 520);

        Panel card = UiTheme.CreateCard(24, 24, 772, 552);
        card.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        Controls.Add(card);

        card.Controls.Add(UiTheme.CreateTitle("Visit & Treatment", 24, 20, 400));

        card.Controls.Add(UiTheme.CreateFieldLabel("Animal", 24, 68));
        _animalComboBox = new ComboBox
        {
            Location = new Point(24, 90),
            Size = new Size(240, 28),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        _animalComboBox.SelectedIndexChanged += (_, _) => UpdateVaccineAlert();
        card.Controls.Add(_animalComboBox);

        card.Controls.Add(UiTheme.CreateFieldLabel("Visit date", 288, 68));
        _visitDatePicker = new DateTimePicker
        {
            Location = new Point(288, 90),
            Size = new Size(180, 28),
            Format = DateTimePickerFormat.Short
        };
        card.Controls.Add(_visitDatePicker);

        card.Controls.Add(UiTheme.CreateFieldLabel("Veterinarian", 492, 68));
        _veterinarianTextBox = new TextBox
        {
            Location = new Point(492, 90),
            Size = new Size(256, 28),
            Text = "Dr. Avi Cohen"
        };
        card.Controls.Add(_veterinarianTextBox);

        card.Controls.Add(UiTheme.CreateFieldLabel("Diagnosis", 24, 132));
        _diagnosisTextBox = new TextBox
        {
            Location = new Point(24, 154),
            Size = new Size(350, 56),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical
        };
        card.Controls.Add(_diagnosisTextBox);

        card.Controls.Add(UiTheme.CreateFieldLabel("Treatment notes", 398, 132));
        _treatmentNotesTextBox = new TextBox
        {
            Location = new Point(398, 154),
            Size = new Size(350, 56),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical
        };
        card.Controls.Add(_treatmentNotesTextBox);

        _vaccineAlertLabel = new Label
        {
            Location = new Point(24, 220),
            Size = new Size(724, 24),
            ForeColor = UiTheme.Muted,
            Text = "Select an animal to check yearly vaccination status."
        };
        card.Controls.Add(_vaccineAlertLabel);

        card.Controls.Add(UiTheme.CreateFieldLabel("Medicine", 24, 256));
        _medicineComboBox = new ComboBox
        {
            Location = new Point(24, 278),
            Size = new Size(280, 28),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        card.Controls.Add(_medicineComboBox);

        Button addMedicineButton = UiTheme.CreateSecondaryButton("Add to Visit", 320, 274, 130);
        addMedicineButton.Click += (_, _) => AddMedicineToVisit();
        card.Controls.Add(addMedicineButton);

        card.Controls.Add(UiTheme.CreateFieldLabel("Medicines for this visit", 24, 320));
        _selectedMedicinesList = new ListBox
        {
            Location = new Point(24, 342),
            Size = new Size(420, 108),
            IntegralHeight = false
        };
        card.Controls.Add(_selectedMedicinesList);

        _totalPriceLabel = new Label
        {
            Location = new Point(468, 342),
            Size = new Size(280, 32),
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = UiTheme.Primary,
            Text = "Total Price: ₪0.00"
        };
        card.Controls.Add(_totalPriceLabel);

        Button saveVisitButton = UiTheme.CreatePrimaryButton("Save Visit", 468, 388, 160);
        saveVisitButton.Click += (_, _) => SaveVisit();
        card.Controls.Add(saveVisitButton);

        Button backButton = UiTheme.CreateSecondaryButton("Back to Dashboard", 468, 432, 160);
        backButton.Click += (_, _) => Close();
        card.Controls.Add(backButton);

        LoadAnimals();
        LoadMedicines();
        UpdateTotalPrice();
    }

    private void LoadAnimals()
    {
        _animalComboBox.DisplayMember = nameof(Animal.Name);
        _animalComboBox.ValueMember = nameof(Animal.Id);
        _animalComboBox.DataSource = _services.LookupService.GetAllAnimals().ToList();
    }

    private void LoadMedicines()
    {
        _medicineComboBox.DisplayMember = nameof(Medicine.Name);
        _medicineComboBox.ValueMember = nameof(Medicine.Id);
        _medicineComboBox.DataSource = _services.MedicineService.GetAllMedicines().ToList();
    }

    private void UpdateVaccineAlert()
    {
        if (_animalComboBox.SelectedItem is not Animal animal)
        {
            _vaccineAlertLabel.ForeColor = UiTheme.Muted;
            _vaccineAlertLabel.Text = "Select an animal to check yearly vaccination status.";
            return;
        }

        bool isDue = _services.VaccineAlertService.IsVaccineDue(animal);
        _vaccineAlertLabel.ForeColor = isDue ? UiTheme.Warning : UiTheme.Success;
        _vaccineAlertLabel.Text = _services.VaccineAlertService.GetAlertMessage(animal);

        if (isDue)
        {
            MessageBox.Show(
                $"Vaccine alert for {animal.Name}:\n{_vaccineAlertLabel.Text}",
                "Yearly Vaccination",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    private void AddMedicineToVisit()
    {
        if (_medicineComboBox.SelectedItem is not Medicine medicine)
        {
            MessageBox.Show("Select a medicine to add.", "Visit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_selectedMedicineIds.Contains(medicine.Id))
        {
            MessageBox.Show("Medicine already added to this visit.", "Visit",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        _selectedMedicineIds.Add(medicine.Id);
        _selectedMedicinesList.Items.Add($"{medicine.Name} — ₪{medicine.Price:0.00}");
        UpdateTotalPrice();
    }

    private void UpdateTotalPrice()
    {
        decimal medicinesTotal = _services.MedicineService
            .GetAllMedicines()
            .Where(medicine => _selectedMedicineIds.Contains(medicine.Id))
            .Sum(medicine => medicine.Price);

        Visit preview = new() { BaseVisitPrice = 150m };
        preview.MedicinesGiven.AddRange(
            _services.MedicineService.GetAllMedicines().Where(m => _selectedMedicineIds.Contains(m.Id)));

        decimal total = preview.BaseVisitPrice + medicinesTotal;
        _totalPriceLabel.Text = $"Total Price: ₪{total:0.00}";
    }

    private void SaveVisit()
    {
        if (_animalComboBox.SelectedValue is not int animalId)
        {
            MessageBox.Show("Select an animal.", "Visit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Employee? currentUser = _services.AuthService.CurrentUser;
        if (currentUser is null)
        {
            MessageBox.Show(ValidationMessages.NotAuthenticated, "Visit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        OperationResult<Visit> result = _services.VisitService.OpenVisit(
            currentUser,
            animalId,
            _treatmentNotesTextBox.Text,
            _diagnosisTextBox.Text,
            _selectedMedicineIds);

        if (!result.IsSuccess)
        {
            MessageBox.Show(result.ErrorMessage, "Visit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show(
            $"Visit saved successfully.\nTotal: ₪{result.Value!.TotalPrice:0.00}",
            "Visit Saved",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        _diagnosisTextBox.Clear();
        _treatmentNotesTextBox.Clear();
        _selectedMedicineIds.Clear();
        _selectedMedicinesList.Items.Clear();
        UpdateTotalPrice();
    }
}
