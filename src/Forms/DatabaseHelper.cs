using System;
using Microsoft.Data.Sqlite;

namespace hoccsharp.Models
{
    public static class DatabaseHelper
    {
        public static string ConnectionString = "Data Source=app.db";

        public static void InitDatabase()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    -- Bảng tài khoản cho bài 5.1
                    CREATE TABLE IF NOT EXISTS Users (
                        Username TEXT PRIMARY KEY,
                        Password TEXT NOT NULL
                    );

                    -- Bảng khóa học và học viên cho bài 5.2
                    CREATE TABLE IF NOT EXISTS Courses (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS Students (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FullName TEXT,
                        Phone TEXT,
                        BirthDate TEXT,
                        CourseId INTEGER,
                        Gender TEXT
                    );

                    -- Thêm dữ liệu mẫu (nếu chưa có)
                    INSERT OR IGNORE INTO Users (Username, Password) VALUES ('admin', '123456'), ('user1', 'password');
                    INSERT OR IGNORE INTO Courses (Id, Name) VALUES (1, 'Lập trình C#'), (2, 'Lập trình Web'), (3, 'Cơ sở dữ liệu');
                ";
                command.ExecuteNonQuery();
            }
        }
    }
}