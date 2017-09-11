using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dome1
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.Title = "盟友系统";


            Console.WriteLine("**********************************************************************");
            Console.WriteLine("******************************欢迎使用盟友系统************************");
            Console.WriteLine("**********************************************************************");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new login());
            
        }
    }
}
