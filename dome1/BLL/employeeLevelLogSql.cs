using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADL;
using Model;

namespace BLL
{
    /// <summary>
    /// 操作员工等级日志的业务逻辑层
    /// </summary>
   public class employeeLevelLogSql
    {
        /// <summary>
        ///定义 数据库帮助对象
        /// </summary>
        DBHeple ple = new DBHeple();

        /// <summary>
        /// 返回所有操作日志结果
        /// </summary>
        /// <returns>结果</returns>
        public List<employeeLevelLog> getEmployeeLevelLog(String datetime)
        {
            List<employeeLevelLog> st = new List<employeeLevelLog>();
            if (datetime == null || datetime.ToString() == "" || datetime.ToString().Length == 0)
            {
                return st = ple.getEmployeeLevelLog(string.Format("SELECT * FROM employeelevelLog WHERE DATEDIFF(DATETIME,NOW())=0 ORDER BY id DESC"));
            }
            else {
                return st = ple.getEmployeeLevelLog(string.Format("SELECT * FROM employeelevelLog WHERE DATEDIFF(DATETIME,'{0}')=0 ORDER BY id DESC", datetime.ToString()));
            }
            
        }

        /// <summary>
        /// 添加操作员工等级日志
        /// </summary>
        /// <param name="emp">实体</param>
        /// <returns>添加状态</returns>
        public int setEmployeeLevelLogAdd(employeeLevelLog empLog)
        {
            string sql = string.Format("INSERT  INTO `employeelevelLog`(`jobNumber`,`userName`,`dateTime`,`addMerchant`,`addSubordinate`,`stock`) " +
            "VALUES ('{0}','{1}',DEFAULT,DEFAULT,DEFAULT,'{2}')", empLog.JobNumber, empLog.UserName,empLog.Stock);
            return ple.GetExecuteNonQuery(sql);
        }
    }
}
