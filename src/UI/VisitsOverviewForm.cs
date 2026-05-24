using ClinicVets.Models;
using ClinicVets.Services;

namespace ClinicVets.UI;

/// <summary>
/// Read-only SQLite-backed visits overview for the secretary dashboard.
/// </summary>
public class VisitsOverviewForm : Form
{
    private readonly ClinicAppServices _services;
    private readonly DataGridView _visitsGrid = new();

    public VisitsOverviewForm(ClinicAppServices services)
    {
        _services = services;

        UiTheme.ApplyForm(this);
        Text = "ClinicVets - Visits Overview";
        ClientSize = new Size(920, 620);
        MinimumSize = new Size(760, 520);
        StartPosition = FormStartPosition.CenterParent;

        BuildLayout();
        LoadVisits();
    }

    private void BuildLayout()
    {
        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(24),
            BackColor = Color.FromArgb(245, 248, 251)
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
        Controls.Add(root);

        Label title = new()
        {
            Text = "Visits Overview",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = UiTheme.Text,
            TextAlign = ContentAlignment.MiddleLeft
        };
        root.Controls.Add(title, 0, 0);

        _visitsGrid.Dock = DockStyle.Fill;
        _visitsGrid.AllowUserToAddRows = false;
        _visitsGrid.AllowUserToDeleteRows = false;
        _visitsGrid.AllowUserToResizeRows = false;
        _visitsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _visitsGrid.BackgroundColor = Color.White;
        _visitsGrid.BorderStyle = BorderStyle.None;
        _visitsGrid.ColumnHeadersHeight = 38;
        _visitsGrid.ReadOnly = true;
        _visitsGrid.RowHeadersVisible = false;
        _visitsGrid.RowTemplate.Height = 34;
        _visitsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        root.Controls.Add(_visitsGrid, 0, 1);

        Button close = UiTheme.CreateSecondaryButton("Close", 0, 0, 110, 38);
        close.Anchor = AnchorStyles.Right;
        close.Click += (_, _) => Close();
        root.Controls.Add(close, 0, 2);
    }

    private void LoadVisits()
    {
        IReadOnlyList<DashboardVisitSummary> visits = _services.LookupService.GetRecentVisits(50);
        _visitsGrid.DataSource = visits.Select(visit => new
        {
            Date = visit.VisitDateTime.ToString("dd/MM/yyyy"),
            Time = visit.VisitDateTime.ToString("HH:mm"),
            Pet = visit.PetName,
            Owner = visit.OwnerName,
            Veterinarian = visit.VeterinarianName,
            Reason = visit.Reason,
            Diagnosis = visit.Diagnosis
        }).ToList();
    }
}
