using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using hoccsharp.Models;

namespace hoccsharp.Forms
{
    public class frmLogin : Form
    {
        private TextBox txtUsername = new TextBox();
        private TextBox txtPassword = new TextBox();
        private CheckBox chkShowPassword = new CheckBox();
        private Button btnLogin = new Button();
        private Button btnExit = new Button();
        private ErrorProvider errorProvider = new ErrorProvider();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Bài 5.1: Đăng nhập Hệ thống";
            this.Size = new Size(380, 260);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblUser = new Label { Text = "Tên đăng nhập:", Location = new Point(30, 30), AutoSize = true };
            txtUsername.Location = new Point(140, 27);
            txtUsername.Size = new Size(180, 23);

            Label lblPass = new Label { Text = "Mật khẩu:", Location = new Point(30, 70), AutoSize = true };
            txtPassword.Location = new Point(140, 67);
            txtPassword.Size = new Size(180, 23);
            txtPassword.UseSystemPasswordChar = true;

            chkShowPassword.Text = "Hiện mật khẩu";
            chkShowPassword.Location = new Point(140, 100);
            chkShowPassword.AutoSize = true;
            chkShowPassword.CheckedChanged += (s, e) => txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;

            btnLogin.Text = "Đăng nhập";
            btnLogin.Location = new Point(140, 140);
            btnLogin.Size = new Size(85, 30);
            btnLogin.Click += btnLogin_Click;

            btnExit.Text = "Thoát";
            btnExit.Location = new Point(235, 140);
            btnExit.Size = new Size(85, 30);
            btnExit.Click += (s, e) => this.Close();

            this.AcceptButton = btnLogin;
            this.CancelButton = btnExit;

            this.Controls.AddRange(new Control[] { lblUser, txtUsername, lblPass, txtPassword, chkShowPassword, btnLogin, btnExit });
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorProvider.SetError(txtUsername, "Vui lòng nhập tên đăng nhập!");
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider.SetError(txtPassword, "Vui lòng nhập mật khẩu!");
                isValid = false;
            }

            if (!isValid) return;

            // Xử lý kiểm tra tài khoản từ SQLite CSDL
            using (var connection = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @user AND Password = @pass";
                command.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                command.Parameters.AddWithValue("@pass", txtPassword.Text.Trim());

                long count = (long)command.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}