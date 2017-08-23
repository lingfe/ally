using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 员工等级实体
    /// </summary>
    public class employeeLevel
    {


        private DateTime dateTime;//  `dateTime` DATETIME DEFAULT NOW()  COMMENT '录入时间',
        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }
        private List<employeeLevel> childEmployeeLevel;

        public List<employeeLevel> ChildEmployeeLevel
        {
            get { return  childEmployeeLevel; }
            set { childEmployeeLevel = value; }
        }

        private int id;// `id` INT(11) UNSIGNED NOT NULL AUTO_INCREMENT,

        /// <summary>
        /// 员工等级ID
        /// </summary>
        public int Id
        {   
            get { return id; }
            set { id = value; }
        }
        private string jobNumber;// `jobNumber` VARCHAR(255) DEFAULT '无工号' COMMENT '工号',

        /// <summary>
        /// 员工工号
        /// </summary>
        public string JobNumber
        {
            get { return jobNumber; }
            set { jobNumber = value; }
        }
        private string userName;//  `userName` VARCHAR(255) NOT NULL DEFAULT '' COMMENT '名称',

        /// <summary>
        /// 员工名称
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        private int parent_id;//  `parent_id` INT(11) NOT NULL COMMENT '父id，父级',

        /// <summary>
        /// 员工父级ID
        /// </summary>
        public int Parent_id
        {
            get { return parent_id; }
            set { parent_id = value; }
        }
        private string stock;//  `stock` VARCHAR(10)  DEFAULT '' COMMENT '库存',

        /// <summary>
        /// 库存
        /// </summary>
        public string Stock
        {
            get { return stock; }
            set { stock = value; }
        }
        private int level;//  `level` INT(11)  COMMENT '等级',

        /// <summary>
        /// 员工等级编号
        /// </summary>
        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        private string identity;//  `identity` VARCHAR(255) DEFAULT '' COMMENT '身份证',

        /// <summary>
        /// 员工身份证
        /// </summary>
        public string Identity
        {
            get { return identity; }
            set { identity = value; }
        }

        private string shuxin;//属性
        /// <summary>
        /// 属性
        /// </summary>
        public string Shuxin
        {
            get { return shuxin; }
            set { shuxin = value; }
        }


        private string superiorNumber;//上级数量
        /// <summary>
        /// 上级数量
        /// </summary>
        public string SuperiorNumber
        {
            get { return superiorNumber; }
            set { superiorNumber = value; }
        }


        private string subordinateNumber;//下级梳理
        /// <summary>
        /// 下级梳理
        /// </summary>
        public string SubordinateNumber
        {
            get { return subordinateNumber; }
            set { subordinateNumber = value; }
        }

        private string merchantNumber;//商户数量
        /// <summary>
        /// 商户数量
        /// </summary>
        public string MerchantNumber
        {
            get { return merchantNumber; }
            set { merchantNumber = value; }
        }
    }
}
