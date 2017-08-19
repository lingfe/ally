﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//导入命名空间
using BLL;
using Model;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HPSF;
using Microsoft.CSharp;

using System.Web;
using NPOI;
using NPOI.HSSF;
using NPOI.POIFS;
using NPOI.Util;
using System.Collections;

namespace dome1
{
    public partial class employeeLevelMain : Form
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public employeeLevelMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        employeeLevelSql sql = new employeeLevelSql();
        employeeLevelLogSql sqlLog = new employeeLevelLogSql();


        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //调用函数,加载，刷新
            this.dataNode();
            toolStripStatusLabel1.Text += PublicField.userName;
            //加载日志
            this.getLog("");
        }


        /// <summary>
        /// 递归，得到父级数量
        /// </summary>
        /// <param name="parent_id">父级id</param>
        /// <param name="count">行数</param>
        /// <param name="list">数据集合</param>
        public int getParentCount(int parent_id, int count, List<employeeLevel> list)
        {
            List<employeeLevel> parent1 = sql.getEmployeeLevelParent(parent_id);
            btn_Superior.Text = "上级(" + count + ")";
            foreach (employeeLevel temp1 in parent1) {
               count++;
               list.Add(temp1);
               this.getParentCount(temp1.Parent_id,count,list);

            }
            return count;
            
        }


        /// <summary>
        /// 遍历根节点数组,窗体加载,初始化,刷新
        /// </summary>
        public void dataNode()
        {
            //得到所有员工等级信息
            List<employeeLevel> list = sql.getEmployeeLevel();
            List<employeeLevel> childEmployeelevel = new List<employeeLevel>();
            //清空现有数据
            treeView1.Nodes.Clear();
            listView1.Items.Clear();
            cob_parent_id.Items.Clear();

            //树数据
            List<employeeLevel> st = sql.getEmployeeLevel();
            TreeNode root = new TreeNode();
            root.Text = "所有员工";
            root.Tag = 0;

            cob_parent_id.Items.Add("0.一级");
            //循环遍历
            foreach (employeeLevel temp in list)
            {
                ListViewItem lv = new ListViewItem(temp.Id.ToString());
                lv.SubItems.Add(temp.UserName);
                lv.SubItems.Add(temp.JobNumber);



                //统计上下级
                //统计上级总数
                int id = temp.Id;
                int count = 0;
                List<employeeLevel> parent1 = new List<employeeLevel>();
                count=this.getParentCount(temp.Parent_id, count, parent1);
                lv.SubItems.Add(count.ToString());

                //根据id查询,统计下级总数
                List<employeeLevel> Achildlist = sql.getEmployeeLevelAchild(id);
                lv.SubItems.Add(Achildlist.Count.ToString());
                //统计商户数量
                List<employeeLevel> Merchantlist = sql.getEmployeeLevelMerchant(id);;
                lv.SubItems.Add(Merchantlist.Count.ToString());

                lv.SubItems.Add(temp.Stock + "");
                lv.SubItems.Add(temp.DateTime.ToString());
                lv.SubItems.Add(temp.Parent_id + "");
                lv.Tag = temp;

                listView1.Items.Add(lv);
                

            }
            //默认
            cob_parent_id.SelectedIndex = 0;
            cob_shuxin.SelectedIndex = 0;
            

            //遍历
            foreach (employeeLevel temp in st)
            {
                foreach (employeeLevel temp1 in st)
                {
                    if (temp.Id == temp1.Parent_id)
                    {
                        if (temp.ChildEmployeeLevel == null)
                        {
                            temp.ChildEmployeeLevel = new List<employeeLevel>();

                        }

                        temp.ChildEmployeeLevel.Add(temp1);
                        childEmployeelevel.Add(temp1);

                    }
                }
            }
            //清空子节点
            foreach (employeeLevel temp in childEmployeelevel)
            {
                st.Remove(temp);

            }
            
            //得到树
            foreach (employeeLevel temp1 in st)
            {
                TreeNode no = new TreeNode();
                no.Text = temp1.UserName;
                root.Nodes.Add(no);
                this.node(temp1, no);
            }
            treeView1.Nodes.Add(root);
            treeView1.ExpandAll();
            listView1.Focus();
        }
        
        
        /// <summary>
        /// 递归,得到员工树
        /// </summary>
        /// <param name="temp">当前节点name</param>
        /// <param name="root">树</param>
        public void node(employeeLevel temp, TreeNode root)
        {
            //循环遍历
            if (temp.ChildEmployeeLevel != null)
            {
                foreach (employeeLevel temp1 in temp.ChildEmployeeLevel)
                {
                    TreeNode node2 = new TreeNode();
                    node2.Text = temp1.UserName;
                    root.Nodes.Add(node2);
                    this.node(temp1, node2);
                }
            }
            
        }

        /// <summary>
        /// ListView的单机事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //判断是否选中了
            if (listView1.SelectedItems.Count != 0)
            {
                int id = Int32.Parse(listView1.SelectedItems[0].SubItems[0].Text);
                //id
                lab_id.Text = id.ToString();
                //父id
                lab_parent_id.Text = listView1.SelectedItems[0].SubItems[8].Text;
                this.ListView_TreeView_click(id, "listView1");

                this.setIdGetNumber(id);
            } 
        }
        /// <summary>
        /// 统计上下级,以及商户
        /// </summary>
        /// <param name="id">id</param>
        public void setIdGetNumber(int id) {
            
            //统计上级总数
            int count = 0;
            List<employeeLevel> parent1 = new List<employeeLevel>();
            this.getParentCount(Int32.Parse(listView1.SelectedItems[0].SubItems[8].Text), count, parent1);


            //根据id查询,统计下级总数
            List<employeeLevel> Achildlist = sql.getEmployeeLevelAchild(id);
            btn_subordinate.Text = "下级(" + Achildlist.Count + ")";
            //统计商户数量
            List<employeeLevel> Merchantlist = sql.getEmployeeLevelMerchant(id);
            btn_Merchant.Text = "商户(" + Merchantlist.Count + ")";
        }

        /// <summary>
        /// ListView和Treeview所用的方法
        /// </summary>
        /// <param name="num"></param>
        public void ListView_TreeView_click(int num,string strName) {

            if (strName == "listView1") {
                //根据id查询
                List<employeeLevel> st = sql.getEmployeeLevelParent(num);
                listView3.Items.Clear();
                //遍历
                foreach (employeeLevel temp in st)
                {
                    ListViewItem lv = new ListViewItem(temp.Id.ToString());
                    lv.SubItems.Add(temp.UserName);
                    lv.SubItems.Add(temp.JobNumber);
                    lv.SubItems.Add(temp.Stock + "");
                    lv.SubItems.Add(temp.Parent_id + "");
                    lv.SubItems.Add(temp.DateTime.ToString());
                    lv.SubItems.Add(temp.Shuxin);
                    lv.Tag = temp;

                    lab_name.Text = temp.UserName;
                    lab_jobNumber.Text = temp.JobNumber;
                    lab_stock.Text = "产品(" + temp.Stock + ")";
                    listView3.Items.Add(lv);
                }
            }
            else if (strName == "listView3")
            {
                //根据id查询
                List<employeeLevel> st = sql.getEmployeeLevelParent(num);
                //遍历
                foreach (employeeLevel temp in st)
                {
                    txt_userName.Text = temp.UserName;
                    txt_jobNumber.Text = temp.JobNumber;
                    txt_stock.Text = temp.Stock;
                    lab_id.Text = temp.Id.ToString();
                    lab_parent_id.Text = temp.Parent_id.ToString();

                    lab_name.Text = temp.UserName;
                    lab_jobNumber.Text = temp.JobNumber;
                    lab_stock.Text = "产品(" + temp.Stock + ")";

                }
            }
        
        }

        /// <summary>
        /// 清空文本框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            txt_userName.Text = "";
            txt_jobNumber.Text = "";
            txt_stock.Text = "";
            cob_parent_id.Text = "";

        }

        /// <summary>
        /// 显示上级信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Superior_Click(object sender, EventArgs e)
        {
            //得到父id
            string parent_id = lab_parent_id.Text;
            //判断是否选中了
            if (parent_id.Length != 0 && parent_id != "")
            {
                int num = Int32.Parse(parent_id);
                //根据id查询
                List<employeeLevel> parent1=new List<employeeLevel>();
                int count = 0;
                this.getParentCount(num, count, parent1);

                //清空listView3
                listView3.Items.Clear();
                //循环遍历
                foreach (employeeLevel temp in parent1)
                {
                    ListViewItem lv = new ListViewItem(temp.Id.ToString());
                    lv.SubItems.Add(temp.UserName);
                    lv.SubItems.Add(temp.JobNumber);
                    lv.SubItems.Add(temp.Stock + "");
                    lv.SubItems.Add(temp.Parent_id + "");
                    lv.SubItems.Add(temp.DateTime.ToString());
                    lv.Tag = temp;

                    //添加到listView3
                    listView3.Items.Add(lv);

               }

            }
        }

        /// <summary>
        /// 显示下级信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_subordinate_Click(object sender, EventArgs e)
        {
            //得到id
            string id = lab_id.Text;
            //判断是否选中了
            if (id.Length != 0 && id != "")
            {
                int num = Int32.Parse(id);
                //根据id查询
                List<employeeLevel> st = sql.getEmployeeLevelAchild(num);
                btn_subordinate.Text = "下级(" + st.Count + ")";
                listView3.Items.Clear();
                //循环遍历
                foreach (employeeLevel temp in st)
                {
                    ListViewItem lv = new ListViewItem(temp.Id.ToString());
                    lv.SubItems.Add(temp.UserName);
                    lv.SubItems.Add(temp.JobNumber);
                    lv.SubItems.Add(temp.Stock + "");
                    lv.SubItems.Add(temp.Parent_id + "");
                    lv.SubItems.Add(temp.DateTime.ToString());
                    lv.Tag = temp;

                    listView3.Items.Add(lv);

                }


            }
        }

        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //判断是否选中了
            if (listView1.SelectedItems.Count != 0 || listView3.SelectedItems.Count != 0)
            {
                //定义实体变量
                employeeLevel emp = new employeeLevel();
                //得到数据
                emp.Id = Int32.Parse(lab_id.Text);
                emp.UserName = txt_userName.Text;
                if ("".Equals(emp.UserName)||emp.UserName == null) {
                    MessageBox.Show("请填写名称！"); return;
                }
                emp.Stock = txt_stock.Text;
                if ("".Equals(emp.Stock) || emp.Stock == null) {
                    MessageBox.Show("请填写产品库存！"); return;
                }
                emp.JobNumber = txt_jobNumber.Text;
                if ("".Equals(emp.JobNumber) || emp.JobNumber == null)
                {
                    MessageBox.Show("工号不能为空！"); return;
                }
                //父id
                emp.Parent_id = Int32.Parse(cob_parent_id.Text.Split('.')[0].ToString());
                if (emp.Id == emp.Parent_id) {
                    MessageBox.Show("不能选择自己作为父级！"); return;
                }
                emp.Shuxin = cob_shuxin.Text;
                List<employeeLevel> getJobNumberList = sql.getJobNumberList(emp.JobNumber);
                if (getJobNumberList.Count >= 1)
                {
                    MessageBox.Show("工号重复了！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    return;
                }
                //调用Update
                int tt = sql.setEmployeeLevelUpdate(emp);
                if (tt == 1)
                {
                    //调用函数,加载，刷新
                   // this.dataNode();
                    MessageBox.Show("修改成功！");
                }
                else
                {
                    MessageBox.Show("修改失败！");
                }
            }
            else {
                MessageBox.Show("请选择要修改的员工！");
            }

        }
        
        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //定义实体变量
            employeeLevel emp = new employeeLevel();
            //得到数据
            emp.UserName = txt_userName.Text;
            if ("".Equals(emp.UserName) || emp.UserName == null)
            {
                MessageBox.Show("请填写名称！"); return;
            }
            emp.Stock = txt_stock.Text;
            if ("".Equals(emp.Stock) || emp.Stock == null)
            {
                MessageBox.Show("请填写产品库存！"); return;
            }
            emp.JobNumber = txt_jobNumber.Text;
            if ("".Equals(emp.JobNumber) || emp.JobNumber == null)
            {
                MessageBox.Show("工号不能为空！"); return;
            }
            //父id
            emp.Parent_id = Int32.Parse(cob_parent_id.Text.Split('.')[0].ToString());
            emp.Shuxin = cob_shuxin.Text;

            List<employeeLevel> getJobNumberList = sql.getJobNumberList(emp.JobNumber);
            if (getJobNumberList.Count >= 1)
            {
                MessageBox.Show("工号重复了！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                return;
            }
            //调用add
            int tt = sql.setEmployeeLeveLAdd(emp);
            if (tt == 1)
            {
                //调用函数,加载，刷新
                //this.dataNode();

                employeeLevelLog empLog = new employeeLevelLog();
                List<employeeLevel> st= sql.getEmployeeLevelParent(emp.Parent_id);
                if(st!=null && st.Count!=0){
                    empLog.JobNumber = st[0].JobNumber;
                    empLog.UserName = st[0].UserName;
                    empLog.Stock = emp.Stock;
                    this.setLog(empLog);
                }
                MessageBox.Show("添加成功！");
            }
            else {
                MessageBox.Show("添加失败！");
            }

        }

        /// <summary>
        /// 新增员工操作日志
        /// </summary>
        /// <param name="empLog"></param>
        public void setLog(employeeLevelLog empLog)
        {
            //添加日志
            int tt=sqlLog.setEmployeeLevelLogAdd(empLog);
            if (tt == 1)
            {
                this.getLog("");
            }
            else {
                MessageBox.Show("日志添加错误！");
            }
        }

        /// <summary>
        /// 根据时间得到日志,时间可以为空。如果时间为空，默认得到今天的日志
        /// </summary>
        /// <param name="dateTime">一个时间</param>
        public void getLog(String dateTime)
        {
            //得到集合
            List<employeeLevelLog> list = sqlLog.getEmployeeLevelLog(dateTime);
            int num1 = 0, num2 = list.Count;
            string str="(个)";
            //清空
            listView2.Items.Clear();
            //循环遍历
            foreach (employeeLevelLog temp in list)
            {
                num1 += Int32.Parse(temp.Stock);
            }
            //循环遍历
            foreach (employeeLevelLog temp in list)
            {
                ListViewItem lv = new ListViewItem(temp.DateTime.ToString());
                lv.SubItems.Add(temp.UserName);
                lv.SubItems.Add(temp.JobNumber);
                lv.SubItems.Add(temp.AddMerchant+str);
                lv.SubItems.Add(temp.AddSubordinate + str);
                lv.SubItems.Add(temp.Stock + str);

                num2--;

                lv.SubItems.Add(num2+str);
                lv.SubItems.Add(num1 + str);
                lv.SubItems.Add(num2+str);

                num1 -= Int32.Parse(temp.Stock);
                lv.Tag = temp;

                listView2.Items.Add(lv);

            }
        }

        /// <summary>
        /// 关键字搜索，如果为空则得到全部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_search_Click(object sender, EventArgs e)
        {
            //得到搜索条件
            string searchKey=txt_search.Text;
            List<employeeLevel> st = sql.getEmployeeLeveLikeSearch(searchKey);
            //清空
            listView1.Items.Clear();
            //循环遍历
            foreach (employeeLevel temp in st)
            {
                ListViewItem lv = new ListViewItem(temp.Id.ToString());
                lv.SubItems.Add(temp.UserName);
                lv.SubItems.Add(temp.JobNumber);
                lv.SubItems.Add(temp.Stock + "");
                lv.SubItems.Add(temp.Parent_id + "");

                lv.SubItems.Add(temp.DateTime.ToString());
                lv.SubItems.Add(temp.Shuxin);
                lv.Tag = temp;

                listView1.Items.Add(lv);

            }
        }

        /// <summary>
        /// listView3的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count != 0)
            {
                int num = Int32.Parse(listView3.SelectedItems[0].SubItems[0].Text);
                //id
                lab_id.Text = num.ToString();
                //父id
                lab_parent_id.Text = listView3.SelectedItems[0].SubItems[4].Text;
                this.ListView_TreeView_click(num, "listView3");
                //统计上下级
                //统计上级总数
                int count = 0;
                List<employeeLevel> parent1 = new List<employeeLevel>();
                this.getParentCount(Int32.Parse(listView3.SelectedItems[0].SubItems[4].Text), count, parent1);


                //根据id查询,统计下级总数
                List<employeeLevel> Achildlist = sql.getEmployeeLevelAchild(num);
                btn_subordinate.Text = "下级(" + Achildlist.Count + ")";
                //统计商户数量
                List<employeeLevel> Merchantlist = sql.getEmployeeLevelMerchant(num);
                btn_Merchant.Text = "商户(" + Merchantlist.Count + ")";
            }
        }

        /// <summary>
        /// 删除员工信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click_1(object sender, EventArgs e)
        {

            //判断是否选中了
            if (listView1.SelectedItems.Count != 0 || listView3.SelectedItems.Count != 0)
            {
                //得到id
                string id = lab_id.Text;
                //判断是否选中了
                if (id.Length != 0 && id != "")
                {
                    DialogResult dr = MessageBox.Show("您确定要删除吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr != DialogResult.OK)
                    {
                        //点取消的代码
                        return;
                    }
                    int num = Int32.Parse(id);
                    int tt = sql.setEmployeeLevelDelete(num);
                    if (tt != -1)
                    {
                        //调用函数,加载，刷新
                        this.dataNode();
                        MessageBox.Show("删除成功!");
                    }
                    else
                    {
                        MessageBox.Show("删除失败!");
                    }
                }

            }
            else {
                MessageBox.Show("请选择要删除的员工！"); 
            }

        }

        /// <summary>
        /// 根据日期查询日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataTime_CloseUp(object sender, EventArgs e)
        {
           DateTime date= DateTime.Parse(dataTime.Text);
           this.getLog(date.ToString());
        }

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Merchant_Click(object sender, EventArgs e)
        {
                        //得到id
            string id = lab_id.Text;
            //判断是否选中了
            if (id.Length != 0 && id != "")
            {
                int num = Int32.Parse(id);
                List<employeeLevel> Merchantlist = sql.getEmployeeLevelMerchant(num);
                btn_Merchant.Text = "商户(" + Merchantlist.Count + ")";
                listView3.Items.Clear();
                //循环遍历
                foreach (employeeLevel temp in Merchantlist)
                {
                    ListViewItem lv = new ListViewItem(temp.Id.ToString());
                    lv.SubItems.Add(temp.UserName);
                    lv.SubItems.Add(temp.JobNumber);
                    lv.SubItems.Add(temp.Stock + "");
                    lv.SubItems.Add(temp.Parent_id + "");
                    lv.SubItems.Add(temp.DateTime.ToString());
                    lv.Tag = temp;

                    listView3.Items.Add(lv);

                }

            }
            

        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataNode();
        }

        /// <summary>
        /// 选为上级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 选为上级ToolStripMenuItem_Click(object sender, EventArgs e)
        {
             //判断是否选中了
            if (listView1.SelectedItems.Count != 0 )
            {
                if (listView1.SelectedItems[0].SubItems[6].Text == "商户") {
                    MessageBox.Show("商户不能增加商户！");
                    return;
                }

                cob_parent_id.Items.Clear();
                cob_parent_id.Items.Add("0.一级");
                cob_parent_id.Items.Add(listView1.SelectedItems[0].SubItems[0].Text + "." + listView1.SelectedItems[0].SubItems[1].Text);
                cob_parent_id.SelectedIndex = 1;
                txt_jobNumber.Text = "";
                txt_stock.Text = "";
                txt_userName.Text = "";
                cob_shuxin.SelectedIndex = 1;
            }
            else {
                MessageBox.Show("请选择要设置的上级！");
            }
        }

        /// <summary>
        /// 关闭程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void employeeLevelMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 选择修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {   
            //判断是否选中了
            if (listView1.SelectedItems.Count != 0)
            {
                //得到id
                int id = Int32.Parse(listView1.SelectedItems[0].SubItems[0].Text);
                this.ListView_TreeView_click(id, "listView3");
            }

        }

        /// <summary>
        /// 当前时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = DateTime.Now + "";
            toolStripStatusLabel2.ForeColor = Color.Red;
            toolStripStatusLabel2.ForeColor = Color.Blue;
        }

        /// <summary>
        /// 单机某个节点时发生的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.Addtreeview();
        }

        /// <summary>
        /// 单击节点执行
        /// </summary>
        public void Addtreeview()
        {
            TreeNode de = treeView1.SelectedNode;
            if (treeView1.SelectedNode != null && de.Level>0)
            {
                //MessageBox.Show(de.Text, "选中了");
                return;
            }
        }

        /// <summary>
        /// 导出员工到Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 导出到ExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (MemoryStream ms = this.Export(sql.getEmployeeLevelDataTable(), "员工信息"))
            {
                string saveFileName = "";                    //保存路径
                //bool fileSaved = false;
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.DefaultExt = "xls";              //默认文件类型
                saveDialog.Filter = "Excel文件|*.xls";       //可选择的文件类型
                saveDialog.FileName = "test";               //默认的文件名
                saveDialog.ShowDialog();                    //打开文件窗口
                saveFileName = saveDialog.FileName;

                if (saveFileName.IndexOf(":") < 0) return; //被点了取消 
                //Microsoft.Office.Interop.Excel.Application xlApp;

               //保存
                SaveToFile(ms, saveFileName);
          
                
            }
            
        }

        /// <summary>
        /// 保存Excel文档流到文件
        ///
        /// Excel文档流
        /// 文件名
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="fileName"></param>
        private static void SaveToFile(MemoryStream ms, string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                    data = null;
                }

                MessageBox.Show("导出Excel成功！位置："+fileName);
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
            }

        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream Export()
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="strHeaderText">Excel表头文本（例如：车辆列表）</param>
        public  MemoryStream Export(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "文件作者信息";               //填加xls文件作者信息
                si.ApplicationName = "创建程序信息";      //填加xls文件创建程序信息
                si.LastAuthor = "最后保存者信息";          //填加xls文件最后保存者信息
                si.Comments = "作者信息";                   //填加xls文件作者信息
                si.Title = "标题信息";                  //填加xls文件标题信息
                si.Subject = "主题信息";                //填加文件主题信息
                si.CreateDateTime = System.DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion

            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                    }
                    #endregion

                    #region 列头及样式
                    {
                        IRow headerRow = sheet.CreateRow(1);
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                    }
                    #endregion

                    rowIndex = 2;
                }
                #endregion

                #region 填充内容
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            System.DateTime dateV;
                            System.DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }
                #endregion

                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                sheet.Dispose();
                return ms;
            }
        }

        /// <summary>
        /// 读取excel ,默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable Import(string strFileName)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }
    }
}
