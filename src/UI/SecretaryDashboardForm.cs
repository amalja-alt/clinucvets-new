using ClinicVets.Models;
using ClinicVets.Services;

namespace ClinicVets.UI;

/// <summary>
/// Secretary dashboard focused on the required customer, animal, and visits workflow.
/// </summary>
public class SecretaryDashboardForm : Form
{
    private readonly ClinicAppServices _services;
    private readonly string _username;
    private readonly TextBox _searchBox = new();
    private readonly Label _appointmentsValueLabel = new();
    private readonly Label _customersValueLabel = new();
    private readonly Label _animalsValueLabel = new();
    private readonly DataGridView _appointmentsGrid = new();
    private readonly Panel _mainContentPanel = new();
    private readonly List<Button> _navButtons = [];
    private Control? _currentView;

    public SecretaryDashboardForm(ClinicAppServices services, string? welcomeMessage = null)
    {
        _services = services;
        _username = _services.AuthService.CurrentUser?.Username ?? "Secretary";

        UiTheme.ApplyForm(this);
        Text = "ClinicVets - Secretary Dashboard";
        ClientSize = new Size(1180, 760);
        MinimumSize = new Size(900, 620);
        StartPosition = FormStartPosition.CenterScreen;

        BuildLayout();
        ShowHomeView(welcomeMessage);
    }

    private void BuildLayout()
    {
        TableLayoutPanel shell = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.FromArgb(245, 248, 251)
        };
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 224));
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        Controls.Add(shell);

        shell.Controls.Add(CreateSidebar(), 0, 0);
        shell.Controls.Add(CreateMainShell(), 1, 0);
    }

    private Control CreateSidebar()
    {
        Panel sidebar = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(18, 20, 18, 18)
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 82));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 118));
        sidebar.Controls.Add(layout);

        layout.Controls.Add(new Label
        {
            Text = "ClinicVets",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 20F, FontStyle.Bold),
            ForeColor = Color.FromArgb(20, 84, 132),
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 0);

        FlowLayoutPanel nav = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false
        };
        layout.Controls.Add(nav, 0, 1);

        nav.Controls.Add(CreateNavButton("Dashboard", true, () => ShowHomeView()));
        nav.Controls.Add(CreateNavButton("Appointments / Visits", false, () => OpenEmbeddedForm("Appointments / Visits", () => new VisitsOverviewForm(_services))));
        nav.Controls.Add(CreateNavButton("Customers", false, () => OpenEmbeddedForm("Customers", () => new CustomerForm(_services))));
        nav.Controls.Add(CreateNavButton("Pets", false, () => OpenEmbeddedForm("Pets", () => new AnimalForm(_services))));

        TableLayoutPanel footer = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        footer.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        footer.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        layout.Controls.Add(footer, 0, 2);

        footer.Controls.Add(new Label
        {
            Text = $"{_username}\nSecretary",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = UiTheme.Muted,
            TextAlign = ContentAlignment.BottomLeft
        }, 0, 0);

        Button logout = UiTheme.CreateSecondaryButton("Logout", 0, 0, 160, 36);
        logout.Dock = DockStyle.Fill;
        logout.Click += (_, _) => Logout();
        footer.Controls.Add(logout, 0, 1);

        return sidebar;
    }

    private Button CreateNavButton(string text, bool active, Action action)
    {
        Button button = new()
        {
            Text = text,
            Width = 188,
            Height = 42,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(14, 0, 0, 0),
            Margin = new Padding(0, 0, 0, 10),
            BackColor = active ? Color.FromArgb(230, 245, 249) : Color.White,
            ForeColor = active ? Color.FromArgb(20, 84, 132) : UiTheme.Text,
            Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderSize = active ? 1 : 0;
        button.FlatAppearance.BorderColor = Color.FromArgb(190, 222, 232);
        button.Click += (_, _) => action();
        _navButtons.Add(button);
        return button;
    }

    private Control CreateMainShell()
    {
        TableLayoutPanel main = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.FromArgb(245, 248, 251)
        };
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 74));
        main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        main.Controls.Add(CreateTopBar(), 0, 0);
        _mainContentPanel.Dock = DockStyle.Fill;
        _mainContentPanel.BackColor = Color.FromArgb(245, 248, 251);
        main.Controls.Add(_mainContentPanel, 0, 1);

        return main;
    }

    private Control CreateTopBar()
    {
        Panel topBar = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(22, 14, 22, 14)
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 1
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112));
        topBar.Controls.Add(layout);

        layout.Controls.Add(new Label
        {
            Text = "Secretary Dashboard",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 17F, FontStyle.Bold),
            ForeColor = UiTheme.Text,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 0);

        _searchBox.Dock = DockStyle.Fill;
        _searchBox.Font = new Font("Segoe UI", 10.5F);
        _searchBox.PlaceholderText = "Search today's pet or owner";
        _searchBox.Margin = new Padding(0, 4, 14, 4);
        _searchBox.TextChanged += (_, _) => LoadTodayAppointments();
        layout.Controls.Add(_searchBox, 1, 0);

        Button logout = UiTheme.CreatePrimaryButton("Logout", 0, 0, 96, 38);
        logout.Dock = DockStyle.Fill;
        logout.Margin = new Padding(0, 4, 0, 4);
        logout.Click += (_, _) => Logout();
        layout.Controls.Add(logout, 2, 0);

        return topBar;
    }

    private void ShowHomeView(string? welcomeMessage = null)
    {
        SetActiveNav("Dashboard");
        OpenView(CreateHomeView(welcomeMessage));
        RefreshDashboard();
    }

    private Control CreateHomeView(string? welcomeMessage)
    {
        Panel scrollHost = new()
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(24),
            BackColor = Color.FromArgb(245, 248, 251)
        };

        TableLayoutPanel content = new()
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            RowCount = 4
        };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        scrollHost.Controls.Add(content);

        content.Controls.Add(CreateWelcomeCard(welcomeMessage), 0, 0);
        content.Controls.Add(CreateSummaryCards(), 0, 1);
        content.Controls.Add(CreateQuickActions(), 0, 2);
        content.Controls.Add(CreateTodayAppointmentsSection(), 0, 3);
        return scrollHost;
    }

    private Control CreateWelcomeCard(string? welcomeMessage)
    {
        RoundedPanel card = new()
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

        card.Controls.Add(new Label
        {
            Text = $"Welcome back, {_username}",
            Dock = DockStyle.Top,
            Height = 48,
            Font = new Font("Segoe UI", 24F, FontStyle.Bold),
            ForeColor = Color.White,
            AutoEllipsis = true
        });
        card.Controls.Add(new Label
        {
            Text = string.IsNullOrWhiteSpace(welcomeMessage)
                ? "Customer, pet, and visit information loaded from SQLite"
                : welcomeMessage,
            Dock = DockStyle.Bottom,
            Height = 32,
            Font = new Font("Segoe UI", 11F),
            ForeColor = Color.FromArgb(225, 245, 250),
            AutoEllipsis = true
        });

        return card;
    }

    private Control CreateSummaryCards()
    {
        TableLayoutPanel cards = new()
        {
            Dock = DockStyle.Top,
            Height = 126,
            ColumnCount = 3,
            RowCount = 1,
            Margin = new Padding(0, 0, 0, 18)
        };
        cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

        cards.Controls.Add(CreateStatCard("Today's Appointments", _appointmentsValueLabel, Color.FromArgb(50, 126, 210)), 0, 0);
        cards.Controls.Add(CreateStatCard("Customers", _customersValueLabel, Color.FromArgb(20, 150, 170)), 1, 0);
        cards.Controls.Add(CreateStatCard("Animals / Pets", _animalsValueLabel, Color.FromArgb(63, 176, 112)), 2, 0);
        return cards;
    }

    private static Control CreateStatCard(string title, Label valueLabel, Color color)
    {
        RoundedPanel card = new()
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 14, 0),
            BackColor = Color.White,
            BorderColor = UiTheme.Border,
            BorderSize = 1,
            CornerRadius = 14,
            Padding = new Padding(18)
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 56));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 38));
        card.Controls.Add(layout);

        layout.Controls.Add(new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = color,
            Margin = new Padding(0, 0, 12, 8)
        }, 0, 0);

        valueLabel.Text = "0";
        valueLabel.Dock = DockStyle.Fill;
        valueLabel.Font = new Font("Segoe UI", 25F, FontStyle.Bold);
        valueLabel.ForeColor = UiTheme.Text;
        valueLabel.TextAlign = ContentAlignment.MiddleRight;
        layout.Controls.Add(valueLabel, 1, 0);

        Label titleLabel = new()
        {
            Text = title,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            ForeColor = UiTheme.Muted,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true
        };
        layout.Controls.Add(titleLabel, 0, 1);
        layout.SetColumnSpan(titleLabel, 2);

        return card;
    }

    private Control CreateQuickActions()
    {
        RoundedPanel panel = CreateSectionPanel(146);
        TableLayoutPanel content = CreateSectionContent();
        panel.Controls.Add(content);
        content.Controls.Add(CreateSectionTitle("Quick Actions"), 0, 0);

        TableLayoutPanel actions = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 1
        };
        actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
        content.Controls.Add(actions, 0, 1);

        actions.Controls.Add(CreateActionButton("New Customer", Color.FromArgb(20, 150, 170), () => OpenEmbeddedForm("Customers", () => new CustomerForm(_services))), 0, 0);
        actions.Controls.Add(CreateActionButton("New Pet", Color.FromArgb(63, 176, 112), () => OpenEmbeddedForm("Pets", () => new AnimalForm(_services))), 1, 0);
        actions.Controls.Add(CreateActionButton("View Visits", Color.FromArgb(50, 126, 210), () => OpenEmbeddedForm("Appointments / Visits", () => new VisitsOverviewForm(_services))), 2, 0);

        return panel;
    }

    private static Control CreateActionButton(string title, Color color, Action action)
    {
        Button button = new()
        {
            Text = title,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 14, 0),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = color,
            Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderSize = 0;
        button.Click += (_, _) => action();
        return button;
    }

    private Control CreateTodayAppointmentsSection()
    {
        RoundedPanel panel = CreateSectionPanel(320);
        TableLayoutPanel content = CreateSectionContent();
        panel.Controls.Add(content);
        content.Controls.Add(CreateSectionTitle("Today's Appointments"), 0, 0);

        _appointmentsGrid.Dock = DockStyle.Fill;
        _appointmentsGrid.AllowUserToAddRows = false;
        _appointmentsGrid.AllowUserToDeleteRows = false;
        _appointmentsGrid.AllowUserToResizeRows = false;
        _appointmentsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _appointmentsGrid.BackgroundColor = Color.White;
        _appointmentsGrid.BorderStyle = BorderStyle.None;
        _appointmentsGrid.ColumnHeadersHeight = 38;
        _appointmentsGrid.ReadOnly = true;
        _appointmentsGrid.RowHeadersVisible = false;
        _appointmentsGrid.RowTemplate.Height = 34;
        _appointmentsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        content.Controls.Add(_appointmentsGrid, 0, 1);

        return panel;
    }

    private static TableLayoutPanel CreateSectionContent()
    {
        TableLayoutPanel content = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        return content;
    }

    private static RoundedPanel CreateSectionPanel(int height)
    {
        return new RoundedPanel
        {
            Dock = DockStyle.Top,
            Height = height,
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
            Height = 38,
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            ForeColor = UiTheme.Text
        };
    }

    private void RefreshDashboard()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        _appointmentsValueLabel.Text = _services.LookupService.CountVisitsForDate(today).ToString();
        _customersValueLabel.Text = _services.LookupService.GetAllCustomers().Count.ToString();
        _animalsValueLabel.Text = _services.LookupService.GetAllAnimals().Count.ToString();
        LoadTodayAppointments();
    }

    private void LoadTodayAppointments()
    {
        if (_appointmentsGrid.IsDisposed)
        {
            return;
        }

        string query = _searchBox.Text.Trim();
        IEnumerable<DashboardVisitSummary> visits = _services.LookupService.GetVisitsForDate(DateOnly.FromDateTime(DateTime.Today));

        if (!string.IsNullOrWhiteSpace(query))
        {
            visits = visits.Where(visit =>
                visit.PetName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                visit.OwnerName.Contains(query, StringComparison.OrdinalIgnoreCase));
        }

        _appointmentsGrid.DataSource = visits.Select(visit => new
        {
            Time = visit.VisitDateTime.ToString("HH:mm"),
            Pet = visit.PetName,
            Owner = visit.OwnerName,
            Veterinarian = visit.VeterinarianName,
            Reason = visit.Reason,
            Diagnosis = visit.Diagnosis
        }).ToList();
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
        foreach (Button button in _navButtons)
        {
            bool active = button.Text == text;
            button.BackColor = active ? Color.FromArgb(230, 245, 249) : Color.White;
            button.ForeColor = active ? Color.FromArgb(20, 84, 132) : UiTheme.Text;
            button.FlatAppearance.BorderSize = active ? 1 : 0;
        }
    }

    private void Logout()
    {
        _services.AuthService.Logout();
        DialogResult = DialogResult.OK;
        Close();
    }
}
