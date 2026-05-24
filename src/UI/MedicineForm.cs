using ClinicVets.Models;
using ClinicVets.Services;

namespace ClinicVets.UI;

/// <summary>
/// Manages the clinic medicine inventory.
/// </summary>
public class MedicineForm : Form
{
    private readonly ClinicAppServices _services;
    private readonly TextBox _nameTextBox;
    private readonly NumericUpDown _priceInput;
    private readonly NumericUpDown _quantityInput;
    private readonly DataGridView _medicineGrid;
    private readonly Label _statusLabel;

    public MedicineForm(ClinicAppServices services)
    {
        _services = services;

        UiTheme.ApplyForm(this);
        Text = "ClinicVets - Medicine Inventory";
        ClientSize = new Size(760, 520);
        MinimumSize = new Size(640, 480);

        Panel card = UiTheme.CreateCard(24, 24, 712, 472);
        card.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        Controls.Add(card);

        Label title = UiTheme.CreateTitle("Medicine Inventory", 24, 20, 400);
        card.Controls.Add(title);

        card.Controls.Add(UiTheme.CreateFieldLabel("Medicine name", 24, 70));
        _nameTextBox = new TextBox
        {
            Location = new Point(24, 92),
            Size = new Size(220, 28)
        };
        card.Controls.Add(_nameTextBox);

        card.Controls.Add(UiTheme.CreateFieldLabel("Price (₪)", 264, 70));
        _priceInput = new NumericUpDown
        {
            Location = new Point(264, 92),
            Size = new Size(120, 28),
            DecimalPlaces = 2,
            Maximum = 100000,
            Minimum = 0
        };
        card.Controls.Add(_priceInput);

        card.Controls.Add(UiTheme.CreateFieldLabel("Quantity in stock", 404, 70));
        _quantityInput = new NumericUpDown
        {
            Location = new Point(404, 92),
            Size = new Size(120, 28),
            Maximum = 100000,
            Minimum = 0
        };
        card.Controls.Add(_quantityInput);

        Button addButton = UiTheme.CreatePrimaryButton("Add Medicine", 544, 88, 144);
        addButton.Click += (_, _) => AddMedicine();
        card.Controls.Add(addButton);

        Button deleteButton = UiTheme.CreateDangerButton("Delete Selected", 544, 132, 144);
        deleteButton.Click += (_, _) => DeleteSelectedMedicine();
        card.Controls.Add(deleteButton);

        Button refreshButton = UiTheme.CreateSecondaryButton("Refresh", 544, 176, 144);
        refreshButton.Click += (_, _) => LoadMedicines();
        card.Controls.Add(refreshButton);

        Button backButton = UiTheme.CreateSecondaryButton("Back to Dashboard", 544, 220, 144);
        backButton.Click += (_, _) => Close();
        card.Controls.Add(backButton);

        _medicineGrid = new DataGridView
        {
            Location = new Point(24, 140),
            Size = new Size(500, 280),
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            RowHeadersVisible = false
        };
        _medicineGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", FillWeight = 40 });
        _medicineGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name", FillWeight = 120 });
        _medicineGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Price", FillWeight = 60 });
        _medicineGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Quantity",
            HeaderText = "Quantity",
            FillWeight = 60
        });
        card.Controls.Add(_medicineGrid);

        _statusLabel = new Label
        {
            Location = new Point(24, 430),
            Size = new Size(660, 24),
            ForeColor = UiTheme.Muted,
            Text = "Manage medicines used during visits."
        };
        card.Controls.Add(_statusLabel);

        LoadMedicines();
    }

    private void LoadMedicines()
    {
        _medicineGrid.Rows.Clear();
        foreach (Medicine medicine in _services.MedicineService.GetAllMedicines())
        {
            _medicineGrid.Rows.Add(medicine.Id, medicine.Name, medicine.Price.ToString("0.00"), medicine.QuantityInStock);
        }

        _statusLabel.Text = $"{_medicineGrid.Rows.Count} medicine(s) in inventory.";
    }

    private void AddMedicine()
    {
        OperationResult<Medicine> result = _services.MedicineService.AddMedicine(
            _nameTextBox.Text,
            _priceInput.Value,
            (int)_quantityInput.Value);

        if (!result.IsSuccess)
        {
            MessageBox.Show(result.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _nameTextBox.Clear();
        _priceInput.Value = 0;
        _quantityInput.Value = 0;
        LoadMedicines();
        _statusLabel.ForeColor = UiTheme.Success;
        _statusLabel.Text = $"Added {result.Value!.Name} successfully.";
    }

    private void DeleteSelectedMedicine()
    {
        if (_medicineGrid.CurrentRow is null)
        {
            MessageBox.Show("Select a medicine to delete.", "Medicine Inventory",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        int medicineId = Convert.ToInt32(_medicineGrid.CurrentRow.Cells["Id"].Value);
        string medicineName = _medicineGrid.CurrentRow.Cells["Name"].Value?.ToString() ?? "medicine";

        DialogResult confirm = MessageBox.Show(
            $"Delete {medicineName}?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        if (!_services.MedicineService.RemoveMedicine(medicineId))
        {
            MessageBox.Show("Medicine could not be deleted.", "Medicine Inventory",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        LoadMedicines();
        _statusLabel.ForeColor = UiTheme.Warning;
        _statusLabel.Text = $"{medicineName} removed from inventory.";
    }
}
