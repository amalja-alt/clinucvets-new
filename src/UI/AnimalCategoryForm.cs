using ClinicVets.Models;
using ClinicVets.Services;

namespace ClinicVets.UI;

/// <summary>
/// Displays the animal categories supported by the current model.
/// </summary>
public class AnimalCategoryForm : Form
{
    private readonly ClinicAppServices _services;
    private readonly ListBox _categoryList;
    private readonly Label _statusLabel;

    public AnimalCategoryForm(ClinicAppServices services)
    {
        _services = services;

        UiTheme.ApplyForm(this);
        Text = "ClinicVets - Animal Categories";
        ClientSize = new Size(520, 420);
        MinimumSize = new Size(420, 360);

        Panel card = UiTheme.CreateCard(24, 24, 472, 372);
        card.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        Controls.Add(card);
 
        card.Controls.Add(UiTheme.CreateTitle("Animal Categories", 24, 20, 400));
        card.Controls.Add(UiTheme.CreateFieldLabel("Available categories", 24, 72));

        _categoryList = new ListBox
        {
            Location = new Point(24, 96),
            Size = new Size(424, 190),
            IntegralHeight = false
        };
        card.Controls.Add(_categoryList);

        Button refreshButton = UiTheme.CreateSecondaryButton("Refresh", 24, 304, 100);
        refreshButton.Click += (_, _) => LoadCategories();
        card.Controls.Add(refreshButton);

        Button closeButton = UiTheme.CreateSecondaryButton("Close", 308, 304, 140);
        closeButton.Click += (_, _) => Close();
        card.Controls.Add(closeButton);

        _statusLabel = new Label
        {
            Location = new Point(24, 340),
            Size = new Size(424, 20),
            ForeColor = UiTheme.Muted
        };
        card.Controls.Add(_statusLabel);

        LoadCategories();
    }

    private void LoadCategories()
    {
        _categoryList.Items.Clear();
        foreach (AnimalCategory category in _services.AnimalCategoryService.GetAllCategories())
        {
            _categoryList.Items.Add(category.Name);
        }

        _statusLabel.Text = $"{_categoryList.Items.Count} categor(ies) available.";
    }
}
