using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using hoccsharp.Models;

namespace hoccsharp.Forms
{
    public class frmRegister : Form
    {
        private TextBox txtFullName = new TextBox();
        private MaskedTextBox mtxtPhone = new MaskedTextBox();
        private DateTimePicker dtpBirthDate = new DateTimePicker();
        private ComboBox cboCourse = new ComboBox();
        private RadioButton rdbMale = new RadioButton();
        private RadioButton rdbFemale = new RadioButton();
        private Button btnRegister = new Button();

        public frmRegister()
        {
            InitializeComponent();
            LoadCourses();
        }

        private void InitializeComponent()
        {
            this.Text = "Bài 5.2: Đăng ký Học viên";
            this.Size = new Size(420, 320);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblName = new Label { Text = "Họ và tên:", Location = new Point(30, 20), AutoSize = true };
            txtFullName.Location = new Point(140, 17); txtFullName.Size = new Size(200, 23);

            Label lblPhone = new Label { Text = "Số điện thoại:", Location = new Point(30, 60), AutoSize = true };
            mtxtPhone.Location = new Point(140, 57); mtxtPhone.Size = new Size(200, 23);
            mtxtPhone.Mask = "(000) 000-0000";

            Label lblDob = new Label { Text = "Ngày sinh:", Location = new Point(30, 100), AutoSize = true };
            dtpBirthDate.Location = new Point(140, 97); dtpBirthDate.Size = new Size(200, 23);
            dtpBirthDate.Format = DateTimePickerFormat.Custom;
            dtpBirthDate.CustomFormat = "dd/MM/yyyy";

            Label lblGender = new Label { Text = "Giới tính:", Location = new Point(30, 140), AutoSize = true };
            rdbMale.Text = "Nam"; rdbMale.Location = new Point(140, 138); rdbMale.AutoSize = true; rdbMale.Checked = true;
            rdbFemale.Text = "Nữ"; rdbFemale.Location = new Point(210, 138); rdbFemale.AutoSize = true;

            Label lblCourse = new Label { Text = "Khóa học:", Location = new Point(30, 180), AutoSize = true };
            cboCourse.Location = new Point(140, 177); cboCourse.Size = new Size(200, 23);
            cboCourse.DropDownStyle = ComboBoxStyle.DropDownList;

            btnRegister.Text = "Đăng ký";
            btnRegister.Location = new Point(140, 220);
            btnRegister.Size = new Size(100, 32);
            btnRegister.Click += btnRegister_Click;

            this.Controls.AddRange(new Control[] { 
                lblName, txtFullName, lblPhone, mtxtPhone, 
                lblDob, dtpBirthDate, lblGender, rdbMale, rdbFemale, 
                lblCourse, cboCourse, btnRegister 
            });
        }

        private void LoadCourses()
        {
            var courseList = new List<dynamic>();
            using (var connection = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name FROM Courses";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courseList.Add(new { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                    }
                }
            }
            cboCourse.DataSource = courseList;
            cboCourse.DisplayMember = "Name";
            cboCourse.ValueMember = "Id";
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string gender = rdbMale.Checked ? "Nam" : "Nữ";

            using (var connection = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Students (FullName, Phone, BirthDate, CourseId, Gender)
                    VALUES (@name, @phone, @dob, @courseId, @gender)";

                command.Parameters.AddWithValue("@name", txtFullName.Text);
                command.Parameters.AddWithValue("@phone", mtxtPhone.Text);
                command.Parameters.AddWithValue("@dob", dtpBirthDate.Value.ToString("dd/MM/yyyy"));
                command.Parameters.AddWithValue("@courseId", cboCourse.SelectedValue);
                command.Parameters.AddWithValue("@gender", gender);

                command.ExecuteNonQuery();
            }

            MessageBox.Show($"Đã đăng ký thành công cho học viên: {txtFullName.Text}\nDữ liệu đã được lưu vào CSDL!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}