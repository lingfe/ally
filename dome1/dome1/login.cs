using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using Model;

namespace dome1
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        //定义变量
        adminSql sql = new adminSql();


        /// <summary>
        /// 点击登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_login_Click(object sender, EventArgs e)
        {
            string userName=textBox1.Text;
            if (userName == "") {
                MessageBox.Show("用户名不能为空！");
                Console.WriteLine("*******提示:用户名不能为空!");
                return;
            }
            string password = textBox2.Text;
            if (password == "") {
                MessageBox.Show("密码不能为空!");
                Console.WriteLine("*******提示:密码不能为空!");
                return;
            }

            List<admin> st=sql.setAdmingLogin(userName,password);
            
            if (st.Count > 0)
            {
                MessageBox.Show("登录成功！");
                Console.WriteLine("*******登录成功!");
                PublicField.userName = st[0].UserName;
                employeeLevelMain main = new employeeLevelMain();
                main.Show();

                sql.close();
                this.Visible = false;
            }
            else {
                MessageBox.Show("登录失败！");
                Console.WriteLine("*******登录失败！");
            }

        }

        /// <summary>
        /// 取消登录，关闭程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_close_Click(object sender, EventArgs e)
        {
            Console.WriteLine("*******再见！");
            this.Close();
            sql.close();
            Application.Exit();
        }

        private void login_Load(object sender, EventArgs e)
        {
            this.getxin();
            Console.WriteLine("*******准备登录了吗？*********");


        }

        public void getxin() {

            int i, j, k, l, m;
            char c='*';

            for (i = 1; i <= 5; i++) Console.WriteLine();

            for (i = 1; i <=3; i++) {
                for (j = 1; j <= 32 - 2 * i; j++) Console.Write(" ");
                for (k = 1; k <= 4 * i + 1; k++) Console.Write("*");
                for (l = 1; l <= 13 - 4 * i; l++) Console.Write(" ");
                for (m = 1; m <= 4 *1 + i; m++) Console.Write("*");
                Console.WriteLine();
            }

            for (i = 1; i <=3; i++)
            {
                for (j = 1; j <= 24+1; j++) Console.Write(" ");
                for (k = 1; k <=29; k++) Console.Write("*");
                Console.WriteLine();
            }

            for (i = 7; i>=1; i--)
            {
                for (j = 1; j <= 40-2*i; j++) Console.Write(" ");
                for (k = 1; k <= 4*i-1; k++) Console.Write("*");
                Console.WriteLine();
            }

            for (i = 1; i <= 39; i++) Console.Write(" ");
            Console.WriteLine("*");

            for (i = 1; i <= 5; i++) Console.WriteLine();
        }

    }
}
