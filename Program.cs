using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sale_Management.Forms;
using Sale_Management.Forms.Manager;

namespace Sale_Management
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AdminForm("admin", "manager"));
            //Application.Run(new SalerForm("saler001", "saler"));
            //bool shouldContinue = true;

            //do
            //{
            //    using (LoginForm loginForm = new LoginForm())
            //    {
            //        if (loginForm.ShowDialog() == DialogResult.OK)
            //        {
            //            // LoginForm sẽ tự động mở form chính và đóng khi logout
            //            // Không cần xử lý gì thêm ở đây
            //        }
            //        else
            //        {
            //            // Người dùng hủy đăng nhập hoặc đóng form
            //            shouldContinue = false;
            //        }
            //    }
            //} while (shouldContinue);
        }
    }
}
