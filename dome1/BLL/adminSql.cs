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
    /// 管理员数据业务逻辑层
    /// </summary>
    public class adminSql
    {
        /// <summary>
        ///定义 数据库帮助对象
        /// </summary>
        DBHeple ple = new DBHeple();

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="userName">管理员名称</param>
        /// <param name="password">管理员登录密码</param>
        /// <returns>数据</returns>
        public List<admin> setAdmingLogin(string userName, string password) {
            return ple.getAdmin(string.Format("SELECT * FROM admin a WHERE a.userName='{0}' AND a.password='{1}'",userName,password));
        }
    }
}
