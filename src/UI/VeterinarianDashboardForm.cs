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
<<<<<<< HEAD
<<<<<<< HEAD
            BackColor = Color.FromArgb(245, 248, 251),
            BackgroundImage = Image.FromFile(UiTheme.ImagePasth + "bg.jpg")
        };
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));
=======
            BackColor = Color.FromArgb(245, 248, 251)
        };
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 236));
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
            BackColor = Color.FromArgb(245, 248, 251),
            BackgroundImage = Image.FromFile(UiTheme.ImagePasth + "bg.jpg")
        };
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));
>>>>>>> main
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
<<<<<<< HEAD
<<<<<<< HEAD
            BackColor = Color.FromArgb(70, 245, 248, 251),
=======
            BackColor = Color.White,
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
            BackColor = Color.FromArgb(70, 245, 248, 251),
>>>>>>> main
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

<<<<<<< HEAD
<<<<<<< HEAD
        PictureBox brand = new()
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.StretchImage,


            Image = Image.FromFile(UiTheme.ImagePasth + "logo.png")
=======
        Label brand = new()
=======
        PictureBox brand = new()
>>>>>>> main
        {
            Dock = DockStyle.Fill,
<<<<<<< HEAD
            Font = new Font("Segoe UI", 20F, FontStyle.Bold),
            ForeColor = Color.FromArgb(20, 84, 132),
            TextAlign = ContentAlignment.MiddleLeft
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
            SizeMode = PictureBoxSizeMode.StretchImage,


            Image = Image.FromFile(UiTheme.ImagePasth + "logo.png")
>>>>>>> main
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

    private Control CreateNavItem(string text, string icon, bool active, Action action)
    {
        RoundedPanel item = new()
        {
            Width = 196,
            Height = 46,
            Margin = new Padding(0, 0, 0, 8),
            BackColor = active ? Color.FromArgb(225, 245, 250) : Color.White,
            BorderSize = active ? 1 : 0,
            BorderColor = Color.FromArgb(183, 230, 237),
            CornerRadius = 10,
            Cursor = Cursors.Hand,
            Tag = text
        };
        item.Click += (_, _) => action();

        Label iconLabel = new()
        {
            Text = icon,
            Location = new Point(12, 9),
            Size = new Size(30, 28),
            BackColor = active ? Color.FromArgb(20, 150, 170) : Color.FromArgb(229, 236, 244),
            ForeColor = active ? Color.White : Color.FromArgb(78, 99, 120),
            Font = new Font("Segoe UI", 8.8F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        };
        iconLabel.Click += (_, _) => action();
        item.Controls.Add(iconLabel);

        Label label = new()
        {
            Text = text,
            Location = new Point(52, 10),
            Size = new Size(138, 26),
            Font = new Font("Segoe UI", 10.5F, active ? FontStyle.Bold : FontStyle.Regular),
            ForeColor = active ? Color.FromArgb(20, 84, 132) : Color.FromArgb(65, 78, 92),
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true
        };
        label.Click += (_, _) => action();
        item.Controls.Add(label);

        _navItems[text] = item;
        return item;
    }

    private Control CreateTopBar()
    {
        Panel topBar = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(24, 14, 24, 14)
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 340));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        topBar.Controls.Add(layout);

        layout.Controls.Add(new Label
        {
            Text = "Veterinarian Dashboard",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            ForeColor = UiTheme.Text,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 0);

        _searchBox.Dock = DockStyle.Fill;
        _searchBox.Font = new Font("Segoe UI", 10.5F);
        _searchBox.PlaceholderText = "Search patient or owner";
        _searchBox.Margin = new Padding(0, 5, 14, 5);
        _searchBox.TextChanged += (_, _) => RefreshPatients();
        layout.Controls.Add(_searchBox, 1, 0);

        layout.Controls.Add(new Label
        {
            Text = $"{_username} (Veterinarian)",
            AutoSize = true,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(54, 73, 94),
            TextAlign = ContentAlignment.MiddleRight,
            Margin = new Padding(0, 0, 16, 0)
        }, 2, 0);

        Button logout = UiTheme.CreateSecondaryButton("Logout", 0, 0, 94, 38);
        logout.Dock = DockStyle.Fill;
        logout.Margin = new Padding(0, 4, 0, 4);
        logout.Click += (_, _) =>
        {
            _services.AuthService.Logout();
            DialogResult = DialogResult.OK;
            Close();
        };
        layout.Controls.Add(logout, 3, 0);

        return topBar;
    }

    private void ShowHomeView(string? welcomeMessage = null)
    {
        SetActiveNav("Dashboard");
        OpenView(CreateContent(welcomeMessage));
        RefreshPatients();
    }

    private Control CreateContent(string? welcomeMessage)
    {
        Panel scrollHost = new()
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Color.FromArgb(245, 248, 251),
            Padding = new Padding(24)
        };

        TableLayoutPanel content = new()
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            RowCount = 5
        };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        scrollHost.Controls.Add(content);

        content.Controls.Add(CreateWelcomeSection(welcomeMessage), 0, 0);
        content.Controls.Add(CreatePatientsSection(), 0, 1);
        content.Controls.Add(CreateQuickActionsSection(), 0, 2);
        content.Controls.Add(CreateMedicalWorkspace(), 0, 3);
        content.Controls.Add(CreateUpcomingSection(), 0, 4);

        return scrollHost;
    }

    private Control CreateWelcomeSection(string? welcomeMessage)
    {
        RoundedPanel panel = new()
        {
            Dock = DockStyle.Top,
            Height = 116,
            BackColor = Color.FromArgb(20, 150, 170),
            GradientEndColor = Color.FromArgb(50, 126, 210),
            BorderSize = 0,
            CornerRadius = 16,
            Margin = new Padding(0, 0, 0, 18),
            Padding = new Padding(26, 18, 26, 18)
        };

        panel.Controls.Add(new Label
        {
            Text = $"Welcome back, Dr. {_username}",
            Dock = DockStyle.Top,
            Height = 48,
            Font = new Font("Segoe UI", 24F, FontStyle.Bold),
            ForeColor = Color.White,
            AutoEllipsis = true
        });
        panel.Controls.Add(new Label
        {
            Text = string.IsNullOrWhiteSpace(welcomeMessage) ? "Review today's patients and continue medical visits" : welcomeMessage,
            Dock = DockStyle.Bottom,
            Height = 34,
            Font = new Font("Segoe UI", 12F),
            ForeColor = Color.FromArgb(225, 245, 250),
            AutoEllipsis = true
        });

        return panel;
    }

    private Control CreatePatientsSection()
    {
        RoundedPanel panel = CreateSectionPanel();
        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 1,
            RowCount = 2
        };
        panel.Controls.Add(layout);

        TableLayoutPanel header = new()
        {
            Dock = DockStyle.Top,
            Height = 52,
            ColumnCount = 3
        };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        layout.Controls.Add(header, 0, 0);

        header.Controls.Add(CreateSectionTitle("Today's Patients"), 0, 0);
        _statusFilter.Dock = DockStyle.Fill;
        _statusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        _statusFilter.Margin = new Padding(0, 8, 12, 8);
        _statusFilter.Items.AddRange(["All Statuses", "Waiting", "In Progress", "Completed", "Emergency"]);
        _statusFilter.SelectedIndex = 0;
        _statusFilter.SelectedIndexChanged += (_, _) => RefreshPatients();
        header.Controls.Add(_statusFilter, 1, 0);

        Button refresh = UiTheme.CreateSecondaryButton("Refresh", 0, 0, 130, 36);
        refresh.Dock = DockStyle.Fill;
        refresh.Margin = new Padding(0, 8, 0, 8);
        refresh.Click += (_, _) => RefreshPatients();
        header.Controls.Add(refresh, 2, 0);

        _patientsList.Dock = DockStyle.Top;
        _patientsList.AutoSize = true;
        _patientsList.FlowDirection = FlowDirection.TopDown;
        _patientsList.WrapContents = false;
        layout.Controls.Add(_patientsList, 0, 1);

        return panel;
    }

    private Control CreateQuickActionsSection()
    {
        RoundedPanel panel = CreateSectionPanel();
        panel.Controls.Add(CreateSectionTitle("Quick Actions"));

        FlowLayoutPanel actions = new()
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            WrapContents = true
        };
        panel.Controls.Add(actions);
        actions.BringToFront();

        actions.Controls.Add(CreateQuickAction("Start Visit", Color.FromArgb(50, 126, 210), () => OpenEmbeddedForm("Treatments", () => new VisitForm(_services))));
        actions.Controls.Add(CreateQuickAction("Add Diagnosis", Color.FromArgb(20, 150, 170), null));
        actions.Controls.Add(CreateQuickAction("Add Prescription", Color.FromArgb(116, 101, 220), () => OpenEmbeddedForm("Prescriptions", () => new MedicineForm(_services))));
        actions.Controls.Add(CreateQuickAction("Complete Visit", Color.FromArgb(63, 176, 112), null));
        actions.Controls.Add(CreateQuickAction("Open Medical Record", Color.FromArgb(235, 154, 42), () => OpenEmbeddedForm("Medical Records", () => new AnimalForm(_services))));

        return panel;
    }

    private Control CreateMedicalWorkspace()
    {
        TableLayoutPanel workspace = new()
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = 1,
            Margin = new Padding(0, 0, 0, 18)
        };
        workspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48));
        workspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));

        RoundedPanel info = CreateSectionPanel();
        info.Margin = new Padding(0, 0, 18, 0);
        info.Controls.Add(CreateSectionTitle("Patient Medical Information"));
        FlowLayoutPanel infoList = new()
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false
        };
        info.Controls.Add(infoList);
        infoList.BringToFront();
        infoList.Controls.Add(CreateInfoRow("Medical history", "Previous visits, conditions, and observations"));
        infoList.Controls.Add(CreateInfoRow("Vaccinations", "Yearly vaccination status and reminders"));
        infoList.Controls.Add(CreateInfoRow("Previous visits", "Diagnosis and treatments from earlier appointments"));
        infoList.Controls.Add(CreateInfoRow("Prescriptions", "Medication plan and dosage notes"));

        RoundedPanel notes = CreateSectionPanel();
        notes.Controls.Add(CreateSectionTitle("Medical Notes"));
        _medicalNotesTextBox.Dock = DockStyle.Top;
        _medicalNotesTextBox.Height = 190;
        _medicalNotesTextBox.Multiline = true;
        _medicalNotesTextBox.ScrollBars = ScrollBars.Vertical;
        _medicalNotesTextBox.Font = new Font("Segoe UI", 10.5F);
        _medicalNotesTextBox.PlaceholderText = "Write diagnosis, treatment notes, observations, and follow-up instructions...";
        notes.Controls.Add(_medicalNotesTextBox);
        _medicalNotesTextBox.BringToFront();

        workspace.Controls.Add(info, 0, 0);
        workspace.Controls.Add(notes, 1, 0);
        return workspace;
    }

    private Control CreateUpcomingSection()
    {
        RoundedPanel panel = CreateSectionPanel();
        panel.Controls.Add(CreateSectionTitle("Upcoming Visits"));
        _upcomingList.Dock = DockStyle.Top;
        _upcomingList.AutoSize = true;
        _upcomingList.FlowDirection = FlowDirection.TopDown;
        _upcomingList.WrapContents = false;
        panel.Controls.Add(_upcomingList);
        _upcomingList.BringToFront();
        return panel;
    }

    private static RoundedPanel CreateSectionPanel()
    {
        return new RoundedPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            BackColor = Color.White,
            BorderColor = UiTheme.Border,
            BorderSize = 1,
            CornerRadius = 16,
            Margin = new Padding(0, 0, 0, 18),
            Padding = new Padding(20)
        };
    }

    private static Label CreateSectionTitle(string text)
    {
        return new Label
        {
            Text = text,
            Dock = DockStyle.Top,
            Height = 40,
            Font = new Font("Segoe UI", 17F, FontStyle.Bold),
            ForeColor = UiTheme.Text
        };
    }

    private static Control CreateQuickAction(string title, Color color, Action? action)
    {
        RoundedPanel card = new()
        {
            Width = 210,
            Height = 82,
            Margin = new Padding(0, 0, 14, 14),
            BackColor = Color.FromArgb(247, 251, 253),
            BorderColor = Color.FromArgb(221, 232, 240),
            BorderSize = 1,
            CornerRadius = 14,
            Cursor = action is null ? Cursors.Default : Cursors.Hand
        };
        card.Click += (_, _) => action?.Invoke();
        card.Controls.Add(new Panel
        {
            Location = new Point(16, 22),
            Size = new Size(38, 38),
            BackColor = color
        });
        card.Controls.Add(new Label
        {
            Text = title,
            Location = new Point(68, 13),
            Size = new Size(124, 56),
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            ForeColor = UiTheme.Text,
            TextAlign = ContentAlignment.MiddleLeft
        });
        return card;
    }

    private static Control CreateInfoRow(string title, string detail)
    {
        RoundedPanel row = new()
        {
            Width = 460,
            Height = 64,
            Margin = new Padding(0, 8, 0, 0),
            BackColor = Color.FromArgb(247, 251, 253),
            BorderColor = Color.FromArgb(228, 235, 242),
            BorderSize = 1,
            CornerRadius = 10
        };
        row.Controls.Add(new Label
        {
            Text = title,
            Location = new Point(16, 8),
            Size = new Size(420, 22),
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = UiTheme.Text
        });
        row.Controls.Add(new Label
        {
            Text = detail,
            Location = new Point(16, 32),
            Size = new Size(420, 22),
            Font = new Font("Segoe UI", 9F),
            ForeColor = UiTheme.Muted,
            AutoEllipsis = true
        });
        return row;
    }

    private void LoadPatientData()
    {
        _patients.Clear();
        _patients.AddRange([
            new PatientVisitItem("09:00", "Buddy", "Dana Levi", "Annual checkup", "Waiting"),
            new PatientVisitItem("09:30", "Luna", "Noam Cohen", "Vaccination", "In Progress"),
            new PatientVisitItem("10:15", "Kiwi", "Maya Amir", "Emergency breathing concern", "Emergency"),
            new PatientVisitItem("11:00", "Max", "Sarah Johnson", "Dental exam", "Completed"),
            new PatientVisitItem("12:30", "Rocky", "Yoni Bar", "Follow-up", "Waiting")
        ]);

        _upcomingList.Controls.Clear();
        _upcomingList.Controls.Add(CreateUpcomingVisit("13:00", "Bella", "Skin irritation"));
        _upcomingList.Controls.Add(CreateUpcomingVisit("13:30", "Charlie", "Post-surgery check"));
        _upcomingList.Controls.Add(CreateUpcomingVisit("14:15", "Milo", "Vaccination"));
    }

    private void RefreshPatients()
    {
        string query = _searchBox.Text.Trim();
        string status = _statusFilter.SelectedItem?.ToString() ?? "All Statuses";
        IEnumerable<PatientVisitItem> filtered = _patients;

        if (!string.IsNullOrWhiteSpace(query))
        {
            filtered = filtered.Where(item =>
                item.PetName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                item.OwnerName.Contains(query, StringComparison.OrdinalIgnoreCase));
        }

        if (status != "All Statuses")
        {
            filtered = filtered.Where(item => item.Status == status);
        }

        _patientsList.Controls.Clear();
        foreach (PatientVisitItem item in filtered)
        {
            _patientsList.Controls.Add(CreatePatientRow(item));
        }
    }

    private Control CreatePatientRow(PatientVisitItem item)
    {
        RoundedPanel row = new()
        {
            Width = 1040,
            Height = 76,
            Margin = new Padding(0, 8, 0, 0),
            BackColor = Color.FromArgb(248, 251, 253),
            BorderColor = Color.FromArgb(229, 236, 243),
            BorderSize = 1,
            CornerRadius = 12
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 6,
            RowCount = 1,
            Padding = new Padding(14, 10, 14, 10)
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 78));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250));
        row.Controls.Add(layout);

        layout.Controls.Add(CreateCell(item.Time, FontStyle.Bold), 0, 0);
        layout.Controls.Add(CreateCell(item.PetName, FontStyle.Bold), 1, 0);
        layout.Controls.Add(CreateCell(item.OwnerName, FontStyle.Regular), 2, 0);
        layout.Controls.Add(CreateCell(item.VisitReason, FontStyle.Regular), 3, 0);
        layout.Controls.Add(CreateStatusBadge(item.Status), 4, 0);

        FlowLayoutPanel actions = new()
        {
            Dock = DockStyle.Fill,
            WrapContents = false
        };
        actions.Controls.Add(CreateSmallButton("Start", () => UpdatePatientStatus(item, "In Progress")));
        actions.Controls.Add(CreateSmallButton("Record", null));
        actions.Controls.Add(CreateSmallButton("Done", () => UpdatePatientStatus(item, "Completed")));
        layout.Controls.Add(actions, 5, 0);
        return row;
    }

    private static Control CreateUpcomingVisit(string time, string pet, string reason)
    {
        return new Label
        {
            Text = $"{time}   {pet}   {reason}",
            Width = 980,
            Height = 34,
            Margin = new Padding(0, 8, 0, 0),
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = Color.FromArgb(20, 84, 132),
            BackColor = Color.FromArgb(247, 251, 253),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(12, 0, 0, 0)
        };
    }

    private static Label CreateCell(string text, FontStyle style)
    {
        return new Label
        {
            Text = text,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10F, style),
            ForeColor = UiTheme.Text,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true,
            Margin = new Padding(0)
        };
    }

    private static Label CreateStatusBadge(string status)
    {
        Color fore = status switch
        {
            "Waiting" => Color.FromArgb(20, 150, 170),
            "In Progress" => Color.FromArgb(50, 126, 210),
            "Completed" => Color.FromArgb(63, 176, 112),
            "Emergency" => Color.FromArgb(229, 92, 84),
            _ => UiTheme.Muted
        };

        return new Label
        {
            Text = status,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = fore,
            BackColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter,
            Margin = new Padding(0, 12, 10, 12)
        };
    }

    private static Button CreateSmallButton(string text, Action? action)
    {
        Button button = UiTheme.CreateSecondaryButton(text, 0, 0, 68, 32);
        button.Margin = new Padding(0, 12, 6, 10);
        button.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        if (action is not null)
        {
            button.Click += (_, _) => action();
        }

        return button;
    }

    private void UpdatePatientStatus(PatientVisitItem item, string status)
    {
        item.Status = status;
        RefreshPatients();
    }

    private void OpenEmbeddedForm(string navText, Func<Form> createForm)
    {
        SetActiveNav(navText);
        Form form = createForm();
        form.TopLevel = false;
        form.FormBorderStyle = FormBorderStyle.None;
        form.Dock = DockStyle.Fill;
        form.StartPosition = FormStartPosition.Manual;
        form.AutoScroll = true;
        OpenView(form);
        form.Show();
    }

    private void OpenView(Control view)
    {
        Control? oldView = _currentView;
        _currentView = view;
        _mainContentPanel.SuspendLayout();
        _mainContentPanel.Controls.Clear();
        view.Dock = DockStyle.Fill;
        _mainContentPanel.Controls.Add(view);
        _mainContentPanel.ResumeLayout();
        if (oldView is Form)
        {
            oldView.Dispose();
        }
    }

    private void SetActiveNav(string text)
    {
        foreach ((string key, RoundedPanel item) in _navItems)
        {
            bool active = key == text;
            item.BackColor = active ? Color.FromArgb(225, 245, 250) : Color.White;
            item.BorderSize = active ? 1 : 0;
            item.Invalidate();
        }
    }

    private sealed class PatientVisitItem(string time, string petName, string ownerName, string visitReason, string status)
    {
        public string Time { get; } = time;
        public string PetName { get; } = petName;
        public string OwnerName { get; } = ownerName;
        public string VisitReason { get; } = visitReason;
        public string Status { get; set; } = status;
    }
}
