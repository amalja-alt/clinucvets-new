using ClinicVets.Models;
using ClinicVets.Services;

namespace ClinicVets.UI;

/// <summary>
/// Veterinarian dashboard focused on medical workflow and patient treatment.
/// </summary>
public class VeterinarianDashboardForm : Form
{
    private readonly ClinicAppServices _services;
    private readonly string _username;
    private readonly FlowLayoutPanel _patientsList = new();
    private readonly FlowLayoutPanel _upcomingList = new();
    private readonly TextBox _medicalNotesTextBox = new();
    private readonly ComboBox _statusFilter = new();
    private readonly TextBox _searchBox = new();
    private readonly List<PatientVisitItem> _patients = [];
    private readonly Panel _mainContentPanel = new();
    private readonly Dictionary<string, RoundedPanel> _navItems = [];
    private Control? _currentView;

    public VeterinarianDashboardForm(ClinicAppServices services, string? welcomeMessage = null)
    {
        _services = services;
        _username = _services.AuthService.CurrentUser?.Username ?? "Doctor";

        UiTheme.ApplyForm(this);
        Text = "ClinicVets - Veterinarian Dashboard";
        ClientSize = new Size(1360, 820);
        MinimumSize = new Size(880, 640);
        StartPosition = FormStartPosition.CenterScreen;

        BuildShell();
        LoadPatientData();
        ShowHomeView(welcomeMessage);
    }

    private void BuildShell()
    {
        TableLayoutPanel shell = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            BackColor = Color.FromArgb(245, 248, 251),
            BackgroundImage = Image.FromFile(UiTheme.ImagePasth + "bg.jpg")
        };

        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        Controls.Add(shell);

        Control sidebar = CreateSidebar();
        shell.Controls.Add(sidebar, 0, 0);
        shell.SetRowSpan(sidebar, 2);
        shell.Controls.Add(CreateTopBar(), 1, 0);

        _mainContentPanel.Dock = DockStyle.Fill;
        _mainContentPanel.BackColor = Color.FromArgb(245, 248, 251);
        shell.Controls.Add(_mainContentPanel, 1, 1);
    }

    private Control CreateSidebar()
    {
        Panel sidebar = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(70, 245, 248, 251),
            Padding = new Padding(18)
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3
        };

        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 74));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 74));

        sidebar.Controls.Add(layout);

        PictureBox brand = new()
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.StretchImage,
            Image = Image.FromFile(UiTheme.ImagePasth + "logo.png")
        };

        layout.Controls.Add(brand, 0, 0);

        FlowLayoutPanel nav = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };

        layout.Controls.Add(nav, 0, 1);

        nav.Controls.Add(CreateNavItem("Dashboard", "D", true, () => ShowHomeView()));
        nav.Controls.Add(CreateNavItem("My Appointments", "A", false, () => ShowHomeView()));
        nav.Controls.Add(CreateNavItem("Patients", "P", false, () => OpenEmbeddedForm("Patients", () => new AnimalForm(_services))));
        nav.Controls.Add(CreateNavItem("Medical Records", "M", false, () => OpenEmbeddedForm("Medical Records", () => new AnimalForm(_services))));
        nav.Controls.Add(CreateNavItem("Treatments", "T", false, () => OpenEmbeddedForm("Treatments", () => new VisitForm(_services))));
        nav.Controls.Add(CreateNavItem("Prescriptions", "Rx", false, () => OpenEmbeddedForm("Prescriptions", () => new MedicineForm(_services))));

        Button logout = UiTheme.CreateSecondaryButton("Logout", 0, 0, 188, 42);

        logout.Dock = DockStyle.Bottom;

        logout.Click += (_, _) =>
        {
            _services.AuthService.Logout();
            DialogResult = DialogResult.OK;
            Close();
        };

        layout.Controls.Add(logout, 0, 2);

        return sidebar;
    }
}