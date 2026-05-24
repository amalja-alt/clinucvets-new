using ClinicVets.Models;
using ClinicVets.Services;

namespace ClinicVets.UI;

public class LoginForm : Form
{
    private readonly ClinicAppServices _services;
    public   TextBox _usernameTextBox = new();
    private readonly TextBox _passwordTextBox = new();
    private readonly Label _errorLabel = new();

    public LoginForm(ClinicAppServices services)
    {
        _services = services;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        UiTheme.ApplyForm(this);
        Text = "ClinicVets - Staff Login";
        MinimumSize = new Size(620, 680);
        this.WindowState = FormWindowState.Maximized;
        this.DoubleBuffered = true;
      
        StartPosition = FormStartPosition.CenterScreen;
        BackgroundImage = Image.FromFile(UiTheme.ImagePasth+"bg.jpg");
 
 
        Panel mainpanel = new Panel
        {
             Height = 750,
            Width = 450,
            BackColor = Color.Transparent,
            Anchor = AnchorStyles.None,
            BackgroundImage = Image.FromFile(UiTheme.ImagePasth + "login/bg.png"),
            BackgroundImageLayout = ImageLayout.Stretch,
            Dock = DockStyle.None,
        };


        TableLayoutPanel shell = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 1,
            BackColor = Color.Transparent
        };
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Controls.Add(shell);
        shell.Controls.Add(mainpanel, 0, 0);

        TableLayoutPanel grid = new TableLayoutPanel
        {
        
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,

        };
       grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
       grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 200));
       grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        mainpanel.Controls.Add(grid);

        grid.Controls.Add(new Panel { BackColor = Color.Transparent, Dock = DockStyle.Fill },0,0);

        FlowLayoutPanel stack = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10, 10, 10, 10) , BackColor = Color.Transparent , Margin = new Padding(10), FlowDirection = FlowDirection.TopDown, WrapContents = false };
        grid.Controls.Add(stack,0,1);


        _errorLabel.Name = "errorLabel";
        _errorLabel.Dock = DockStyle.Fill;
        _errorLabel.ForeColor = UiTheme.Danger;
        _errorLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _errorLabel.TextAlign = ContentAlignment.MiddleCenter;
        _errorLabel.AutoEllipsis = true;
        stack.Controls.Add(_errorLabel);


        stack.Controls.Add(_usernameTextBox);
        stack.Controls.Add(_passwordTextBox);
        _usernameTextBox.Name = "usernameTextBox";
        _usernameTextBox.Size = new Size(mainpanel.Width - 110, 58);
        _usernameTextBox.Font = new Font("Segoe UI", 20F, FontStyle.Regular);
        _usernameTextBox.Margin = new Padding(0, 14, 0, 12);
        _usernameTextBox.PlaceholderText = "USER NAME";

        _passwordTextBox.Name = "passwordTextBox";
        _passwordTextBox.Size = new Size(mainpanel.Width - 110, 58);
        _passwordTextBox.Font = new Font("Segoe UI", 20F, FontStyle.Regular);
        _passwordTextBox.Margin = new Padding(0, 0, 0, 0);
        _passwordTextBox.PlaceholderText = "PASSWORD";
        _passwordTextBox.UseSystemPasswordChar = true;


        Button btn_login = new Button
        {
            Name = "loginButton",
            Dock = DockStyle.None,
            BackgroundImage = Image.FromFile(UiTheme.ImagePasth + "login/btnlogin.png"),
            BackgroundImageLayout = ImageLayout.Zoom,
            FlatStyle = FlatStyle.Flat,

            Width = mainpanel.Width - 110,
            Height = 64,
            BackColor = Color.Transparent,
            Margin = new Padding(0, 16, 0, 14),
            UseVisualStyleBackColor = false,
        };


        btn_login.Click += LoginButton_Click;

        btn_login.FlatAppearance.MouseOverBackColor =  Color.Transparent;
        btn_login.FlatAppearance.MouseDownBackColor = Color.Transparent;
        btn_login.TabStop = false;
        btn_login.FlatAppearance.BorderSize = 0;

        btn_login.MouseEnter += Btn_login_MouseEnter;
        btn_login.MouseLeave += Btn_login_MouseLeave;
        stack.Controls.Add(btn_login);
     

        stack.Controls.Add(new PictureBox { Image = Image.FromFile(UiTheme.ImagePasth+"/login/sep.png"), Margin = new Padding(0, 8, 0, 0), Width = mainpanel.Width - 90, Height = 18, SizeMode = PictureBoxSizeMode.Zoom });
        stack.Controls.Add(new PictureBox { BackgroundImage = Image.FromFile(UiTheme.ImagePasth + "/login/txtreg.png"), Margin = new Padding(0, 4, 0, 8), Width = mainpanel.Width - 110, Height = 34, Dock = DockStyle.None, BackgroundImageLayout = ImageLayout.Zoom });

        Button btn_reg = new Button
        {
            Name = "registerButton",
            Dock = DockStyle.None,
            BackgroundImage = Image.FromFile(UiTheme.ImagePasth + "login/btnreg.png"),
            BackgroundImageLayout = ImageLayout.Zoom,
            FlatStyle = FlatStyle.Flat,
            Width = mainpanel.Width - 110,
            Height = 46,
            BackColor = Color.Transparent,
            Margin = new Padding(0, 4, 0, 10),
            UseVisualStyleBackColor = false,
        };

        stack.Controls.Add(btn_reg);
        btn_reg.FlatAppearance.MouseOverBackColor = Color.Transparent;
        btn_reg.FlatAppearance.MouseDownBackColor = Color.Transparent;
        btn_reg.TabStop = false;
        btn_reg.FlatAppearance.BorderSize = 0;


        btn_reg.MouseEnter += Btn_login_MouseEnter;
        btn_reg.MouseLeave += Btn_login_MouseLeave;

        btn_reg.Click += RegisterButton_Click;


    }


   


    private void Btn_login_MouseLeave(object? sender, EventArgs e)
    {
       
    }

    private void Btn_login_MouseEnter(object? sender, EventArgs e)
    {
       

    }

    private void LoginButton_Click(object? sender, EventArgs e)
    {
        AuthenticationResult result = _services.AuthService.Login(
            _usernameTextBox.Text.Trim(),
            _passwordTextBox.Text);

       if (!result.IsSuccess)
        {
           _errorLabel.Text = result.ErrorMessage;
          return;
       }

        _errorLabel.Text = string.Empty;
        Hide();

        string welcome = _services.AuthService.CurrentUser is null
            ? "Veterinary Clinic Management"
            : $"Welcome, {_services.AuthService.CurrentUser.Username} ({_services.AuthService.CurrentUser.Role})";

        using Form dashboardForm = result.LoggedInUser?.Role == StaffRole.Veterinarian
            ? new VeterinarianDashboardForm(_services, welcome)
            : new SecretaryDashboardForm(_services, welcome);
          dashboardForm.ShowDialog(this);

        _services.AuthService.Logout();
        _passwordTextBox.Clear();
        Show();
    }

    private void RegisterButton_Click(object? sender, EventArgs e)
    {
        Hide();
        using RegisterEmployeeForm registerForm = new(_services);
        registerForm.ShowDialog();
        Show();
        _passwordTextBox.Clear();
    }
}
