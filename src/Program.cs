using System;
using System.Windows.Forms;
using hoccsharp.Forms;
using hoccsharp.Models;

namespace hoccsharp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // 1. Tự động khởi tạo CSDL SQLite (Tạo bảng Users, Courses, Students nếu chưa có)
            DatabaseHelper.InitDatabase();

            // 2. Chạy Form chính MDI
            Application.Run(new frmSchoolMain());
        }
    }
}