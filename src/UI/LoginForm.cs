using ClinicVets.Models;
using ClinicVets.Services;

namespace ClinicVets.UI;

public class LoginForm : Form
{
    private readonly ClinicAppServices _services;
<<<<<<< HEAD
<<<<<<< HEAD
    public   TextBox _usernameTextBox = new();
=======
    private readonly TextBox _usernameTextBox = new();
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
    public   TextBox _usernameTextBox = new();
>>>>>>> main
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
<<<<<<< HEAD
<<<<<<< HEAD
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

=======
        ClientSize = new Size(1280, 720);
=======
>>>>>>> main
        MinimumSize = new Size(620, 680);
        this.WindowState = FormWindowState.Maximized;
        this.DoubleBuffered = true;
      
        StartPosition = FormStartPosition.CenterScreen;
<<<<<<< HEAD
        Paint += (_, e) => UiTheme.PaintGradientBackground(this, e);
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
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

>>>>>>> main

        TableLayoutPanel shell = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
<<<<<<< HEAD
<<<<<<< HEAD
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

=======
            RowCount = 3,
            BackColor = Color.Transparent,
            Padding = new Padding(24)
=======
            RowCount = 1,
            BackColor = Color.Transparent
>>>>>>> main
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

<<<<<<< HEAD
        cardLayout.Controls.Add(CreateLogo(), 0, 0);
        cardLayout.Controls.Add(CreateTitle("ClinicVets", 22F, UiTheme.Text, FontStyle.Bold), 0, 1);
        cardLayout.Controls.Add(CreateTitle("Veterinary Clinic Management System", 10F, UiTheme.Muted, FontStyle.Regular), 0, 2);
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
>>>>>>> main

        _errorLabel.Name = "errorLabel";
        _errorLabel.Dock = DockStyle.Fill;
        _errorLabel.ForeColor = UiTheme.Danger;
        _errorLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _errorLabel.TextAlign = ContentAlignment.MiddleCenter;
        _errorLabel.AutoEllipsis = true;
<<<<<<< HEAD
<<<<<<< HEAD
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
       

=======
        cardLayout.Controls.Add(_errorLabel, 0, 3);
=======
        stack.Controls.Add(_errorLabel);
>>>>>>> main


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
       

<<<<<<< HEAD
    private static Control CreateInputBlock(string labelText, TextBox textBox, string placeholder, bool password)
    {
        TableLayoutPanel block = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Margin = new Padding(0, 4, 0, 8)
        };
        block.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        block.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        block.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        Label label = new()
        {
            Text = labelText,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = UiTheme.Text,
            TextAlign = ContentAlignment.MiddleLeft
        };
        block.Controls.Add(label, 0, 0);

        textBox.Dock = DockStyle.Fill;
        textBox.Font = new Font("Segoe UI", 11F);
        textBox.BorderStyle = BorderStyle.FixedSingle;
        textBox.ForeColor = UiTheme.Text;
        textBox.PlaceholderText = placeholder;
        textBox.UseSystemPasswordChar = password;
        textBox.Margin = new Padding(0, 4, 0, 0);
        block.Controls.Add(textBox, 0, 1);

        return block;
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
>>>>>>> main
    }

    private void LoginButton_Click(object? sender, EventArgs e)
    {
        AuthenticationResult result = _services.AuthService.Login(
            _usernameTextBox.Text.Trim(),
            _passwordTextBox.Text);

<<<<<<< HEAD
<<<<<<< HEAD
       if (!result.IsSuccess)
        {
           _errorLabel.Text = result.ErrorMessage;
          return;
       }
=======
        if (!result.IsSuccess)
        {
            _errorLabel.Text = result.ErrorMessage;
            return;
        }
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
       if (!result.IsSuccess)
        {
           _errorLabel.Text = result.ErrorMessage;
          return;
       }
>>>>>>> main

        _errorLabel.Text = string.Empty;
        Hide();

        string welcome = _services.AuthService.CurrentUser is null
            ? "Veterinary Clinic Management"
            : $"Welcome, {_services.AuthService.CurrentUser.Username} ({_services.AuthService.CurrentUser.Role})";

        using Form dashboardForm = result.LoggedInUser?.Role == StaffRole.Veterinarian
            ? new VeterinarianDashboardForm(_services, welcome)
            : new SecretaryDashboardForm(_services, welcome);
<<<<<<< HEAD
<<<<<<< HEAD
          dashboardForm.ShowDialog(this);
=======
        dashboardForm.ShowDialog(this);
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
          dashboardForm.ShowDialog(this);
>>>>>>> main

        _services.AuthService.Logout();
        _passwordTextBox.Clear();
        Show();
    }

    private void RegisterButton_Click(object? sender, EventArgs e)
    {
<<<<<<< HEAD
<<<<<<< HEAD
        Hide();
        using RegisterEmployeeForm registerForm = new(_services);
        registerForm.ShowDialog();
        Show();
        _passwordTextBox.Clear();
=======
        using RegisterEmployeeForm registerForm = new(_services);
        registerForm.ShowDialog(this);
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
        Hide();
        using RegisterEmployeeForm registerForm = new(_services);
        registerForm.ShowDialog();
        Show();
        _passwordTextBox.Clear();
>>>>>>> main
    }
}
