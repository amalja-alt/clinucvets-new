using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace ClinicVets.UI;

/// <summary>
/// Shared visual styling for ClinicVets WinForms screens.
/// </summary>
internal static class UiTheme
{
<<<<<<< HEAD
<<<<<<< HEAD
    public static string ImagePasth = FindImagesDirectory();

    public static readonly Color Semitransparent = Color.FromArgb(40, 255, 255, 255);
=======
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
    public static string ImagePasth = FindImagesDirectory();

    public static readonly Color Semitransparent = Color.FromArgb(40, 255, 255, 255);
>>>>>>> main
    public static readonly Color BackgroundTop = Color.FromArgb(248, 251, 255);
    public static readonly Color BackgroundBottom = Color.FromArgb(239, 250, 244);
    public static readonly Color Background = Color.FromArgb(247, 250, 252);
    public static readonly Color Card = Color.White;
    public static readonly Color Border = Color.FromArgb(225, 232, 240);
    public static readonly Color Primary = Color.FromArgb(79, 126, 236);
    public static readonly Color PrimaryDark = Color.FromArgb(45, 84, 185);
    public static readonly Color PrimaryHover = Color.FromArgb(62, 105, 214);
    public static readonly Color PrimarySoft = Color.FromArgb(222, 237, 251);
    public static readonly Color Secondary = Color.FromArgb(242, 245, 249);
    public static readonly Color Danger = Color.FromArgb(220, 38, 38);
    public static readonly Color Success = Color.FromArgb(22, 163, 74);
    public static readonly Color Warning = Color.FromArgb(234, 88, 12);
    public static readonly Color Text = Color.FromArgb(15, 23, 42);
    public static readonly Color Muted = Color.FromArgb(100, 116, 139);
    public static readonly Color InputText = Color.FromArgb(51, 65, 85);

    public static readonly Font TitleFont = new("Segoe UI", 22F, FontStyle.Bold);
    public static readonly Font SectionTitleFont = new("Segoe UI", 18F, FontStyle.Bold);
    public static readonly Font SubtitleFont = new("Segoe UI", 10.5F);
    public static readonly Font BodyFont = new("Segoe UI", 10F);
    public static readonly Font LabelFont = new("Segoe UI", 9.5F, FontStyle.Bold);
    public static readonly Font ButtonFont = new("Segoe UI", 10.5F, FontStyle.Bold);

    public static void ApplyForm(Form form)
    {
        form.AutoScaleMode = AutoScaleMode.Dpi;
        form.AutoScroll = true;
        form.Font = BodyFont;
<<<<<<< HEAD
<<<<<<< HEAD
        form.ForeColor = Text;
        form.StartPosition = FormStartPosition.CenterScreen;
        EnableDoubleBuffering(form);
    }

    public static void EnableDoubleBuffering(Control control)
    {
        typeof(Control)
            .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(control, true);
    }

    private static string FindImagesDirectory()
    {
        string? imagesPath = FindImagesDirectoryFrom(AppContext.BaseDirectory)
            ?? FindImagesDirectoryFrom(Application.StartupPath)
            ?? FindImagesDirectoryFrom(Directory.GetCurrentDirectory());

        return imagesPath is null
            ? Path.Combine(Application.StartupPath, "src", "images") + Path.DirectorySeparatorChar
            : imagesPath;
    }

    private static string? FindImagesDirectoryFrom(string startPath)
    {
        DirectoryInfo? current = new(startPath);

        while (current is not null)
        {
            string candidate = Path.Combine(current.FullName, "src", "images");
            if (Directory.Exists(candidate))
            {
                return Path.EndsInDirectorySeparator(candidate)
                    ? candidate
                    : candidate + Path.DirectorySeparatorChar;
            }

            current = current.Parent;
        }

        return null;
=======
        form.BackColor = Background;
        form.ForeColor = Text;
        form.StartPosition = FormStartPosition.CenterScreen;
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
        form.ForeColor = Text;
        form.StartPosition = FormStartPosition.CenterScreen;
        EnableDoubleBuffering(form);
    }

    public static void EnableDoubleBuffering(Control control)
    {
        typeof(Control)
            .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(control, true);
    }

    private static string FindImagesDirectory()
    {
        string? imagesPath = FindImagesDirectoryFrom(AppContext.BaseDirectory)
            ?? FindImagesDirectoryFrom(Application.StartupPath)
            ?? FindImagesDirectoryFrom(Directory.GetCurrentDirectory());

        return imagesPath is null
            ? Path.Combine(Application.StartupPath, "src", "images") + Path.DirectorySeparatorChar
            : imagesPath;
    }

    private static string? FindImagesDirectoryFrom(string startPath)
    {
        DirectoryInfo? current = new(startPath);

        while (current is not null)
        {
            string candidate = Path.Combine(current.FullName, "src", "images");
            if (Directory.Exists(candidate))
            {
                return Path.EndsInDirectorySeparator(candidate)
                    ? candidate
                    : candidate + Path.DirectorySeparatorChar;
            }

            current = current.Parent;
        }

        return null;
>>>>>>> main
    }

    public static RoundedPanel CreateCard(int x, int y, int width, int height, int radius = 18) =>
        new()
        {
            Location = new Point(x, y),
            Size = new Size(width, height),
            BackColor = Card,
            BorderColor = Border,
            BorderSize = 1,
<<<<<<< HEAD
<<<<<<< HEAD
            CornerRadius = radius,
            BackgroundImage = Image.FromFile(UiTheme.ImagePasth + "bg.jpg")

=======
            CornerRadius = radius
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
            CornerRadius = radius,
            BackgroundImage = Image.FromFile(UiTheme.ImagePasth + "bg.jpg")

>>>>>>> main
        };

    public static Label CreateTitle(string text, int x, int y, int width) =>
        new()
        {
            Text = text,
            Font = TitleFont,
            ForeColor = Text,
            Location = new Point(x, y),
            Size = new Size(width, 42),
            TextAlign = ContentAlignment.MiddleLeft
        };

    public static Label CreateFieldLabel(string text, int x, int y, int width = 320) =>
        new()
        {
            Text = text,
            Font = LabelFont,
            ForeColor = Text,
            Location = new Point(x, y),
            Size = new Size(width, 22),
            TextAlign = ContentAlignment.MiddleLeft
        };

    public static TextBox CreateTextBox(string placeholder, int x, int y, int width, bool password = false) =>
        new()
        {
            Location = new Point(x, y),
            Size = new Size(width, 34),
            Font = new Font("Segoe UI", 10.5F),
            BorderStyle = BorderStyle.FixedSingle,
            ForeColor = InputText,
            PlaceholderText = placeholder,
<<<<<<< HEAD
<<<<<<< HEAD
            UseSystemPasswordChar = password,
        };
   




=======
            UseSystemPasswordChar = password
        };
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
            UseSystemPasswordChar = password,
        };
   




>>>>>>> main

    public static ComboBox CreateComboBox(int x, int y, int width) =>
        new()
        {
            Location = new Point(x, y),
            Size = new Size(width, 34),
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10.5F),
            ForeColor = InputText
        };

    public static Button CreatePrimaryButton(string text, int x, int y, int width = 160, int height = 44) =>
        CreateButton(text, x, y, width, height, Primary, Color.White);

    public static Button CreateSecondaryButton(string text, int x, int y, int width = 160, int height = 44) =>
        CreateButton(text, x, y, width, height, Secondary, Text);

    public static Button CreateSoftPrimaryButton(string text, int x, int y, int width = 160, int height = 44) =>
        CreateButton(text, x, y, width, height, PrimarySoft, Color.FromArgb(20, 68, 111));

    public static Button CreateDangerButton(string text, int x, int y, int width = 120, int height = 40) =>
        CreateButton(text, x, y, width, height, Danger, Color.White);

    private static Button CreateButton(string text, int x, int y, int width, int height, Color back, Color fore)
    {
        Button button = new()
        {
            Text = text,
            Location = new Point(x, y),
            Size = new Size(width, height),
            FlatStyle = FlatStyle.Flat,
            BackColor = back,
            ForeColor = fore,
            Font = ButtonFont,
            Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderSize = 0;
        return button;
    }

    public static void PaintGradientBackground(Control control, PaintEventArgs e)
    {
        Rectangle rect = control.ClientRectangle;
        if (rect.Width <= 0 || rect.Height <= 0)
        {
            return;
        }

        using LinearGradientBrush brush = new(rect, BackgroundTop, BackgroundBottom, 90F);
        e.Graphics.FillRectangle(brush, rect);
    }
}

internal sealed class RoundedPanel : Panel
{
    private Color _gradientEndColor = Color.Empty;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int CornerRadius { get; set; } = 18;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color BorderColor { get; set; } = UiTheme.Border;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int BorderSize { get; set; } = 1;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color GradientEndColor
    {
        get => _gradientEndColor;
        set
        {
            _gradientEndColor = value;
            Invalidate();
        }
    }

    public RoundedPanel()
    {
        DoubleBuffered = true;
        BackColor = Color.White;
        Padding = new Padding(0);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle bounds = new(0, 0, Width - 1, Height - 1);

        using GraphicsPath path = CreateRoundedRectangle(bounds, CornerRadius);
        if (GradientEndColor == Color.Empty)
        {
            using SolidBrush brush = new(BackColor);
            e.Graphics.FillPath(brush, path);
        }
        else
        {
            using LinearGradientBrush brush = new(bounds, BackColor, GradientEndColor, 0F);
            e.Graphics.FillPath(brush, path);
        }

        if (BorderSize > 0)
        {
            using Pen pen = new(BorderColor, BorderSize);
            e.Graphics.DrawPath(pen, path);
        }
    }

    protected override void OnResize(EventArgs eventargs)
    {
        base.OnResize(eventargs);
        using GraphicsPath path = CreateRoundedRectangle(new Rectangle(0, 0, Width, Height), CornerRadius);
        Region = new Region(path);
    }

    private static GraphicsPath CreateRoundedRectangle(Rectangle rectangle, int radius)
    {
        int diameter = radius * 2;
        GraphicsPath path = new();
        path.AddArc(rectangle.X, rectangle.Y, diameter, diameter, 180, 90);
        path.AddArc(rectangle.Right - diameter, rectangle.Y, diameter, diameter, 270, 90);
        path.AddArc(rectangle.Right - diameter, rectangle.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(rectangle.X, rectangle.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }
}

internal sealed class RoundedButton : Button
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int CornerRadius { get; set; } = 16;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color BorderColor { get; set; } = Color.Transparent;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int BorderSize { get; set; }

    public RoundedButton()
    {
        DoubleBuffered = true;
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        UseVisualStyleBackColor = false;
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        pevent.Graphics.Clear(Parent?.BackColor ?? UiTheme.Background);

        Rectangle bounds = new(0, 0, Width - 1, Height - 1);
        using GraphicsPath path = CreateRoundedRectangle(bounds, CornerRadius);
        using SolidBrush brush = new(BackColor);
        pevent.Graphics.FillPath(brush, path);

        if (BorderSize > 0)
        {
            using Pen pen = new(BorderColor, BorderSize);
            pevent.Graphics.DrawPath(pen, path);
        }

        TextRenderer.DrawText(
            pevent.Graphics,
            Text,
            Font,
            ClientRectangle,
            ForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        using GraphicsPath path = CreateRoundedRectangle(new Rectangle(0, 0, Width, Height), CornerRadius);
        Region = new Region(path);
    }

    private static GraphicsPath CreateRoundedRectangle(Rectangle rectangle, int radius)
    {
        int diameter = radius * 2;
        GraphicsPath path = new();
        path.AddArc(rectangle.X, rectangle.Y, diameter, diameter, 180, 90);
        path.AddArc(rectangle.Right - diameter, rectangle.Y, diameter, diameter, 270, 90);
        path.AddArc(rectangle.Right - diameter, rectangle.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(rectangle.X, rectangle.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }
<<<<<<< HEAD
<<<<<<< HEAD

     
    
=======
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======

     
    
>>>>>>> main
}
