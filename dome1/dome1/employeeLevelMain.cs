using System;
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

using System.Threading;//引用空间名称

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
            
            //默认
            cob_parent_id.Items.Add("0.一级");
            cob_parent_id.SelectedIndex = 0;

            listView1.Focus();
            //调用函数,加载，刷新
            this.dataListView("");
            this.dataTreeView(null);
            //加载日志
            this.getLog("");
            //管理员
            toolStripStatusLabel1.Text += PublicField.userName;

        }


        /// <summary>
        /// 递归，得到上级数量
        /// </summary>
        /// <param name="parent_id">父级id</param>
        /// <param name="count">行数</param>
        /// <param name="list">数据集合</param>
        public void getParentCount(int parent_id, List<employeeLevel> list)
        {
            //根据需要统计的父id，得到数据
            List<employeeLevel> parent1 = sql.getEmployeeLevelParent(parent_id);
            //循环遍历
            foreach (employeeLevel temp1 in parent1) {
               list.Add(temp1);
               //再次调用，直到没有父级为止
               this.getParentCount(temp1.Parent_id,list);

            }
        }

        /// <summary>
        /// 递归，得到下级数量
        /// </summary>
        /// <param name="emp">实体</param>
        public void getSubordinateCount(int id,List<employeeLevel> st)
        {
            //根据需要统计的id，得到数据
            List<employeeLevel> parent1 = sql.getEmployeeLevelAchild(id);
            
            //循环遍历
            foreach (employeeLevel temp1 in parent1)
            {
                st.Add(temp1);
                //再次调用，直到没有下级为止
                this.getSubordinateCount(temp1.Id, st);
            }

        }

        /// <summary>
        /// ListView
        /// </summary>
        /// <param name="dateTime"></param>
        public void dataListView(string dateTime) {
            //得到所有员工等级信息
            List<employeeLevel> list = sql.getEmployeeLevel(dateTime);
            //清空现有数据
            listView1.Items.Clear();
            //循环遍历
            foreach (employeeLevel temp in list)
            {
                ListViewItem lv = new ListViewItem(temp.Id.ToString());
                lv.SubItems.Add(temp.UserName);                     //名称
                lv.SubItems.Add(temp.JobNumber);                    //工号
                lv.SubItems.Add(temp.SuperiorNumber);               //上级数量
                lv.SubItems.Add(temp.SubordinateNumber);            //下级数量
                lv.SubItems.Add(temp.MerchantNumber);               //商户数量
                lv.SubItems.Add(temp.Stock + "");                   //产品
                lv.SubItems.Add(temp.DateTime.ToString());          //录入时间
                lv.SubItems.Add(temp.Parent_id + "");               //父id
                lv.Tag = temp;

                listView1.Items.Add(lv);
            }
        }

        /// <summary>
        /// 遍历根节点数组,窗体加载,初始化,刷新
        /// </summary>
        public void dataTreeView(string dateTime)
        {
            //定义变量
            List<employeeLevel> childEmployeelevel = new List<employeeLevel>();
            //清空现有数据
            treeView1.Nodes.Clear();

            //树数据
            List<employeeLevel> st = sql.getEmployeeLevel(dateTime);
            TreeNode root = new TreeNode();
            root.Text = "所有员工";
            root.Tag = 0;

            //遍历集合
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
                no.Text = temp1.UserName+"("+temp1.SubordinateNumber+")";
                root.Nodes.Add(no);
                this.getNode(temp1, no);
            }

            //添加到树
            treeView1.Nodes.Add(root);
            treeView1.ExpandAll();
        }

        /// <summary>
        /// 递归,得到员工树
        /// </summary>
        /// <param name="temp">当前节点name</param>
        /// <param name="root">树</param>
        public void getNode(employeeLevel temp, TreeNode root)
        {
            
            //循环遍历
            if (temp.ChildEmployeeLevel != null)
            {
                foreach (employeeLevel temp1 in temp.ChildEmployeeLevel)
                {
                    TreeNode node2 = new TreeNode();
                    node2.Text = temp1.UserName + "(" + temp1.SubordinateNumber + ")";
                    root.Nodes.Add(node2);
                    this.getNode(temp1, node2);
                }
            }
        }

        /// <summary>
        /// ListView1的单机事件
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
                //上级数量
                btn_Superior.Text = "上级(" + listView1.SelectedItems[0].SubItems[3].Text + ")";
                //下级数量
                btn_subordinate.Text = "下级(" + listView1.SelectedItems[0].SubItems[4].Text + ")";
                //商户梳理
                btn_Merchant.Text = "商户(" + listView1.SelectedItems[0].SubItems[5].Text + ")";
                //产品
                lab_stock.Text = "商户(" + listView1.SelectedItems[0].SubItems[6].Text + ")";

                //遍历数据
                this.ListView_TreeView_click(id, "listView1");
            } 
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
                    lv.SubItems.Add(temp.UserName);                     //名称
                    lv.SubItems.Add(temp.JobNumber);                    //工号
                    lv.SubItems.Add(temp.SuperiorNumber);               //上级数量
                    lv.SubItems.Add(temp.SubordinateNumber);            //下级数量
                    lv.SubItems.Add(temp.MerchantNumber);               //商户数量
                    lv.SubItems.Add(temp.Stock + "");                   //产品
                    lv.SubItems.Add(temp.DateTime.ToString());          //录入时间
                    lv.SubItems.Add(temp.Parent_id + "");               //父id
                    lv.Tag = temp;

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
                    txt_MerchantNumber.Text = temp.MerchantNumber;
                    
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
                this.getParentCount(num, parent1);

                //清空listView3
                listView3.Items.Clear();
                //循环遍历
                foreach (employeeLevel temp in parent1)
                {
                    ListViewItem lv = new ListViewItem(temp.Id.ToString());
                    lv.SubItems.Add(temp.UserName);                     //名称
                    lv.SubItems.Add(temp.JobNumber);                    //工号
                    lv.SubItems.Add(temp.SuperiorNumber);               //上级数量
                    lv.SubItems.Add(temp.SubordinateNumber);            //下级数量
                    lv.SubItems.Add(temp.MerchantNumber);               //商户数量
                    lv.SubItems.Add(temp.Stock + "");                   //产品
                    lv.SubItems.Add(temp.DateTime.ToString());          //录入时间
                    lv.SubItems.Add(temp.Parent_id + "");               //父id
                    lv.Tag = temp;

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
                listView3.Items.Clear();
                //循环遍历
                foreach (employeeLevel temp in st)
                {
                    ListViewItem lv = new ListViewItem(temp.Id.ToString());
                    lv.SubItems.Add(temp.UserName);                     //名称
                    lv.SubItems.Add(temp.JobNumber);                    //工号
                    lv.SubItems.Add(temp.SuperiorNumber);               //上级数量
                    lv.SubItems.Add(temp.SubordinateNumber);            //下级数量
                    lv.SubItems.Add(temp.MerchantNumber);               //商户数量
                    lv.SubItems.Add(temp.Stock + "");                   //产品
                    lv.SubItems.Add(temp.DateTime.ToString());          //录入时间
                    lv.SubItems.Add(temp.Parent_id + "");               //父id
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
            if (listView1.SelectedItems.Count != 0 )
            {
                //定义实体变量
                employeeLevel emp = new employeeLevel();
                emp.SuperiorNumber = listView1.SelectedItems[0].SubItems[3].Text;
                emp.SubordinateNumber = listView1.SelectedItems[0].SubItems[4].Text;
                emp.MerchantNumber = listView1.SelectedItems[0].SubItems[5].Text;

                //验证后得到数据,
                if (this.vadata(emp) == null) return;
                //父id
                emp.Parent_id = Int32.Parse(cob_parent_id.Text.Split('.')[0].ToString());
                if (emp.Parent_id == emp.Id)
                {
                    MessageBox.Show("不能选择自己作为父级！"); return;
                }

                DialogResult dr = MessageBox.Show("确定之前请注意防止关系循环！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr != DialogResult.OK) return; //点取消的代码  
                //调用Update
                int tt = sql.setEmployeeLevelUpdate(emp);
                if (tt == 1)
                {
                    //调用函数,加载，刷新
                    this.dataListView("");
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
        /// 基础验证
        /// </summary>
        /// <param name="emp"></param>
        public employeeLevel vadata(employeeLevel emp)
        {
            emp.Id = lab_id.Text!=""? Int32.Parse(lab_id.Text):0;
            emp.Parent_id = lab_parent_id.Text!=""?Int32.Parse(lab_parent_id.Text):0;
            emp.UserName = txt_userName.Text;

            if ("".Equals(emp.UserName) || emp.UserName == null)
            {
                MessageBox.Show("请填写名称！"); return null;
            }
            emp.Stock = txt_stock.Text;
            if ("".Equals(emp.Stock) || emp.Stock == null)
            {
                MessageBox.Show("请填写产品库存！"); return null;
            }
            emp.JobNumber = txt_jobNumber.Text;
            if ("".Equals(emp.JobNumber) || emp.JobNumber == null)
            {
                MessageBox.Show("工号不能为空！"); return null;
            }
            emp.MerchantNumber = txt_MerchantNumber.Text;
            if ("".Equals(emp.MerchantNumber) || emp.MerchantNumber == null)
            {
                MessageBox.Show("商户数量不能为空！"); return null;
            }
            //父id
            emp.Parent_id = Int32.Parse(cob_parent_id.Text.Split('.')[0].ToString());
            List<employeeLevel> getJobNumberList = sql.getJobNumberList(emp.JobNumber);
            if (getJobNumberList.Count >= 1)
            {
                MessageBox.Show("工号重复了！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                return null;
            }
            return emp;
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
            //验证后得到数据,
            if (this.vadata(emp) == null) return;
            //调用add
            int tt = sql.setEmployeeLeveLAdd(emp);
            if (tt == 1)
            {
                //日志信息
                employeeLevelLog empLog = new employeeLevelLog();
                List<employeeLevel> st= sql.getEmployeeLevelParent(emp.Parent_id);
                if(st!=null && st.Count!=0){
                    empLog.JobNumber = st[0].JobNumber;
                    empLog.UserName = st[0].UserName;
                    empLog.Stock = emp.Stock;
                    this.setLog(empLog);
                }
                //调用函数,加载，刷新
                //调用函数,加载，刷新
                this.dataListView("");
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
                lv.SubItems.Add(temp.UserName);                     //名称
                lv.SubItems.Add(temp.JobNumber);                    //工号
                lv.SubItems.Add(temp.SuperiorNumber);               //上级数量
                lv.SubItems.Add(temp.SubordinateNumber);            //下级数量
                lv.SubItems.Add(temp.MerchantNumber);               //商户数量
                lv.SubItems.Add(temp.Stock + "");                   //产品
                lv.SubItems.Add(temp.DateTime.ToString());          //录入时间
                lv.SubItems.Add(temp.Parent_id + "");               //父id
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
                int id = Int32.Parse(listView3.SelectedItems[0].SubItems[0].Text);
                //id
                lab_id.Text = id.ToString();
                //父id
                lab_parent_id.Text = listView3.SelectedItems[0].SubItems[8].Text;
                //上级数量
                btn_Superior.Text = "上级(" + listView3.SelectedItems[0].SubItems[3].Text + ")";
                //下级数量
                btn_subordinate.Text = "下级(" + listView3.SelectedItems[0].SubItems[4].Text + ")";
                //商户数量
                btn_Merchant.Text = "商户(" + listView3.SelectedItems[0].SubItems[5].Text + ")";

                //遍历数据
                this.ListView_TreeView_click(id, "listView3");
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
            if (listView1.SelectedItems.Count != 0)
            {
                employeeLevel emp = new employeeLevel();
                emp.SuperiorNumber = listView1.SelectedItems[0].SubItems[3].Text;
                emp.SubordinateNumber = listView1.SelectedItems[0].SubItems[4].Text;
                emp.MerchantNumber = listView1.SelectedItems[0].SubItems[5].Text;
                //得到id
                string id = lab_id.Text;
                //判断是否选中了
                if (id.Length != 0 && id != "")
                {
                    DialogResult dr = MessageBox.Show("您确定要删除吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr != DialogResult.OK) return; //点取消的代码  
                    //得到id
                    int num = Int32.Parse(id);
                    emp=sql.getEmployeeLevelParent(num)[0];
                    int tt = sql.setEmployeeLevelDelete(num);
                    if (tt != -1)
                    {
                        //调用函数,加载，刷新
                        this.dataListView("");
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
        /// 根据日期查询日志，员工信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataTime_CloseUp(object sender, EventArgs e)
        {
           DateTime date= DateTime.Parse(dataTime.Text);
            //根据时间查询日志，数据
           this.getLog(date.ToString());
           //调用函数,加载，刷新
           this.dataListView(date.ToString());
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
                    lv.SubItems.Add(temp.UserName);                     //名称
                    lv.SubItems.Add(temp.JobNumber);                    //工号
                    lv.SubItems.Add(temp.SuperiorNumber);               //上级数量
                    lv.SubItems.Add(temp.SubordinateNumber);            //下级数量
                    lv.SubItems.Add(temp.MerchantNumber);               //商户数量
                    lv.SubItems.Add(temp.Stock + "");                   //产品
                    lv.SubItems.Add(temp.DateTime.ToString());          //录入时间
                    lv.SubItems.Add(temp.Parent_id + "");               //父id
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
            //调用函数,加载，刷新
            this.dataListView("");
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
            sql.close();
            sqlLog.close();
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
            if (de.Level == 0)
            {
                if (de.Text == "所有员工") {
                   List<employeeLevel> st= sql.getEmployeeLevel(null);
                   listView1.Items.Clear();
                   //循环遍历
                   foreach (employeeLevel temp in st)
                   {
                       ListViewItem lv = new ListViewItem(temp.Id.ToString());
                       lv.SubItems.Add(temp.UserName);                     //名称
                       lv.SubItems.Add(temp.JobNumber);                    //工号
                       lv.SubItems.Add(temp.SuperiorNumber);               //上级数量
                       lv.SubItems.Add(temp.SubordinateNumber);            //下级数量
                       lv.SubItems.Add(temp.MerchantNumber);               //商户数量
                       lv.SubItems.Add(temp.Stock + "");                   //产品
                       lv.SubItems.Add(temp.DateTime.ToString());          //录入时间
                       lv.SubItems.Add(temp.Parent_id + "");               //父id
                       lv.Tag = temp;

                       listView1.Items.Add(lv);

                   }
                }
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
        /// </summary>
        /// <param name="ms">Excel文档流</param>
        /// <param name="fileName">文件名</param>
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
                si.Author = "零风";               //填加xls文件作者信息
                si.ApplicationName = "盟友1.10";      //填加xls文件创建程序信息
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
                            string strName = "";
                            switch (column.Caption)
                            {
                                case "id":
                                    strName = "编号";
                                    break;
                                case "jobNumber":
                                    strName = "工号";
                                    break;
                                case "userName":
                                    strName = "名称";
                                    break;
                                case "parent_id":
                                    strName = "父id";
                                    break;
                                case "stock":
                                    strName = "产品";
                                    break;
                                case "level":
                                    strName = "等级";
                                    break;
                                case "identity":
                                    strName = "身份证";
                                    break;
                                case "dateTime":
                                    strName = "录入时间";
                                    break;
                                case "shuxin":
                                    strName = "属性";
                                    break;
                                case "superiorNumber":
                                    strName = "上级数量";
                                    break;
                                case "subordinateNumber":
                                    strName = "下级数量";
                                    break;
                                case "merchantNumber":
                                    strName = "商户数量";
                                    break;
                                default:
                                    break;
                            }
                            headerRow.CreateCell(column.Ordinal).SetCellValue(strName);
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

        /// <summary>
        /// 树刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 刷新ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //调用函数,加载，刷新
            this.dataTreeView(null);
        }

        /// <summary>
        /// 统计上下级数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 统计上下级数量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr =  MessageBox.Show("统计上下级数量,等待时间较长是否开始?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes) return; //点取消的代码  
            //得到所有数据
            List<employeeLevel> st = sql.getEmployeeLevel(null);

            //遍历集合
            foreach (employeeLevel temp in st)
            {
                //MessageBox.Show("开始统计"+temp.UserName+",是否开始?");
                //统计上级总数
                int count = 0;
                List<employeeLevel> ttst = new List<employeeLevel>();
                this.getParentCount(temp.Parent_id, ttst);
                temp.SuperiorNumber = ttst.Count.ToString();


                ttst = new List<employeeLevel>();
                //调用统计下级
                this.getSubordinateCount(temp.Id, ttst);
                temp.SubordinateNumber = ttst.Count.ToString();
                //执行修改
                sql.setEmployeeLevelUpdate(temp);

                
            }
            MessageBox.Show("统计结束！准备刷新..");

            //调用树
            this.dataTreeView(null);
        }


    }
}
