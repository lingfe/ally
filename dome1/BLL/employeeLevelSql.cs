﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADL;
using Model;

namespace BLL
{
    /// <summary>
    /// 操作员工等级的业务逻辑层
    /// </summary>
    public class employeeLevelSql
    {
        /// <summary>
        ///定义 数据库帮助对象
        /// </summary>
        DBHeple ple = new DBHeple();

        /// <summary>
        /// 返回所有员工结果
        /// </summary>
        /// <returns>结果</returns>
        public List<employeeLevel> getEmployeeLevel()
        {
            List<employeeLevel> st = new List<employeeLevel>();
            return st = ple.getEmployeeLevel(string.Format("SELECT * FROM employeeLevel"));
        }

        /// <summary>
        /// 返回员工父级结果
        /// </summary>
        /// <param name="fo">查询条件</param>
        /// <returns>结果</returns>
        public List<employeeLevel> getEmployeeLevelParent(int id)
        {
            List<employeeLevel> st = new List<employeeLevel>();
            return st = ple.getEmployeeLevel(string.Format("SELECT * FROM employeeLevel t WHERE t.id={0}", id));
        }

        /// <summary>
        /// 返回员工子级结果
        /// </summary>
        /// <param name="fo">查询条件</param>
        /// <returns>结果</returns>
        public List<employeeLevel> getEmployeeLevelAchild(int parent_id)
        {
            List<employeeLevel> st = new List<employeeLevel>();
            return st = ple.getEmployeeLevel(string.Format("SELECT * FROM employeeLevel t WHERE t.parent_id={0}  and shuxin='员工'", parent_id));
        }

        /// <summary>
        /// 返回员工的商户
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public List<employeeLevel> getEmployeeLevelMerchant(int parent_id) {
            List<employeeLevel> st = new List<employeeLevel>();
            return st = ple.getEmployeeLevel(string.Format("SELECT * FROM employeeLevel t WHERE t.parent_id={0}", parent_id));
        }

        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="emp">实体</param>
        /// <returns>添加状态</returns>
        public int setEmployeeLeveLAdd(employeeLevel emp) {
            string sql = string.Format(" INSERT  INTO `employeelevel`(`jobNumber`,`userName`,`parent_id`,`stock`,`level`,`identity`,`dateTime`,`shuxin`) " +
                "VALUES  ('{0}','{1}',{2},'{3}',{4},'{5}',DEFAULT,'{6}')", emp.JobNumber, emp.UserName, emp.Parent_id, emp.Stock, emp.Level, emp.Identity,emp.Shuxin);
            return ple.GetExecuteNonQuery(sql);
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>删除状态</returns>
        public int setEmployeeLevelDelete(int id) {
            return ple.GetExecuteNonQuery(string.Format("delete FROM employeelevel where id={0}", id));
        }

        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="emp">实体</param>
        /// <returns>修改状态</returns>
        public int setEmployeeLevelUpdate(employeeLevel emp) {
            string sql = string.Format("update employeelevel set jobNumber='{0}',userName='{1}',parent_id={2},stock='{3}',level='{4}',identity='{5}',shuxin='{6}' where id={7}",
                emp.JobNumber,emp.UserName,emp.Parent_id,emp.Stock,emp.Level,emp.Identity,emp.Shuxin,emp.Id);
            return ple.GetExecuteNonQuery(sql);
        }

        /// <summary>
        /// 模糊搜索
        /// </summary>
        /// <param name="searchKey">搜索条件</param>
        /// <returns>结果</returns>
        public List<employeeLevel> getEmployeeLeveLikeSearch(string searchKey) {
            string sql = string.Format(" SELECT * FROM employeeLevel t WHERE 1=1 AND t.userName LIKE '%{0}%' OR t.jobNumber LIKE '%{1}%' OR t.stock LIKE '%{2}%' OR t.identity LIKE '%{3}%' OR t.shuxin LIKE '%{4}%'", searchKey, searchKey, searchKey, searchKey,searchKey);
            return ple.getEmployeeLevel(sql);
        }

    }
}
