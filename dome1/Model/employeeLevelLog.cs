using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 员工等级操作日志表
    /// </summary>
   public class employeeLevelLog
    {
        private int id;//          `id` INT(11) UNSIGNED NOT NULL AUTO_INCREMENT,

        /// <summary>
        /// 员工等级操作日志表id标识
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string jobNumber;//  `jobNumber` VARCHAR(255) DEFAULT '无工号' COMMENT '工号',

        /// <summary>
        /// 工号
        /// </summary>
        public string JobNumber
        {
            get { return jobNumber; }
            set { jobNumber = value; }
        }
        private string userName;//  `userName` VARCHAR(255) NOT NULL DEFAULT '' COMMENT '名称',
        /// <summary>
        /// 名称
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        private DateTime dateTime;//  `dateTime` DATETIME DEFAULT NOW()  COMMENT '日志时间',
        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }
        private string addMerchant;//  `addMerchant` VARCHAR(50)  DEFAULT 1 COMMENT '新增商户数量',
        /// <summary>
        /// 新增商户数量
        /// </summary>
        public string AddMerchant
        {
            get { return addMerchant; }
            set { addMerchant = value; }
        }
        private string addSubordinate;//  `addSubordinate` VARCHAR(50)  DEFAULT 1 COMMENT '新增下级数量',
        /// <summary>
        /// 新增下级数量
        /// </summary>
        public string AddSubordinate
        {
            get { return addSubordinate; }
            set { addSubordinate = value; }
        }
        private string stock;//  `stock` VARCHAR(50)  DEFAULT 0 COMMENT '产品出库数量',
        /// <summary>
        /// 产品出库数量
        /// </summary>
        public string Stock
        {
            get { return stock; }
            set { stock = value; }
        }
    }
}
