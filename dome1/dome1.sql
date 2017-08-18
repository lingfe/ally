
DROP DATABASE `dome1`;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`dome1` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `dome1`;

/*员工等级表*/
DROP TABLE IF EXISTS `employeeLevel`;
 
CREATE TABLE `employeeLevel` (
  `id` INT(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  `jobNumber` VARCHAR(255) DEFAULT '无工号' COMMENT '工号',
  `userName` VARCHAR(255) NOT NULL DEFAULT '' COMMENT '名称',
  `parent_id` INT(11) NOT NULL COMMENT '父id，父级',
  `stock` VARCHAR(10)  DEFAULT '' COMMENT '库存',
  `level` INT(11)  COMMENT '等级',
  `identity` VARCHAR(255) DEFAULT '' COMMENT '身份证',
  `dateTime` DATETIME DEFAULT NOW()  COMMENT '录入时间',
  PRIMARY KEY (`id`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;
 
 INSERT  INTO `employeelevel`(`id`,`jobNumber`,`userName`,`parent_id`,`stock`,`level`,`identity`,`dateTime`) VALUES 
 (1,'无工号','李四',0,'5',0,'52222819925614567874',DEFAULT),
 (2,'无工号','张三',0,'15',0,'52222456878942351325',DEFAULT),
 (3,'无工号','王二',1,'100',1,'522224545646564564565',DEFAULT),
 (4,'无工号','麻子',2,'5',1,'522228199456125645645',DEFAULT),
 (5,'无工号','安其拉',3,'5',2,'52222456123456456487',DEFAULT);

 --查询王二的父级
 SELECT * FROM employeeLevel t WHERE t.userName='王二'
 SELECT * FROM employeeLevel t WHERE t.id=2;
 
 --查询王二的子级
  SELECT * FROM employeeLevel t WHERE t.name='王二'  ;
 SELECT * FROM employeeLevel t WHERE t.parent_id=3;
 
 --统计上下级
 SELECT COUNT(*) FROM employeeLevel t WHERE t.id=2;
 SELECT COUNT(*) FROM employeeLevel t WHERE t.parent_id=1;
 
 --搜索
 SELECT * FROM employeeLevel t WHERE 1=1 AND t.userName LIKE '%0%' OR t.jobNumber LIKE '%0%'
 

/*2。员工等级操作日志表*/
DROP TABLE IF EXISTS `employeeLevelLog`;
CREATE TABLE `employeeLevelLog` (
  `id` INT(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  `jobNumber` VARCHAR(255) DEFAULT '无工号' COMMENT '工号',
  `userName` VARCHAR(255) NOT NULL DEFAULT '' COMMENT '名称',
  `dateTime` DATETIME DEFAULT NOW()  COMMENT '日志时间',
  `addMerchant` VARCHAR(50)  DEFAULT 1 COMMENT '新增商户数量',
  `addSubordinate` VARCHAR(50)  DEFAULT 1 COMMENT '新增下级数量',
  `stock` VARCHAR(50)  DEFAULT 0 COMMENT '产品出库数量',
  PRIMARY KEY (`id`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

--查询当天纪录
SELECT * FROM employeeLevelLog WHERE DATEDIFF(DATETIME,NOW())=0 ORDER BY id DESC;
 
/*3.管理员表*/
DROP TABLE IF EXISTS `admin`;
CREATE TABLE admin(
	`id` INT(11) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '管理员表id标识',
	`userName` VARCHAR(255) NOT NULL COMMENT '管理员名称',
	`password` VARCHAR(255) NOT NULL COMMENT '管理员登录密码',
	`isBl` VARCHAR(50) NOT NULL DEFAULT '否' COMMENT '否',
	`createTime` DATETIME DEFAULT NOW() COMMENT '创建时间',  
	PRIMARY KEY (`id`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

--登录
SELECT * FROM admin a WHERE a.userName='张三' AND a.password='123456';`dome1`
SELECT * FROM admin a WHERE a.userName='张三' AND a.password='123456';