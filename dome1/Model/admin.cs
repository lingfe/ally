using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 管理员表
    /// </summary>
   public  class admin
    {
        private int id;//    	`id` INT(11) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '管理员表id标识',
       /// <summary>
        /// 管理员表id标识
       /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string userName;//	`userName` VARCHAR(255) NOT NULL COMMENT '管理员名称',
       /// <summary>
        /// 管理员名称
       /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        private string password;//	`password` VARCHAR(255) NOT NULL COMMENT '管理员登录密码',
       /// <summary>
        /// 管理员登录密码
       /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        private string isBl;//	`isBl` VARCHAR(50) NOT NULL DEFAULT '否' COMMENT '是否删除',
       /// <summary>
        /// 是否删除
       /// </summary>
        public string IsBl
        {
            get { return isBl; }
            set { isBl = value; }
        }
        private DateTime createTime;//	`createTime` DATETIME DEFAULT NOW() COMMENT '创建时间',  
       /// <summary>
        /// 创建时间
       /// </summary>
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
    }
}
