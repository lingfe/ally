using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Model;
using MySql.Data.MySqlClient;

namespace ADL
{
    /// <summary>
    /// SQL帮助类
    /// </summary>
    public class DBHeple
    {

        public DBHeple() {
            con.Open();
        
        }
        #region  建立Sql server 数据库连接
        //SqlConnection con = new SqlConnection("server=.;database=dome1;uid=root;pwd=root;");
        //SqlCommand cmd;
        //SqlDataReader dr;
        #endregion

        #region  建立MySql数据库连接
        /// <summary>
        /// 服务器1：ip=119.23.59.68; port=330;
        /// 服务器2：ip=39.108.118.48;port=3306;
        /// 本地  : ip=192.168.1.104;port=3306;
        /// </summary>
        MySqlConnection con = new MySqlConnection("server=39.108.118.48;port=3306;user id=dahuo;CharSet=utf8;password=dahuo;database=ally"); //根据自己的设置http://sosoft.cnblogs.com/
        
        MySqlCommand cmd;
        MySqlDataReader dr;
        MySqlDataAdapter da;
        #endregion

        /// <summary>
        /// 返回影响的行数的方法
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>影响的行数</returns>
        public int GetExecuteNonQuery(string sql)
        {
            int tt = -1;
            try
            {
                
                cmd = new MySqlCommand(sql, con);
                tt = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                Console.WriteLine(ex.Message);
            }
            finally
            {
               // con.Close();
            }
            return tt;
        }

        /// <summary>
        /// 得到dataTable数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable getEmployeeLevelDataTable(string sql)
        {
            DataTable ds = null;
            try
            {
                da = new MySqlDataAdapter(sql, con);
                ds = new DataTable();
                da.Fill(ds);

                return ds;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                Console.WriteLine(ex.Message);

            }
            finally
            {
                //con.Close();
            }
            return ds;
        }

        /// <summary>
        /// 定义查询所有员工等级的方法
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>List集合，包含employeeLevel数据</returns>
        public List<employeeLevel> getEmployeeLevel(string sql)
        {
            List<employeeLevel> st = new List<employeeLevel>();

            try
            {
                //con.Open();
                cmd = new MySqlCommand(sql, con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    employeeLevel fo = new employeeLevel();
                    fo.Id = Convert.ToInt32(dr["id"]);
                    fo.UserName = dr["userName"].ToString();
                    fo.Identity = dr["identity"].ToString();
                    fo.JobNumber = dr["jobNumber"].ToString();
                    fo.Level = Convert.ToInt32(dr["level"]);
                    fo.Parent_id = Convert.ToInt32(dr["parent_id"]);
                    fo.Stock = dr["stock"].ToString();
                    fo.DateTime = DateTime.Parse(dr["dateTime"].ToString());
                    fo.Shuxin = dr["shuxin"].ToString();

                    st.Add(fo);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Closed) {
                    con.Open();
                }
                Console.WriteLine(ex.Message);

            }
            finally
            {
                //con.Close();
            }

            return st;
        }

        /// <summary>
        /// 定义查询所有员工等级操作日志的方法
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>List集合，包含employeeLevelLog数据</returns>
        public List<employeeLevelLog> getEmployeeLevelLog(string sql)
        {
            List<employeeLevelLog> st = new List<employeeLevelLog>();

            try
            {
                //con.Open();
                cmd = new MySqlCommand(sql, con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    employeeLevelLog fo = new employeeLevelLog();
                    fo.Id = Convert.ToInt32(dr["id"]);
                    fo.UserName = dr["userName"].ToString();
                    fo.DateTime = DateTime.Parse(dr["dateTime"].ToString());
                    fo.JobNumber = dr["jobNumber"].ToString();
                    fo.AddMerchant = dr["addMerchant"].ToString();
                    fo.AddSubordinate = dr["addSubordinate"].ToString();
                    fo.Stock = dr["stock"].ToString();

                    st.Add(fo);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                Console.WriteLine(ex.Message);

            }
            finally
            {
                //con.Close();
            }

            return st;
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>List集合，包含admin数据</returns>
        public List<admin> getAdmin(string sql) {
            List<admin> st = new List<admin>();
            try
            {   
                //con.Open();
                cmd = new MySqlCommand(sql, con);
                dr = cmd.ExecuteReader();
                while (dr.Read()) {
                    admin ad = new admin();
                    ad.Id = Int32.Parse(dr["id"].ToString());
                    ad.UserName = dr["userName"].ToString();
                    ad.Password = dr["password"].ToString();
                    ad.IsBl = dr["isBl"].ToString();
                    ad.CreateTime = DateTime.Parse(dr["createTime"].ToString());

                    st.Add(ad);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                Console.WriteLine(ex);
            }
            finally {
                //con.Clone();
            }
            return st;
        }
    }
}
