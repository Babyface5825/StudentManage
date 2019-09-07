using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel;
using StudentManage.Models;

namespace StudentManage.Models
{
    #region 虚表和根
    /// <summary>
    /// 所有表的根类，只有主码ID
    /// </summary>
    public class tblInfo
    {
        [Column(TypeName = "BIGINT")] //特性
        public long ID { get; set; }
    }
    /// <summary>
    /// 带伪删除
    /// </summary>
    public class tblDelInfo : tblInfo
    {
        /// <summary>
        /// 伪删除
        /// </summary>        
        public bool DelFlag { get; set; }
        public tblDelInfo()
        {
            DelFlag = false;
        }
    }
    /// <summary>
    /// 带名称，备注
    /// </summary>
    public class tblNameRemarkInfo : tblDelInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "说明")]
        public string Remark { get; set; }
    }
    /// <summary>
    /// 简单信息表
    /// </summary>
    public class tblFirstSimpleInfo : tblNameRemarkInfo
    {
        /// <summary>
        /// 排序号
        /// </summary>
        public short TheOrder { set; get; }
    }
    /// <summary>
    /// 带点更多内容的简单信息表
    /// </summary>
    public class tblSimpleInfo : tblFirstSimpleInfo
    {
        [Display(Name = "代码")]
        public string Code { get; set; }
        [Display(Name = "合并同类项")]
        public string JoinNames { get; set; }
        [Display(Name = "合并后的ID")]
        public long JoinID { set; get; }
        public bool ScanFlag { set; get; }
        public tblSimpleInfo()
        {
            JoinID = 0;
            ScanFlag = false;
        }
    }
    /// <summary>
    /// 原始导入基本表的根类
    /// </summary>
    public class tblOriginTable : tblDelInfo
    {
        /// <summary>
        /// 代码
        /// </summary>
        [Display(Name = "代码")]
        public string DM { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string MC { set; get; }
        public int YearCode { set; get; }
        public long ProvinceID { set; get; }
        //[ForeignKey("ProvinceID")]
        //public virtual Td_dqdm dq { get; set; }
        public long RelationID { set; get; }
    }

    /// <summary>
    /// 树节点
    /// </summary>
    public class tblTreeInfo : tblNameRemarkInfo
    {

        [Column(TypeName = "BIGINT")]
        public long ParentIndex { get; set; }

        [Column(TypeName = "smallint")]
        public short TheOrder { set; get; }

        [Column(TypeName = "smallint")]
        public short TheLevel { set; get; }

        public int ChildNum { set; get; }

        public bool IsLeaf { set; get; }

        [Display(Name = "编码")]
        public string Code { set; get; }
        [NotMapped]
        public string ParentName { set; get; }

        public tblTreeInfo()
        {
            ParentIndex = -1;
            TheOrder = 0;
            TheLevel = 1;
            ChildNum = 0;
            IsLeaf = true;
            DelFlag = false;
        }

    }
    /// <summary>
    /// 文件信息表
    /// </summary>
    public class tblFileTable : tblInfo
    {
        public long RelationID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [Column(TypeName = "smallint")]
        public Int16 TheOrder { get; set; }
    }
    #endregion

    #region 实体表
    #region 1 用户个人信息表（tblUserInfo）
    /// <summary>
    /// 1，用户个人信息表
    /// </summary>
    [Table("tblUserInfo")]
    public class tblUserInfo : tblNameRemarkInfo
    {
        /// <summary>
        /// 登录账号
        /// </summary>        
        [Display(Name = "用户名")]
        public string UserICode { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string UserIPsd { get; set; }
        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime? UserIRegisterDate { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [MaxLength(2)]
        public string UserISex { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string UserIIDCard { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>        
        public DateTime? UserIBirth { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string UserIAddress { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string UserIPostalCode { get; set; }
        /// <summary>
        /// 编号
        /// </summary>        
        public string UserINumber { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Phone]
        public string UserIMobilePhone { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        [EmailAddress]
        public string UserIeMail { get; set; }
        /// <summary>
        /// 审核情况
        /// </summary>
        public bool VerifyFlag { set; get; }
        /// <summary>
        /// 单位全称
        /// </summary>
        public string WholeDepartmentName { set; get; }

        [Column(TypeName = "BIGINT")]
        /// <summary>
        /// 职务编号
        /// </summary>        
        public long? UserIDutyIID { get; set; }
        [ForeignKey("UserIDutyIID")]
        public virtual tblDutyInfo DutyInfo { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>        
        public long? UserIDepartmentIID { get; set; }
        [ForeignKey("UserIDepartmentIID")]
        public virtual tblDepartmentInfo DepartmentInfo { get; set; }
        public virtual List<tblUserAuthorRelation> authorRelations { get; set; }
        //public virtual List<tblChannelContent> contents { get; set; }
        //public virtual List<tblContestInfo> ContestInfo { set; get; }
        //public virtual List<tblAllocate> Allocate { set; get; }
        //public virtual List<tblRegisterGroup> tblRegisterGroup { set; get; }
        //public virtual List<tblRegisterUser> tblRegisterUser { get; set; }
        [NotMapped]
        public string ValidatePic { set; get; }
        [NotMapped]
        public bool? SearchVerifyFlag { set; get; }
        [NotMapped]
        public string SearchDepartmentIDs { set; get; }

        public tblUserInfo()
        {
            authorRelations = new List<tblUserAuthorRelation>();
            //contents = new List<tblChannelContent>();
            //ContestInfo = new List<tblContestInfo>();
            //Allocate = new List<tblAllocate>();
            ////tblAllocateALL = new List<tblAllocate>();
            //tblRegisterGroup = new List<tblRegisterGroup>();
            //tblRegisterUser = new List<tblRegisterUser>();
            VerifyFlag = false;
            SearchVerifyFlag = null;
        }
    }
    #endregion
    #region 2 权限关系信息表（tblUserAuthorRelation）
    /// <summary>
    /// 2．权限关系信息表
    /// </summary>
    [Table("tblUserAuthorRelation")]
    public class tblUserAuthorRelation : tblInfo
    {
        [Column(TypeName = "BIGINT")]
        public long AuthorID { get; set; }
        [Column(TypeName = "BIGINT")]
        public long UserID { get; set; }
        [ForeignKey("AuthorID")]
        public virtual tblAuthorizationInfo author { get; set; }
        [ForeignKey("UserID")]
        public virtual tblUserInfo user { get; set; }
    }
    #endregion
    #region 3 角色设置表（tblAuthorizationInfo）
    /// <summary>
    /// 3，角色信息表
    /// </summary>
    [Table("tblAuthorizationInfo")]
    public class tblAuthorizationInfo : tblSimpleInfo
    {
        public virtual List<tblUserAuthorRelation> userRelations { get; set; }
        //public virtual List<tblChannelAuthorRelation> channelRelations { get; set; }
        public virtual List<tblOperationAuthorRelation> operationRelations { get; set; }
        public tblAuthorizationInfo()
        {
            userRelations = new List<tblUserAuthorRelation>();
            //channelRelations = new List<tblChannelAuthorRelation>();
            operationRelations = new List<tblOperationAuthorRelation>();
        }
    }
    #endregion
    #region 4 菜单角色关系表（tblOperationAuthorRelation）
    /// <summary>
    /// 4 菜单角色关系表
    /// </summary>
    [Table("tblOperationAuthorRelation")]
    public class tblOperationAuthorRelation : tblInfo
    {
        [Column(TypeName = "BIGINT", Order = 0)]
        public long AuthorID { get; set; }
        [Column(TypeName = "BIGINT", Order = 1)]
        public long OperationID { get; set; }
        [ForeignKey("AuthorID")]
        public virtual tblAuthorizationInfo author { get; set; }
        [ForeignKey("OperationID")]
        public virtual tblOperationInfo operation { get; set; }
        public bool SearchFlag { get; set; }
        public bool AddFlag { get; set; }
        public bool EditFlag { get; set; }
        public bool DeleteFlag { get; set; }
    }
    #endregion
    #region 5 菜单信息表（tblOperationInfo）
    /// <summary>
    /// 5 菜单信息表
    /// </summary>
    [Table("tblOperationInfo")]
    public class tblOperationInfo : tblTreeInfo
    {

        public virtual List<tblOperationAuthorRelation> authorRelations { get; set; }
        public tblOperationInfo()
        {
            authorRelations = new List<tblOperationAuthorRelation>();
        }
    }
    #endregion
    #region 6 单位部门信息表（tblDepartmentInfo）
    /// <summary>
    /// 6 部门班级信息表
    /// </summary>
    [Table("tblDepartmentInfo")]
    public class tblDepartmentInfo : tblTreeInfo
    {
        [Display(Name = "详细地址")]
        public string DepartmentIADD { get; set; }
        [Display(Name = "邮政编码")]
        public string DepartmentIPostalCode { get; set; }
        [Display(Name = "部门电话")]
        public string DepartmentIPhone { get; set; }
        [Display(Name = "部门传真")]
        public string DepartmentIFax { get; set; }
        [Display(Name = "部门信箱")]
        public string DepartmentIeMail { get; set; }
        public string ZYIDs { get; set; }
        [NotMapped]
        public string ZYNames { set; get; }
        public virtual List<tblUserInfo> users { get; set; }
        public long GxID { get; set; }
        [ForeignKey("GxID")]
        public virtual Td_gxdm gx { get; set; }
        public tblDepartmentInfo()
        {
            users = new List<tblUserInfo>();
            ZYIDs = " ";
        }
    }
    #endregion
    #region 7 职务信息表（tblDutyInfo）
    /// <summary>
    /// 7 职务信息表
    /// </summary>
    [Table("tblDutyInfo")]
    public class tblDutyInfo : tblSimpleInfo
    {
        public virtual List<tblUserInfo> users { get; set; }
        public tblDutyInfo()
        {
            users = new List<tblUserInfo>();
        }
    }
    #endregion

    #region 9 原始数据情况表（tblOriginDataPackageInfo）

    /// <summary>
    /// 9 原始数据情况表（tblOriginDataPackageInfo）
    /// </summary>
    [Table("tblOriginDataPackageInfo")]
    public class tblOriginDataPackageInfo : tblDelInfo
    {
        [Column(TypeName = "BIGINT")]
        public long UserID { get; set; }
        [Column(TypeName = "BIGINT")]
        public long GxID { get; set; }
        public DateTime UploadDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string FileType { get; set; }
        public string OriginName { get; set; }
        public string NowAddress { get; set; }
        public string HandleFlag { get; set; }
        public int SuccessNumber { get; set; }
        public int FailureNumber { get; set; }
        [ForeignKey("UserID")]
        public virtual tblUserInfo user { get; set; }
        [ForeignKey("GxID")]
        public virtual Td_gxdm Gx { get; set; }

        [NotMapped]
        public bool HandleWay { get; set; }
        [Column(TypeName = "BIGINT")]
        public long ParentDepartmentID { get; set; }

        public tblOriginDataPackageInfo()
        {
            UploadDate = DateTime.Now;            
            HandleFlag = "正在处理";
            SuccessNumber = 0;
            FailureNumber = 0;
            HandleWay = true;
        }
    }

    #endregion
    
    #region 11 毕业类别代码原始表（Td_bylbdm_Origin）
        /// <summary>
        /// 11 毕业类别代码原始表
        /// </summary>
        [Table("Td_bylbdm_Origin")]
    public class Td_bylbdm_Origin : tblOriginTable
    {        
        //[ForeignKey("RelationID")]
        //public virtual Td_bylbdm bylb { get; set; }
        public Td_bylbdm_Origin()
        {

        }
    }
    #endregion
    #region 12 毕业类别代码（Td_bylbdm）
    /// <summary>
    ///  12 毕业类别代码
    /// </summary>
    [Table("Td_bylbdm")]
    public class Td_bylbdm : tblSimpleInfo
    {
        public Td_bylbdm()
        {

        }
    }
    #endregion
    #region 13 层次代码原始表（Td_ccdm_Origin）
    /// <summary>
    /// 13 层次代码原始表（Td_ccdm_Origin）
    /// </summary>
    [Table("Td_ccdm_Origin")]
    public class Td_ccdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_ccdm cc { get; set; }
        public Td_ccdm_Origin()
        {

        }
    }
    #endregion
    #region 14 层次代码(Td_ccdm)
    /// <summary>
    ///  14 Td_ccdm
    /// </summary>
    [Table("Td_ccdm")]
    public class Td_ccdm : tblSimpleInfo
    {
        public Td_ccdm()
        {

        }
    }
    #endregion
    #region 15 成绩项代码原始表（Td_cjxdm_Origin）
    /// <summary>
    /// 15 成绩项代码原始表（Td_cjxdm_Origin）
    /// </summary>
    [Table("Td_cjxdm_Origin")]
    public class Td_cjxdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_cjxdm gkcjx { get; set; }
        public Td_cjxdm_Origin()
        {

        }
    }
    #endregion
    #region 16 成绩项代码（Td_cjxdm）
    /// <summary>
    ///  16 成绩项代码（Td_cjxdm）
    /// </summary>
    [Table("Td_cjxdm")]
    public class Td_cjxdm : tblSimpleInfo
    {
        public Td_cjxdm()
        {

        }
    }
    #endregion
    #region 17 残障类别代码原始表（Td_czlbdm_Origin）
    /// <summary>
    /// 17 残障类别代码原始表
    /// </summary>
    [Table("Td_czlbdm_Origin")]
    public class Td_czlbdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_czlbdm czlb { get; set; }
        public Td_czlbdm_Origin()
        {

        }
    }
    #endregion
    #region 18 残障类别代码(Td_czlbdm)
    /// <summary>
    /// 18 残障类别代码(Td_czlbdm)
    /// </summary>
    [Table("Td_czlbdm")]
    public class Td_czlbdm : tblSimpleInfo
    {
        public Td_czlbdm()
        {

        }
    }
    #endregion
    #region 19 地区代码原始表（Td_dqdm_Origin）
    /// <summary>
    /// 19 地区代码原始表（Td_dqdm_Origin）
    /// </summary>
    [Table("Td_dqdm_Origin")]
    public class Td_dqdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_dqdm dqReal { get; set; }
        public Td_dqdm_Origin()
        {

        }
    }
    #endregion
    #region 20 地区代码（Td_dqdm）
    /// <summary>
    /// 20 地区代码表
    /// </summary>
    [Table("Td_dqdm")]
    public class Td_dqdm : tblTreeInfo
    {
        [Display(Name = "合并同类项")]
        public string JoinNames { get; set; }
        [Display(Name = "合并后的ID")]
        public long JoinID { set; get; }
        public bool ScanFlag { set; get; }

        public Td_dqdm()
        {
            JoinID = 0;
            ScanFlag = false;
        }
    }
    #endregion
    #region 21 户籍类别代码原始表（Td_zzmmdm_Origin）
    /// <summary>
    /// 21 户籍类别代码原始表
    /// </summary>
    [Table("Td_hjlbdm_Origin")]
    public class Td_hjlbdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_hjlbdm hjlb { get; set; }
        public Td_hjlbdm_Origin()
        {

        }
    }
    #endregion
    #region 22 户籍类别代码(Td_hjlbdm)
    /// <summary>
    /// 22 户籍类别代码(Td_hjlbdm)
    /// </summary>
    [Table("Td_hjlbdm")]
    public class Td_hjlbdm : tblSimpleInfo
    {
        public Td_hjlbdm()
        {

        }
    }
    #endregion
    #region 23 计划类别代码原始表（Td_jhlbdm_Origin）
    /// <summary>
    /// 23 计划类别代码原始表
    /// </summary>
    [Table("Td_jhlbdm_Origin")]
    public class Td_jhlbdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_jhlbdm jhlb { get; set; }
        public Td_jhlbdm_Origin()
        {

        }
    }
    #endregion
    #region 24 计划类别代码（Td_jhlbdm）
    /// <summary>
    /// 24 计划类别代码（Td_jhlbdm）
    /// </summary>
    [Table("Td_jhlbdm")]
    public class Td_jhlbdm : tblSimpleInfo
    {
        public Td_jhlbdm()
        {

        }
    }
    #endregion
    #region 25 计划性质代码原始表（Td_jhxzdm_Origin）
    /// <summary>
    /// 25 计划性质代码原始表
    /// </summary>
    [Table("Td_jhxzdm_Origin")]
    public class Td_jhxzdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_jhxzdm jhxz { get; set; }
        public Td_jhxzdm_Origin()
        {

        }
    }
    #endregion
    #region 26 计划性质代码（Td_jhxzdm）
    /// <summary>
    /// 26 计划性质代码（Td_jhxzdm）
    /// </summary>
    [Table("Td_jhxzdm")]
    public class Td_jhxzdm : tblSimpleInfo
    {
        public Td_jhxzdm()
        {

        }
    }
    #endregion
    #region 27 科类代码原始表(Td_kldm_Origin)
    /// <summary>
    /// 27 科类代码原始表
    /// </summary>
    [Table("Td_kldm_Origin")]
    public class Td_kldm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_kldm kl { get; set; }
        [Display(Name = "兼报科类")]
        public string Jbkl { set; get; }
        [Display(Name = "考生成绩比较序号（江苏使用）")]
        public int Kscjbjxh { set; get; }
        public Td_kldm_Origin()
        {

        }

    }
    #endregion
    #region 28 科类代码（Td_kldm）
    /// <summary>
    /// 28 科类代码（Td_kldm）
    /// </summary>
    [Table("Td_kldm")]
    public class Td_kldm : tblSimpleInfo
    {
        [Display(Name = "兼报科类")]
        public string Jbkl { set; get; }
        [Display(Name = "考生成绩比较序号（江苏使用）")]
        public int Kscjbjxh { set; get; }
        public Td_kldm()
        {
            Kscjbjxh = 0;
        }
    }
    #endregion
    #region 29 科目代码原始表（Td_kmdm_Origin）
    /// <summary>
    /// 29 科目代码原始表（Td_kmdm_Origin）
    /// </summary>
    [Table("Td_kmdm_Origin")]
    public class Td_kmdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_kmdm km { get; set; }
        public Td_kmdm_Origin()
        {

        }
    }
    #endregion
    #region 30 科目代码(Td_kmdm)
    /// <summary>
    /// 30 科目代码(Td_kmdm)
    /// </summary>
    [Table("Td_kmdm")]
    public class Td_kmdm : tblSimpleInfo
    {
        public Td_kmdm()
        {
            
        }
    }
    #endregion
    #region 31 考生类别代码原始表（Td_kslbdm_Origin）
    /// <summary>
    /// 31 考生类别代码原始表
    /// </summary>
    [Table("Td_kslbdm_Origin")]
    public class Td_kslbdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_kslbdm kslb { get; set; }
        public Td_kslbdm_Origin()
        {

        }
    }

    #endregion
    #region 32 考生类别代码（Td_kslbdm）
    /// <summary>
    ///  32 考生类别代码
    /// </summary>
    [Table("Td_kslbdm")]
    public class Td_kslbdm : tblSimpleInfo
    {
        public Td_kslbdm()
        {

        }
    }
    #endregion
    #region 33 考试类型代码原始表（Td_kslxdm_Origin）
    /// <summary>
    /// 33 考试类型代码原始表
    /// </summary>
    [Table("Td_kslxdm_Origin")]
    public class Td_kslxdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_kslxdm kslx { get; set; }
        public Td_kslxdm_Origin()
        {

        }
    }
    #endregion
    #region 34 考试类型代码(Td_kslxdm）
    /// <summary>
    ///  34 考试类型代码
    /// </summary>
    [Table("Td_kslxdm")]
    public class Td_kslxdm : tblSimpleInfo
    {
        public Td_kslxdm()
        {

        }
    }
    #endregion
    #region 35 考生特征代码原始表（Td_kstzdm_Origin）
    /// <summary>
    /// 35 考生特征代码原始表（Td_kstzdm_Origin）
    /// </summary>
    [Table("Td_kstzdm_Origin")]
    public class Td_kstzdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_kstzdm kstz { get; set; }
        [Display(Name = "照顾分数")]
        public double Zgfs { get; set; }
        [Display(Name = "备注")]
        public string Bz { get; set; }

        public Td_kstzdm_Origin()
        {

        }
    }
    #endregion
    #region 36 考生特征代码(Td_kstzdm)
    /// <summary>
    ///  36 考生特征代码(Td_kstzdm)
    /// </summary>
    [Table("Td_kstzdm")]
    public class Td_kstzdm : tblSimpleInfo
    {
        [Display(Name = "照顾分数")]
        public double Zgfs { get; set; }
        public Td_kstzdm()
        {

        }
    }
    #endregion
    #region 37 考生资格代码原始表（Td_kszgdm_Origin）
    /// <summary>
    /// 37 考生资格代码原始表
    /// </summary>
    [Table("Td_kszgdm_Origin")]
    public class Td_kszgdm_Origin : tblOriginTable
    {
    //    [ForeignKey("RelationID")]
    //    public virtual Td_kszgdm kszg { get; set; }
        public Td_kszgdm_Origin()
        {

        }
    }
    #endregion
    #region 38 考生资格代码(Td_kszgdm)
    /// <summary>
    /// 38 考生资格代码(Td_kszgdm)
    /// </summary>
    [Table("Td_kszgdm")]
    public class Td_kszgdm : tblSimpleInfo
    {
        public Td_kszgdm()
        {

        }
    }
    #endregion
    #region 39 录取方式代码原始表（Td_lqfsdm_Origin）
    /// <summary>
    /// 39 录取方式代码原始表
    /// </summary>
    [Table("Td_lqfsdm_Origin")]
    public class Td_lqfsdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_lqfsdm lqfs { get; set; }
        public Td_lqfsdm_Origin()
        {

        }
    }
    #endregion
    #region 40 录取方式代码(12)/录取类型代码（17）(Td_lqfsdm)
    /// <summary>
    /// 40 录取方式代码(12)/录取类型代码（17）(Td_lqfsdm)
    /// </summary>
    [Table("Td_lqfsdm")]
    public class Td_lqfsdm : tblSimpleInfo
    {
        public Td_lqfsdm()
        {

        }
    }
    #endregion
    #region 41 民族代码原始表（Td_mzdm_Origin)
    /// <summary>
    /// 41 民族代码原始表
    /// </summary>
    [Table("Td_mzdm_Origin")]
    public class Td_mzdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_mzdm mzdm { get; set; }
        public Td_mzdm_Origin()
        {

        }
    }

    #endregion
    #region 42 民族代码表（Td_mzdm）
    /// <summary>
    /// 42 民族代码表
    /// </summary>
    [Table("Td_mzdm")]
    public class Td_mzdm : tblSimpleInfo
    {
        public Td_mzdm()
        {

        }
    }

    #endregion
    #region 43 批次代码原始表（Td_pcdm_Origin）
    /// <summary>
    /// 43 批次代码原始表（Td_pcdm_Origin）
    /// </summary>
    [Table("Td_pcdm_Origin")]
    public class Td_pcdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_pcdm pc { get; set; }
        [Display(Name = "国标批次代码")]
        public string Gbpcdm { get; set; }
        [Display(Name = "国标层次代码")]
        public string Gbccdm { get; set; }

        public Td_pcdm_Origin()
        {

        }
    }
    #endregion
    #region 44 批次代码（Td_pcdm）
    /// <summary>
    /// 44 批次代码（Td_pcdm）
    /// </summary>
    [Table("Td_pcdm")]
    public class Td_pcdm : tblSimpleInfo
    {
        [Display(Name = "国标批次代码")]
        public string Gbpcdm { get; set; }
        [Display(Name = "国标层次代码")]
        public string Gbccdm { get; set; }
        public Td_pcdm()
        {

        }
    }
    #endregion
    #region 45 投档类型代码原始表（Td_tdlxdm_Origin）
    /// <summary>
    /// 45 投档类型代码原始表（Td_tdlxdm_Origin）
    /// </summary>
    [Table("Td_tdlxdm_Origin")]
    public class Td_tdlxdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_tdlxdm tdlx { get; set; }

        public Td_tdlxdm_Origin()
        {

        }
    }
    #endregion
    #region 46 投档类型代码(Td_tdlxdm)
    /// <summary>
    /// 46 投档类型代码(Td_tdlxdm)
    /// </summary>
    [Table("Td_tdlxdm")]
    public class Td_tdlxdm : tblSimpleInfo
    {
        public Td_tdlxdm()
        {

        }
    }
    #endregion
    #region 47 退档原因代码原始表（Td_tdyydm_Origin）
    /// <summary>
    /// 47 退档原因代码原始表
    /// </summary>
    [Table("Td_tdyydm_Origin")]
    public class Td_tdyydm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_tdyydm tdyy { get; set; }
        public Td_tdyydm_Origin()
        {

        }
    }
    #endregion
    #region 48 退档原因代码（Td_tdyydm）
    /// <summary>
    /// 48 退档原因代码（Td_tdyydm）
    /// </summary>
    [Table("Td_tdyydm")]
    public class Td_tdyydm : tblSimpleInfo
    {
        public Td_tdyydm()
        {

        }
    }
    #endregion
    #region 49 体检结论代码原始表（Td_tjjldm_Origin）
    /// <summary>
    /// 49 体检结论代码原始表
    /// </summary>
    [Table("Td_tjjldm_Origin")]
    public class Td_tjjldm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_tjjldm tjjl { get; set; }
        public Td_tjjldm_Origin()
        {

        }
    }
    #endregion
    #region 50 体检结论代码（Td_tjjldm）
    /// <summary>
    /// 50 体检结论代码（Td_tjjldm）
    /// </summary>
    [Table("Td_tjjldm")]
    public class Td_tjjldm : tblSimpleInfo
    {
        public Td_tjjldm()
        {

        }
    }
    #endregion
    #region 51 外语语种代码原始表（Td_wyyzdm_Origin）
    /// <summary>
    /// 51 外语语种代码原始表
    /// </summary>
    [Table("Td_wyyzdm_Origin")]
    public class Td_wyyzdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_wyyzdm wyyz { get; set; }
        public Td_wyyzdm_Origin()
        {

        }
    }
    #endregion
    #region 52 外语语种代码（Td_wyyzdm）
    /// <summary>
    ///  52	外语语种代码
    /// </summary>
    [Table("Td_wyyzdm")]
    public class Td_wyyzdm : tblSimpleInfo
    {
        public Td_wyyzdm()
        {

        }
    }
    #endregion
    #region 53 系统单位代码原始表(Td_xtdwdm_Origin)
    /// <summary>
    /// 53 系统单位代码原始表
    /// </summary>
    [Table("Td_xtdwdm_Origin")]
    public class Td_xtdwdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_xtdwdm xtdw { get; set; }
        public Td_xtdwdm_Origin()
        {

        }
    }

    #endregion
    #region 54 系统单位代码（Td_xtdwdm）
    /// <summary>
    /// 54 系统单位代码
    /// </summary>
    [Table("Td_xtdwdm")]
    public class Td_xtdwdm : tblSimpleInfo
    {
        public Td_xtdwdm()
        {

        }
    }
    #endregion
    //#region 55 政策代码原始表（Td_zcdm_Origin）
    ///// <summary>
    ///// 55 政策代码原始表（Td_zcdm_Origin）
    ///// </summary>
    //[Table("Td_zcdm_Origin")]
    //public class Td_zcdm_Origin : tblOriginTable
    //{
    //    [ForeignKey("RelationID")]
    //    public virtual Td_zcdm zc { get; set; }
    //    public short Zcf { set; get; }
    //    public short Xdx { set; get; }
    //    public Td_zcdm_Origin()
    //    {

    //    }
    //}
    //#endregion
    //#region 56 政策代码（Td_zcdm）
    ///// <summary>
    ///// 56 政策代码（Td_zcdm）
    ///// </summary>
    //[Table("Td_zcdm")]
    //public class Td_zcdm : tblSimpleInfo
    //{
    //    public short Zcf { set; get; }
    //    public short Xdx { set; get; }

    //    public Td_zcdm()
    //    {

    //    }
    //}
    //#endregion
    #region 57 学制代码原始表（Td_xzdm_Origin）
    /// <summary>
    /// 57 学制代码原始表（Td_xzdm_Origin）
    /// </summary>
    [Table("Td_xzdm_Origin")]
    public class Td_xzdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_xzdm xz { get; set; }
        public Td_xzdm_Origin()
        {

        }
    }
    #endregion
    #region 58 学制代码(Td_xzdm)
    /// <summary>
    /// 58 学制代码(Td_xzdm)
    /// </summary>
    [Table("Td_xzdm")]
    public class Td_xzdm : tblSimpleInfo
    {
        public Td_xzdm()
        {

        }
    }
    #endregion
    #region 59 证件类型代码原始表（Td_zzmmdm_Origin）
    /// <summary>
    /// 59 证件类型代码原始表
    /// </summary>
    [Table("Td_zjlxdm_Origin")]
    public class Td_zjlxdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_zjlxdm zjlx { get; set; }
        public Td_zjlxdm_Origin()
        {

        }
    }
    #endregion
    #region 60 证件类型代码(Td_zjlxdm)
    /// <summary>
    /// 60 证件类型代码(Td_zjlxdm)
    /// </summary>
    [Table("Td_zjlxdm")]
    public class Td_zjlxdm : tblSimpleInfo
    {
        public Td_zjlxdm()
        {

        }
    }
    #endregion
    #region 61 招考类型码原始表（Td_zklxdm_Origin）
    /// <summary>
    /// 61 招考类型码原始表（Td_zklxdm_Origin）
    /// </summary>
    [Table("Td_zklxdm_Origin")]
    public class Td_zklxdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_zklxdm zklx { get; set; }
        public Td_zklxdm_Origin()
        {

        }
    }
    #endregion
    #region 62 招考类型代码(Td_zklxdm)
    /// <summary>
    /// 62 招考类型代码(Td_zklxdm)
    /// </summary>
    [Table("Td_zklxdm")]
    public class Td_zklxdm : tblSimpleInfo
    {
        public Td_zklxdm()
        {

        }
    }
    #endregion
    #region 63 专业类别代码原始表（Td_zylbdm_Origin）
    /// <summary>
    /// 63 专业类别代码原始表（Td_zylbdm_Origin）
    /// </summary>
    [Table("Td_zylbdm_Origin")]
    public class Td_zylbdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_zylbdm zylb { get; set; }
        public Td_zylbdm_Origin()
        {

        }
    }
    #endregion
    #region 64 专业类别代码(Td_zylbdm)
    /// <summary>
    /// 64 专业类别代码(Td_zylbdm)
    /// </summary>
    [Table("Td_zylbdm")]
    public class Td_zylbdm : tblTreeInfo
    {
        [Display(Name = "合并同类项")]
        public string JoinNames { get; set; }
        [Display(Name = "合并后的ID")]
        public long JoinID { set; get; }
        public bool ScanFlag { set; get; }
        public Td_zylbdm()
        {

        }
    }
    #endregion
    #region 65 专业调剂类型代码原始表（Td_zytjlxdm_Origin）
    /// <summary>
    /// 65 政治面貌代码原始表
    /// </summary>
    [Table("Td_zytjlxdm_Origin")]
    public class Td_zytjlxdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_zytjlxdm zytjlx { get; set; }
        public Td_zytjlxdm_Origin()
        {

        }
    }
    #endregion
    #region 66 专业调剂类型代码（Td_zytjlxdm）
    /// <summary>
    /// 66 专业调剂类型代码（Td_zytjlxdm）
    /// </summary>
    [Table("Td_zytjlxdm")]
    public class Td_zytjlxdm : tblSimpleInfo
    {
        public Td_zytjlxdm()
        {

        }
    }
    #endregion
    #region 67 志愿特征代码原始表（Td_zytzdm_Origin）
    /// <summary>
    /// 67 志愿特征代码原始表
    /// </summary>
    [Table("Td_zytzdm_Origin")]
    public class Td_zytzdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_zytzdm zzmm { get; set; }
        public Td_zytzdm_Origin()
        {

        }
    }
    #endregion
    #region 68 志愿特征代码（Td_zytzdm）
    /// <summary>
    /// 68 志愿特征代码（Td_zytzdm）
    /// </summary>
    [Table("Td_zytzdm")]
    public class Td_zytzdm : tblSimpleInfo
    {
        public Td_zytzdm()
        {

        }
    }
    #endregion
    #region 69 政治面貌代码原始表（Td_zzmmdm_Origin）
    /// <summary>
    ///69 政治面貌代码原始表
    /// </summary>
    [Table("Td_zzmmdm_Origin")]
    public class Td_zzmmdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_zzmmdm zzmm { get; set; }
        public Td_zzmmdm_Origin()
        {

        }
    }
    #endregion
    #region 70 政治面貌代码（Td_zzmmdm）
    /// <summary>
    /// 70 政治面貌代码
    /// </summary>
    [Table("Td_zzmmdm")]
    public class Td_zzmmdm : tblSimpleInfo
    {
        public Td_zzmmdm()
        {

        }
    }
    #endregion
    #region 71 毕业中学代码原始表（Td_byxxdm_Origin）
    /// <summary>
    ///71 毕业中学代码原始表（Td_byxxdm_Origin）
    /// </summary>
    [Table("Td_byxxdm_Origin")]
    public class Td_byxxdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_byxxdm byxx { get; set; }
        public Td_byxxdm_Origin()
        {

        }
    }
    #endregion
    #region 72 毕业中学代码（Td_byxxdm）
    /// <summary>
    /// 72 毕业中学代码（Td_byxxdm）
    /// </summary>
    [Table("Td_byxxdm")]
    public class Td_byxxdm : tblSimpleInfo
    {
        public Td_byxxdm()
        {

        }
    }
    #endregion
    #region 73 高校代码表（Td_gxdm）
    /// <summary>
    /// 73 高校代码表（Td_gxdm）
    /// </summary>
    [Table("Td_gxdm")]
    public class Td_gxdm : tblSimpleInfo
    {
        public virtual List<T_tddw> tddws { set; get; }

        public Td_gxdm()
        {

        }
    }
    #endregion
    #region 74 性别代码原始表（Td_xbdm_Origin）
    /// <summary>
    /// 74 性别代码原始表（Td_xzdm_Origin）
    /// </summary>
    [Table("Td_xbdm_Origin")]
    public class Td_xbdm_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual Td_xzdm xz { get; set; }
        public Td_xbdm_Origin()
        {

        }
    }
    #endregion
    #region 75 性别代码(Td_xbdm)
    /// <summary>
    /// 75 性别代码(Td_xbdm)
    /// </summary>
    [Table("Td_xbdm")]
    public class Td_xbdm : tblSimpleInfo
    {
        public Td_xbdm()
        {

        }
    }
    #endregion
    #region 76 报表类型信息表(tblReportType)
    /// <summary>
    /// 76 报表类型信息表(tblReportType)
    /// </summary>
    [Table("tblReportType")]
    public class tblReportType : tblSimpleInfo
    {
        public tblReportType()
        {
            
        }
    }
    #endregion
    #region 77 报表信息表（tblReportInfo）
    /// <summary>
    /// 77	报表信息表（tblReportInfo）
    /// </summary>
    [Table("tblReportInfo")]
    public class tblReportInfo : tblNameRemarkInfo
    {
        public long UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual tblUserInfo user { get; set; }
        public DateTime CreateDate { get; set; }

        public long ReportTypeID { set; get; }
        [ForeignKey("ReportTypeID")]
        public virtual tblReportType reporttype { get; set; }
        public long GxID { get; set; }

        [ForeignKey("GxID")]
        public virtual Td_gxdm gx { get; set; }
        public int StartYear { set; get; }
        public int EndYear { set; get; }
        [NotMapped]
        public bool bWishNum { set; get; }
        [NotMapped]
        public bool bRealNum { set; get; }

        public string HandleFlag { get; set; }

        [NotMapped]
        public bool HandleWay { get; set; }

        public tblReportInfo()
        {
            CreateDate = DateTime.Now;
            StartYear = DateTime.Now.Year;
            EndYear = DateTime.Now.Year;
            bWishNum = true;
            bRealNum = true;
            HandleFlag = "正在创建";
            HandleWay = true;
        }
    }
    #endregion


    #region 81 投档单位原始表（T_tddw_Origin)
    /// <summary>
    /// 81 投档单位原始表
    /// </summary>
    [Table("T_tddw_Origin")]
    public class T_tddw_Origin : tblOriginTable
    {
        //[ForeignKey("RelationID")]
        //public virtual T_tddw tddw { get; set; }
        [Display(Name = "批次代码")]
        public long Pcdm { get; set; }
        [ForeignKey("Pcdm")]
        public virtual Td_pcdm_Origin pc { get; set; }
        [Display(Name = "科类代码")]
        public long Kldm { get; set; }
        [ForeignKey("Kldm")]
        public virtual Td_kldm_Origin kl { get; set; }
        [Display(Name = "计划性质代码")]
        public long Jhxzdm { get; set; }
        [ForeignKey("Jhxzdm")]
        public virtual Td_jhxzdm_Origin jhxz { get; set; }

        [Display(Name = "计划类别代码")]
        public long Jhlbdm { get; set; }
        [ForeignKey("Jhlbdm")]
        public virtual Td_jhlbdm_Origin jhlb { get; set; }

        [Display(Name = "参数模板号")]
        public string Csmbh { get; set; }
        [Display(Name = "操作标志（内部使用）")]
        public string Tag { get; set; }

        public T_tddw_Origin()
        {

        }
    }
    #endregion
    # region 82 投档单位(T_tddw)
    /// <summary>
    /// 82 投档单位
    /// </summary>
    [Table("T_tddw")]
    public class T_tddw : tblSimpleInfo
    {        
        public long GxID { get; set; }
        [ForeignKey("GxID")]
        public virtual Td_gxdm gx { get; set; }

        [Display(Name = "原始表批次代码")]
        public long PcdmOrigin { get; set; }
        [ForeignKey("PcdmOrigin")]
        public virtual Td_pcdm_Origin pcOrigin { get; set; }

        [Display(Name = "批次代码")]
        public long Pcdm { get; set; }
        [ForeignKey("Pcdm")]
        public virtual Td_pcdm pc { get; set; }
        [Display(Name = "原始表科类代码")]
        public long KldmOrigin { get; set; }
        [ForeignKey("KldmOrigin")]
        public virtual Td_kldm_Origin klOrigin { get; set; }

        [Display(Name = "科类代码")]
        public long Kldm { get; set; }
        [ForeignKey("Kldm")]
        public virtual Td_kldm kl { get; set; }
        [Display(Name = "原始表计划性质代码")]
        public long JhxzdmOrigin { get; set; }
        [ForeignKey("JhxzdmOrigin")]
        public virtual Td_jhxzdm_Origin jhxzOrigin { get; set; }

        [Display(Name = "计划性质代码")]
        public long Jhxzdm { get; set; }
        [ForeignKey("Jhxzdm")]
        public virtual Td_jhxzdm jhxz { get; set; }
        [Display(Name = "原始表计划类别代码")]
        public long JhlbdmOrigin { get; set; }
        [ForeignKey("JhlbdmOrigin")]
        public virtual Td_jhlbdm_Origin jhlbOrigin { get; set; }

        [Display(Name = "计划类别代码")]
        public long Jhlbdm { get; set; }
        [ForeignKey("Jhlbdm")]
        public virtual Td_jhlbdm jhlb { get; set; }
        [Display(Name = "参数模板号")]
        public string Csmbh { get; set; }
        [Display(Name = "操作标志（内部使用）")]
        public string Tag { get; set; }

        public T_tddw()
        {
            Pcdm = 0; PcdmOrigin = 0;
            Kldm = 0; KldmOrigin = 0;
            Jhxzdm = 0;JhxzdmOrigin = 0;
            Jhlbdm = 0;JhlbdmOrigin = 0;
        }
    }
    #endregion
    # region 83 专业代号原始表（Td_zydh_Origin）
    /// <summary>
    ///83 专业代号原始表（Td_zydh_Origin）
    /// </summary>
    [Table("Td_zydh_Origin")]
    public class Td_zydh_Origin : tblOriginTable
    {
        [Display(Name = "科类代码")]
        public long Kldm { get; set; }

        [Display(Name = "招考方向")]
        public string Zkfx { get; set; }
        [Display(Name = "体检受限标志")]
        public string Tjsxbz { get; set; }
        [Display(Name = "层次代码")]
        public long Ccdm { get; set; }
        [Display(Name = "是否口试")]
        public bool Sfks { get; set; }
        [Display(Name = "学制代码")]
        public long Xzdm { get; set; }
        [Display(Name = "收费标准")]
        public int Sfbz { get; set; }
        [Display(Name = "办学地点")]
        public string Bxdd { get; set; }
        [Display(Name = "外语语种")]
        public string Wyyz { get; set; }
        [Display(Name = "专业类别代码")]
        public long Zylbdm { get; set; }
        [Display(Name = "备注")]
        public string Bz { get; set; }

        //[ForeignKey("RelationID")]
        //public virtual Td_zydh zydh { get; set; }
        public long ParentDepartmentID { get; set; }
        //[ForeignKey("ParentDepartmentID")]
        //public virtual tblDepartmentInfo ParentDepartment { get; set; }
        public Td_zydh_Origin()
        {

        }
    }
    #endregion
    # region 84 专业代号表（Td_zydh）
    /// <summary>
    /// 84 专业代号表（Td_zydh）
     /// </summary>
    [Table("Td_zydh")]
    public class Td_zydh : tblSimpleInfo
    {
        [Display(Name = "科类代码")]
        public long Kldm { get; set; }
        [ForeignKey("Kldm")]
        public virtual Td_kldm kl { get; set; }
        [Display(Name = "原始表科类代码")]
        public long KldmOrigin { get; set; }
        [ForeignKey("KldmOrigin")]
        public virtual Td_kldm_Origin klOrigin { get; set; }

        [Display(Name = "招考方向")]
        public string Zkfx { get; set; }
        [Display(Name = "体检受限标志")]
        public string Tjsxbz { get; set; }
        [Display(Name = "层次代码")]
        public long Ccdm { get; set; }
        [ForeignKey("Ccdm")]
        public virtual Td_ccdm cc { get; set; }
        [Display(Name = "原始表层次代码")]
        public long CcdmOrigin { get; set; }
        [ForeignKey("CcdmOrigin")]
        public virtual Td_ccdm_Origin ccOrigin { get; set; }
        [Display(Name = "是否口试")]
        public bool Sfks { get; set; }
        [Display(Name = "学制代码")]
        public long Xzdm { get; set; }
        [ForeignKey("Xzdm")]
        public virtual Td_xzdm xz { get; set; }
        [Display(Name = "原始表学制代码")]
        public long XzdmOrigin { get; set; }
        [ForeignKey("XzdmOrigin")]
        public virtual Td_xzdm_Origin xzOrigin { get; set; }
        [Display(Name = "收费标准")]
        public int Sfbz { get; set; }
        [Display(Name = "办学地点")]
        public string Bxdd { get; set; }
        [Display(Name = "外语语种")]
        public string Wyyz { get; set; }
        [Display(Name = "专业类别代码")]
        public long Zylbdm { get; set; }
        [ForeignKey("Zylbdm")]
        public virtual Td_zylbdm zylb { get; set; }
        [Display(Name = "原始表专业类别代码")]
        public long ZylbdmOrigin { get; set; }
        [ForeignKey("ZylbdmOrigin")]
        public virtual Td_zylbdm_Origin zylbOrigin { get; set; }
        public long ParentDepartmentID { get; set; }
        [ForeignKey("ParentDepartmentID")]
        public virtual tblDepartmentInfo ParentDepartment { get; set; }

        public long GxID { get; set; }
        [ForeignKey("GxID")]
        public virtual Td_gxdm Gx { get; set; }

        [NotMapped]
        public string GxName { set; get; }

        public Td_zydh()
        {
            Ccdm = 0; CcdmOrigin = 0;
            Xzdm = 0; XzdmOrigin = 0;
            Zylbdm = 0; ZylbdmOrigin = 0;
            Kldm = 0; KldmOrigin = 0;
        }
    }
    #endregion
    #region 85 计划库(T_jhk) 
    /// <summary>
    /// 85 计划库(T_jhk) 
    /// </summary>
    [Table("T_jhk")]
    public class T_jhk : tblDelInfo
    {
        //[Display(Name = "批次代码")]
        //public long Pcdm { get; set; }
        //[ForeignKey("Pcdm")]
        //public virtual Td_pcdm pc { get; set; }
        [Display(Name = "批次代码")]
        public long PcdmOrigin { get; set; }
        [ForeignKey("PcdmOrigin")]
        public virtual Td_pcdm_Origin pcOrigin { get; set; }
        [Display(Name = "科类代码")]
        public long KldmOrigin { get; set; }
        [ForeignKey("KldmOrigin")]
        public virtual Td_kldm_Origin klOrigin { get; set; }
        //[Display(Name = "计划性质代码")]
        //public long Jhxzdm { get; set; }
        //[ForeignKey("Jhxzdm")]
        //public virtual Td_jhxzdm jhxz { get; set; }
        [Display(Name = "计划性质代码")]
        public long JhxzdmOrigin { get; set; }
        [ForeignKey("JhxzdmOrigin")]
        public virtual Td_jhxzdm_Origin jhxzOrigin { get; set; }
        //[Display(Name = "计划类别代码")]
        //public long Jhlbdm { get; set; }
        //[ForeignKey("Jhlbdm")]
        //public virtual Td_jhlbdm jhlb { get; set; }
        [Display(Name = "计划类别代码")]
        public long JhlbdmOrigin { get; set; }
        [ForeignKey("JhlbdmOrigin")]
        public virtual Td_jhlbdm_Origin jhlbOrigin { get; set; }
        [Display(Name = "投档单位代码")]
        public long TddwdmOrigin { get; set; }
        [ForeignKey("TddwdmOrigin")]
        public virtual T_tddw_Origin tddwOrigin { get; set; }
        [Display(Name = "投档单位代码")]
        public long Tddwdm { get; set; }
        [ForeignKey("Tddwdm")]
        public virtual T_tddw tddw { get; set; }
        [Display(Name = "招考类型代码")]
        public long Zklxdm { get; set; }
        [ForeignKey("Zklxdm")]
        public virtual Td_zklxdm zklx { get; set; }
        [Display(Name = "招考类型代码")]
        public long ZklxdmOrigin { get; set; }
        [ForeignKey("ZklxdmOrigin")]
        public virtual Td_zklxdm_Origin zklxOrigin { get; set; }
        [Display(Name = "专业代号")]
        public long Zydh { get; set; }
        [ForeignKey("Zydh")]
        public virtual Td_zydh zydh { get; set; }
        [Display(Name = "专业代号")]
        public long ZydhOrigin { get; set; }
        [ForeignKey("ZydhOrigin")]
        public virtual Td_zydh_Origin zydhOrigin { get; set; }
        [Display(Name = "计划人数")]
        public int Jhrs { get; set; }
        [Display(Name = "计划执行数")]
        public int Jhzxs { get; set; }
        [Display(Name = "是否师范")]
        public bool Sfsf { get; set; }
        [Display(Name = "预投差额数")]
        public int Ytces { get; set; }
        [Display(Name = "专业调剂优先序号")]
        public int Yxxh { get; set; }
        [Display(Name = "备注")]
        public string Bz { get; set; }
        public long GxID { get; set; }
        public long ParentDepartmentID { get; set; }
        [ForeignKey("ParentDepartmentID")]
        public virtual tblDepartmentInfo ParentDepartment { get; set; }
        public int YearCode { set; get; }
        public long ProvinceID { set; get; }
        [ForeignKey("ProvinceID")]
        public virtual Td_dqdm dq { get; set; }
        
        //下面这些用在详细窗口中
        [NotMapped]
        public string GxName { get; set; } //也可用在查询界面
        [NotMapped]
        public string JhxzName { get; set; }
        [NotMapped]
        public string JhlbName { get; set; }
        [NotMapped]
        public string PcName { get; set; }
        [NotMapped]
        public string KlName { get; set; }
        [NotMapped]
        public string ProvinceName { get; set; }
        //下面这些用在查询界面中
        [NotMapped]
        public string SearchZydm { get; set; }
        [NotMapped]
        public string SearchZydh { get; set; }
        [NotMapped]
        public string SearchZymc { get; set; }
        [NotMapped]
        public long SearchPcdm { get; set; }
        [NotMapped]
        public long SearchKldm { get; set; }
        [NotMapped]
        public long SearchJhxzdm { get; set; }
        [NotMapped]
        public long SearchJhlbdm { get; set; }
        [NotMapped]
        public long SearchXzdm { get; set; }
        [NotMapped]
        public int? SearchStartYear { get; set; }
        [NotMapped]
        public int? SearchEndYear { get; set; }
        [NotMapped]
        public int? SearchStartJhrs { get; set; }
        [NotMapped]
        public int? SearchEndJhrs { get; set; }
        [NotMapped]
        public int? SearchStartJhzxs { get; set; }
        [NotMapped]
        public int? SearchEndJhzxs { get; set; }


        public T_jhk()
        {
            JhxzdmOrigin = 0; JhlbdmOrigin = 0;
            PcdmOrigin = 0; KldmOrigin = 0;
            TddwdmOrigin = 0; Tddwdm = 0;
            ZklxdmOrigin = 0;
            Zydh = 0; ZydhOrigin = 0;

            SearchStartYear = DateTime.Now.Year - 10;
            SearchEndYear = DateTime.Now.Year;
            SearchStartJhrs = 0;
            SearchEndJhrs = 1000;
            SearchStartJhzxs = 0;
            SearchEndJhzxs = 1000;
            SearchPcdm = 0;
            SearchKldm = 0;
            SearchJhxzdm = 0;
            SearchJhlbdm = 0;
            SearchXzdm = 0;
            ProvinceID = 0;
            SearchZydm = "";
            SearchZydh = "";
            SearchZymc = "";

        }
    }
    #endregion
    #region 86 原始报名库（T_bmk_Origin）
    /// <summary>
    ///86 原始报名库（T_bmk_Origin）
    /// </summary>
    [Table("T_bmk_Origin")]
    public class T_bmk_Origin : tblDelInfo
    {
        public int YearCode { set; get; }
        public long GxID { set; get; }
        [ForeignKey("GxID")]
        public virtual Td_gxdm gx { get; set; }
        public long ProvinceID { set; get; }
        [ForeignKey("ProvinceID")]
        public virtual Td_dqdm province { get; set; }

        [Display(Name = "考生号")]
        public string Ksh { get; set; }
        [Display(Name = "准考证号")]
        public string Zkzh { get; set; }
        [Display(Name = "姓名")]
        public string Xm { get; set; }
        [Display(Name = "性别代码")]
        public long Xbdm { get; set; }
        [ForeignKey("Xbdm")]
        public virtual Td_xbdm xb { get; set; }
        [Display(Name = "原始性别代码")]
        public long XbdmOrigin { get; set; }
        [ForeignKey("XbdmOrigin")]
        public virtual Td_xbdm_Origin xbOrigin { get; set; }
        [Display(Name = "出生日期")]
        public DateTime Csrq { get; set; }

        [Display(Name = "政治面貌代码")]
        public long Zzmmdm { get; set; }
        [ForeignKey("Zzmmdm")]
        public virtual Td_zzmmdm zzmm { get; set; }
        [Display(Name = "原始表政治面貌代码")]
        public long ZzmmdmOrigin { get; set; }
        [ForeignKey("ZzmmdmOrigin")]
        public virtual Td_zzmmdm_Origin zzmmOrigin { get; set; }
        [Display(Name = "民族代码")]
        public long Mzdm { get; set; }
        [ForeignKey("Mzdm")]
        public virtual Td_mzdm mz { get; set; }
        [Display(Name = "原始表民族代码")]
        public long MzdmOrigin { get; set; }
        [ForeignKey("MzdmOrigin")]
        public virtual Td_mzdm_Origin mzOrigin { get; set; }
        [Display(Name = "会考考号")]
        public string Hkkh { get; set; }
        [Display(Name = "会考等级")]
        public string Hkdj { get; set; }
        [Display(Name = "系统单位代码")]
        public long Xtdwdm { get; set; }
        [ForeignKey("Xtdwdm")]
        public virtual Td_xtdwdm xtdw { get; set; }
        [Display(Name = "原始表系统单位代码")]
        public long XtdwdmOrigin { get; set; }
        [ForeignKey("XtdwdmOrigin")]
        public virtual Td_xtdwdm_Origin xtdwOrigin { set; get; }
        //[Display(Name = "身份证号")]
        //public string Sfzh { get; set; }
        //[Display(Name = "家庭地址")]
        //public string Jtdz { get; set; }
        [Display(Name = "政审意见")]
        public string Zsyj { get; set; }
        //[Display(Name = "科类代码")]
        //public long Kldm { get; set; }
        //[ForeignKey("Kldm")]
        //public virtual Td_kldm kl { get; set; }
        [Display(Name = "原始表科类代码")]
        public long KldmOrigin { get; set; }
        [ForeignKey("KldmOrigin")]
        public virtual Td_kldm_Origin klOrigin { get; set; }
        [Display(Name = "考生类别代码")]
        public long Kslbdm { get; set; }
        [ForeignKey("Kslbdm")]
        public virtual Td_kslbdm kslb { get; set; }
        [Display(Name = "原始表考生类别代码")]
        public long KslbdmOrigin { get; set; }
        [ForeignKey("KslbdmOrigin")]
        public virtual Td_kldm_Origin kslbOrigin { get; set; }
        [Display(Name = "考试类型代码")]
        public long Kslxdm { get; set; }
        [ForeignKey("Kslxdm")]
        public virtual Td_kslxdm kslx { get; set; }
        [Display(Name = "原始表考试类型代码")]
        public long KslxdmOrigin { get; set; }
        [ForeignKey("KslxdmOrigin")]
        public virtual Td_kldm_Origin kslxOrigin { get; set; }
        [Display(Name = "毕业类别代码")]
        public long Bylbdm { get; set; }
        [ForeignKey("Bylbdm")]
        public virtual Td_bylbdm bylb { get; set; }
        [Display(Name = "原始表毕业类别代码")]
        public long BylbdmOrigin { get; set; }
        [ForeignKey("BylbdmOrigin")]
        public virtual Td_kldm_Origin bylbOrigin { get; set; }
        [Display(Name = "毕业学校代码")]
        public long Byxxdm { get; set; }
        [ForeignKey("Byxxdm")]
        public virtual Td_byxxdm byxx { get; set; }
        [Display(Name = "原始表毕业学校代码")]
        public long ByxxdmOrigin { get; set; }
        [ForeignKey("ByxxdmOrigin")]
        public virtual Td_byxxdm_Origin byxxOrigin { get; set; }
        [Display(Name = "外语语种代码")]
        public long Wyyzdm { get; set; }
        [ForeignKey("Wyyzdm")]
        public virtual Td_wyyzdm wyyz { get; set; }
        [Display(Name = "原始表外语语种代码")]
        public long WyyzdmOrigin { get; set; }
        [ForeignKey("WyyzdmOrigin")]
        public virtual Td_kldm_Origin wyyzOrigin { get; set; }
        [Display(Name = "考生特征")]
        public string Kstz { set; get; }
        [Display(Name = "原始表考生特征")]
        public string KstzOrigin { set; get; }

        [Display(Name = "地区代码")]
        public long Dqdm { get; set; }
        [ForeignKey("Dqdm")]
        public virtual Td_dqdm dq { get; set; }
        [Display(Name = "原始表地区代码")]
        public long DqdmOrigin { get; set; }
        [ForeignKey("DqdmOrigin")]
        public virtual Td_dqdm_Origin dqOrigin { get; set; }
        [Display(Name = "证件类型代码")]
        public long Zjlxdm { get; set; }
        [ForeignKey("Zjlxdm")]
        public virtual Td_zjlxdm zjlx { get; set; }
        [Display(Name = "原始表证件类型代码")]
        public long ZjlxdmOrigin { get; set; }
        [ForeignKey("ZjlxdmOrigin")]
        public virtual Td_zjlxdm_Origin zjlxOrigin { get; set; }
        [Display(Name = "证件号码")]
        public string Zjhm { get; set; }
        [Display(Name = "考生学籍号")]
        public string Xjh { get; set; }
        [Display(Name = "外语口试")]
        public string Wyks { get; set; }
        [Display(Name = "外语听力")]
        public string Wytl { get; set; }
        [Display(Name = "通讯地址")]
        public string Txdz { get; set; }
        [Display(Name = "邮政编码")]
        public string Yzbm { get; set; }
        [Display(Name = "联系手机")]
        public string Lxsj { get; set; }
        [Display(Name = "联系电话")]
        public string Lxdh { get; set; }
        [Display(Name = "收件人")]
        public string Sjr { get; set; }
        [Display(Name = "考生特长")]
        public string Kstc { get; set; }
        [Display(Name = "考生奖励或处罚")]
        public string Ksjlhcf { get; set; }
        [Display(Name = "思想品德考核意见")]
        public string Sxpdkhyj { get; set; }
        [Display(Name = "考生资格")]
        public string Kszg { get; set; }
        [Display(Name = "原始表考生资格")]
        public string KszgOrigin { get; set; }
        [Display(Name = "户籍类别代码")]
        public long Hjlbdm { get; set; }
        [ForeignKey("Hjlbdm")]
        public virtual Td_hjlbdm hjlb { get; set; }
        [Display(Name = "原始表户籍类别代码")]
        public long HjlbdmOrigin { get; set; }
        [ForeignKey("HjlbdmOrigin")]
        public virtual Td_hjlbdm_Origin hjlbOrigin { get; set; }
        [Display(Name = "残障类别代码")]
        public long Czlbdm { get; set; }
        [ForeignKey("Czlbdm")]
        public virtual Td_czlbdm czlb { get; set; }
        [Display(Name = "原始表残障类别代码")]
        public long CzlbdmOrigin { get; set; }
        [ForeignKey("CzlbdmOrigin")]
        public virtual Td_czlbdm_Origin czlbOrigin { get; set; }
        [Display(Name = "选考科目")]
        public string Xkkm { get; set; }
        [Display(Name = "选考科目原始码")]
        public string XkkmOrigin { get; set; }

        [Display(Name = "术科科目")]
        public string Skkm { get; set; }
        [Display(Name = "术科科目原始码")]
        public string SkkmOrigin { get; set; }
        [Display(Name = "考生临时操作标志")]
        public string Tag { get; set; }
        
        public T_bmk_Origin()
        {
            Zzmmdm = 0; ZzmmdmOrigin = 0;
            Mzdm = 0; MzdmOrigin = 0;
            Xtdwdm = 0; XtdwdmOrigin = 0;
            KldmOrigin = 0;
            Kslbdm = 0; KslbdmOrigin = 0;
            Kslxdm = 0; KslxdmOrigin = 0;
            Bylbdm = 0; BylbdmOrigin = 0;
            Byxxdm = 0; ByxxdmOrigin = 0;
            Wyyzdm = 0; WyyzdmOrigin = 0;
            Dqdm = 0; DqdmOrigin = 0;
            Zjlxdm = 0; ZjlxdmOrigin = 0;
            Hjlbdm = 0; HjlbdmOrigin = 0;
            Czlbdm = 0; CzlbdmOrigin = 0;
        }
    }

    #endregion
    #region 87 报名库（T_bmk）
    /// <summary>
    /// 87 报名库（T_bmk）
    /// </summary>
    [Table("T_bmk")]
    public class T_bmk : tblDelInfo
    {
        public string VerifyStatus { set; get; }
        public DateTime? VerifyDateTime { set; get; }
        public string FlowNumber { set; get; }
        public string VerifyResult { set; get; }
        public int YearCode { set; get; }
        public long GxID { set; get; }
        [ForeignKey("GxID")]
        public virtual Td_gxdm gx { get; set; }
        public long ProvinceID { set; get; }
        [ForeignKey("ProvinceID")]
        public virtual Td_dqdm province { get; set; }
        public long RelationID { set; get; }
        [ForeignKey("RelationID")]
        public virtual T_bmk_Origin bmkOrigin { get; set; }

        [Display(Name = "考生号")]
        public string Ksh { get; set; }
        [Display(Name = "准考证号")]
        public string Zkzh { get; set; }
        [Display(Name = "姓名")]
        public string Xm { get; set; }
        [Display(Name = "性别代码")]
        public long Xbdm { get; set; }
        [ForeignKey("Xbdm")]
        public virtual Td_xbdm xb { get; set; }
        [Display(Name = "原始性别代码")]
        public long XbdmOrigin { get; set; }
        [ForeignKey("XbdmOrigin")]
        public virtual Td_xbdm_Origin xbOrigin { get; set; }
        [Display(Name = "出生日期")]
        public DateTime Csrq { get; set; }

        [Display(Name = "政治面貌代码")]
        public long Zzmmdm { get; set; }
        [ForeignKey("Zzmmdm")]
        public virtual Td_zzmmdm zzmm { get; set; }
        [Display(Name = "原始表政治面貌代码")]
        public long ZzmmdmOrigin { get; set; }
        [ForeignKey("ZzmmdmOrigin")]
        public virtual Td_zzmmdm_Origin zzmmOrigin { get; set; }
        [Display(Name = "民族代码")]
        public long Mzdm { get; set; }
        [ForeignKey("Mzdm")]
        public virtual Td_mzdm mz { get; set; }
        [Display(Name = "原始表民族代码")]
        public long MzdmOrigin { get; set; }
        [ForeignKey("MzdmOrigin")]
        public virtual Td_mzdm_Origin mzOrigin { get; set; }
        [Display(Name = "会考考号")]
        public string Hkkh { get; set; }
        [Display(Name = "会考等级")]
        public string Hkdj { get; set; }
        [Display(Name = "系统单位代码")]
        public long Xtdwdm { get; set; }
        [ForeignKey("Xtdwdm")]
        public virtual Td_xtdwdm xtdw { get; set; }
        [Display(Name = "原始表系统单位代码")]
        public long XtdwdmOrigin { get; set; }
        [ForeignKey("XtdwdmOrigin")]
        public virtual Td_xtdwdm_Origin xtdwOrigin { set; get; }
        //[Display(Name = "身份证号")]
        //public string Sfzh { get; set; }
        //[Display(Name = "家庭地址")]
        //public string Jtdz { get; set; }
        [Display(Name = "政审意见")]
        public string Zsyj { get; set; }
        //[Display(Name = "科类代码")]
        //public long Kldm { get; set; }
        //[ForeignKey("Kldm")]
        //public virtual Td_kldm kl { get; set; }
        [Display(Name = "原始表科类代码")]
        public long KldmOrigin { get; set; }
        [ForeignKey("KldmOrigin")]
        public virtual Td_kldm_Origin klOrigin { get; set; }
        [Display(Name = "考生类别代码")]
        public long Kslbdm { get; set; }
        [ForeignKey("Kslbdm")]
        public virtual Td_kslbdm kslb { get; set; }
        [Display(Name = "原始表考生类别代码")]
        public long KslbdmOrigin { get; set; }
        [ForeignKey("KslbdmOrigin")]
        public virtual Td_kldm_Origin kslbOrigin { get; set; }
        [Display(Name = "考试类型代码")]
        public long Kslxdm { get; set; }
        [ForeignKey("Kslxdm")]
        public virtual Td_kslxdm kslx { get; set; }
        [Display(Name = "原始表考试类型代码")]
        public long KslxdmOrigin { get; set; }
        [ForeignKey("KslxdmOrigin")]
        public virtual Td_kldm_Origin kslxOrigin { get; set; }
        [Display(Name = "毕业类别代码")]
        public long Bylbdm { get; set; }
        [ForeignKey("Bylbdm")]
        public virtual Td_bylbdm bylb { get; set; }
        [Display(Name = "原始表毕业类别代码")]
        public long BylbdmOrigin { get; set; }
        [ForeignKey("BylbdmOrigin")]
        public virtual Td_kldm_Origin bylbOrigin { get; set; }
        [Display(Name = "毕业学校代码")]
        public long Byxxdm { get; set; }
        [ForeignKey("Byxxdm")]
        public virtual Td_byxxdm byxx { get; set; }
        [Display(Name = "原始表毕业学校代码")]
        public long ByxxdmOrigin { get; set; }
        [ForeignKey("ByxxdmOrigin")]
        public virtual Td_byxxdm_Origin byxxOrigin { get; set; }
        [Display(Name = "外语语种代码")]
        public long Wyyzdm { get; set; }
        [ForeignKey("Wyyzdm")]
        public virtual Td_wyyzdm wyyz { get; set; }
        [Display(Name = "原始表外语语种代码")]
        public long WyyzdmOrigin { get; set; }
        [ForeignKey("WyyzdmOrigin")]
        public virtual Td_kldm_Origin wyyzOrigin { get; set; }
        [Display(Name = "考生特征")]
        public string Kstz { set; get; }
        [Display(Name = "原始表考生特征")]
        public string KstzOrigin { set; get; }

        [Display(Name = "地区代码")]
        public long Dqdm { get; set; }
        [ForeignKey("Dqdm")]
        public virtual Td_dqdm dq { get; set; }
        [Display(Name = "原始表地区代码")]
        public long DqdmOrigin { get; set; }
        [ForeignKey("DqdmOrigin")]
        public virtual Td_dqdm_Origin dqOrigin { get; set; }
        [Display(Name = "证件类型代码")]
        public long Zjlxdm { get; set; }
        [ForeignKey("Zjlxdm")]
        public virtual Td_zjlxdm zjlx { get; set; }
        [Display(Name = "原始表证件类型代码")]
        public long ZjlxdmOrigin { get; set; }
        [ForeignKey("ZjlxdmOrigin")]
        public virtual Td_zjlxdm_Origin zjlxOrigin { get; set; }
        [Display(Name = "证件号码")]
        public string Zjhm { get; set; }
        [Display(Name = "考生学籍号")]
        public string Xjh { get; set; }
        [Display(Name = "外语口试")]
        public string Wyks { get; set; }
        [Display(Name = "外语听力")]
        public string Wytl { get; set; }
        [Display(Name = "通讯地址")]
        public string Txdz { get; set; }
        [Display(Name = "邮政编码")]
        public string Yzbm { get; set; }
        [Display(Name = "联系手机")]
        public string Lxsj { get; set; }
        [Display(Name = "联系电话")]
        public string Lxdh { get; set; }
        [Display(Name = "收件人")]
        public string Sjr { get; set; }
        [Display(Name = "考生特长")]
        public string Kstc { get; set; }
        [Display(Name = "考生奖励或处罚")]
        public string Ksjlhcf { get; set; }
        [Display(Name = "思想品德考核意见")]
        public string Sxpdkhyj { get; set; }
        [Display(Name = "考生资格")] 
        public string Kszg { get; set; }        
        [Display(Name = "原始表考生资格")]
        public string KszgOrigin { get; set; }
        [Display(Name = "户籍类别代码")]
        public long Hjlbdm { get; set; }
        [ForeignKey("Hjlbdm")]
        public virtual Td_hjlbdm hjlb { get; set; }
        [Display(Name = "原始表户籍类别代码")]
        public long HjlbdmOrigin { get; set; }
        [ForeignKey("HjlbdmOrigin")]
        public virtual Td_hjlbdm_Origin hjlbOrigin { get; set; }
        [Display(Name = "残障类别代码")]
        public long Czlbdm { get; set; }
        [ForeignKey("Czlbdm")]
        public virtual Td_czlbdm czlb { get; set; }
        [Display(Name = "原始表残障类别代码")]
        public long CzlbdmOrigin { get; set; }
        [ForeignKey("CzlbdmOrigin")]
        public virtual Td_czlbdm_Origin czlbOrigin { get; set; }
        [Display(Name = "选考科目")]  
        public string Xkkm { get; set; }
        [Display(Name = "选考科目原始码")]
        public string XkkmOrigin { get; set; }

        [Display(Name = "术科科目")]
        public string Skkm { get; set; }
        [Display(Name = "术科科目原始码")]
        public string SkkmOrigin { get; set; }
        [Display(Name = "考生临时操作标志")]
        public string Tag { get; set; }

        //public long ProvinceID { set; get; }
        //[ForeignKey("ProvinceID")]
        //public virtual Td_dqdm dq { get; set; }

        [NotMapped]
        public DateTime SearchStartCsrq { get; set; }
        [NotMapped]
        public DateTime SearchEndCsrq { get; set; }
        [NotMapped]
        public int? SearchStartYear { get; set; }
        [NotMapped]
        public int? SearchEndYear { get; set; }
        [NotMapped]
        public string SearchVerifyStatus { get; set; }
        [NotMapped]
        public string ByxxName { set; get; }

        public T_bmk()
        {
            //Csrq = Convert.ToDateTime("1905-03-14");
            VerifyStatus = "未审核";

            SearchStartYear = DateTime.Now.Year - 10;
            SearchEndYear = DateTime.Now.Year;
            SearchStartCsrq = DateTime.Now.AddYears(-30);
            SearchEndCsrq = DateTime.Now;
            SearchVerifyStatus = "";

            Zzmmdm = 0; ZzmmdmOrigin = 0;
            Mzdm = 0; MzdmOrigin = 0;
            Xtdwdm = 0; XtdwdmOrigin = 0;
            KldmOrigin = 0;
            Kslbdm = 0; KslbdmOrigin = 0;
            Kslxdm = 0; KslxdmOrigin = 0;
            Bylbdm = 0; BylbdmOrigin = 0;
            Byxxdm = 0; ByxxdmOrigin = 0;
            Wyyzdm = 0; WyyzdmOrigin = 0;
            Dqdm = 0; DqdmOrigin = 0;
            Zjlxdm = 0; ZjlxdmOrigin = 0;
            Hjlbdm = 0; HjlbdmOrigin = 0;
            Czlbdm = 0; CzlbdmOrigin = 0;
        }
    }

    #endregion
    #region 88 投档单 （T_tdd)
    [Table("T_tdd")]
    public class T_tdd : tblDelInfo
    {
        //[Display(Name ="管理员ID")]
        //public long DLID { get; set; }
        //[Display(Name ="考生号")]
        [Column(TypeName = "BIGINT")]
        public long RelationID { get; set; }
        [ForeignKey("RelationID")]
        public virtual T_bmk bmk { get; set; }
        [Display(Name = "逻辑志愿号")]
        public string Ljzyh { get; set; }
        //[Display(Name = "投档单位")]
        //public long Tddw { get; set; }
        //[ForeignKey("Tddw")]
        //public virtual T_tddw tddw { get; set; }
        [Display(Name = "原始表投档单位")]
        public long TddwOrigin { get; set; }
        [ForeignKey("TddwOrigin")]
        public virtual T_tddw_Origin tddwOrigin { get; set; }
        [Display(Name = "原始表批次代码")]
        public long PcdmOrigin { get; set; }
        [ForeignKey("PcdmOrigin")]
        public virtual Td_pcdm_Origin pcOrigin { get; set; }
        [Display(Name = "原始表科类代码")]
        public long KldmOrigin { get; set; }
        [ForeignKey("KldmOrigin")]
        public virtual Td_kldm_Origin klOrigin { get; set; }
        [Display(Name = "原始表计划性质代码")]
        public long JhxzdmOrigin { get; set; }
        [ForeignKey("JhxzdmOrigin")]
        public virtual Td_jhxzdm_Origin jhxzOrigin { get; set; }
        [Display(Name = "原始表计划类别代码")]
        public long JhlbdmOrigin { get; set; }
        [ForeignKey("JhlbdmOrigin")]
        public virtual Td_jhlbdm_Origin jhlbOrigin { get; set; }

        [Display(Name = "投档类型代码")]
        public long Tdlxdm { get; set; }
        [ForeignKey("Tdlxdm")]
        public virtual Td_tdlxdm tdlx { get; set; }
        [Display(Name = "原始投档类型代码")]
        public long TdlxdmOrigin { get; set; }
        [ForeignKey("TdlxdmOrigin")]
        public virtual Td_tdlxdm_Origin tdlxOrigin { get; set; }


        [Display(Name = "投档时间")]
        public DateTime Tdsj { get; set; }
        [Display(Name = "成绩")]
        public double Cj { get; set; }
        [Display(Name = "特征成绩")]
        public double Tzcj { get; set; }
        [Display(Name = "投档成绩")]
        public double Tdcj { get; set; }
        [Display(Name = "资格分")]
        public double Zgf { get; set; }
        [Display(Name = "院校导入成绩")]
        public double Yxdycj { get; set; }
        [Display(Name = "专业预投基准成绩")]
        public double Zyytjzcj { get; set; }
        [Display(Name = "专业预投附加成绩")]
        public double Zyytfjcj { get; set; }
        [Display(Name = "投档志愿")]
        public string Tdzy { get; set; }
        [Display(Name = "当前投档单位")]
        public string Dqtddw { get; set; }
        [Display(Name = "已投专业")]
        public long Ytzy { get; set; }
        [Display(Name = "录取专业")]
        public long Lqzy { get; set; }
        [Display(Name = "录取时间")]
        public DateTime Lqsj { get; set; }
        //[Display(Name = "录取类型代码")]  //录取类型就是录取方式？
        //public long Lqlxdm { get; set; }
        //[ForeignKey("Lqlxdm")]
        //public virtual Td_lqlxdm lqlx { get; set; }
        //[Display(Name = "原始表录取类型代码")]
        //public long LqlxdmOrigin { get; set; }
        //[ForeignKey("LqlxdmOrigin")]
        //public virtual Td_lqlxdm_Origin lqlxOrigin { get; set; }
        [Display(Name = "退档原因代码")]
        public long Tdyydm { get; set; }
        [ForeignKey("Tdyydm")]
        public virtual Td_tdyydm tdyy { get; set; }
        [Display(Name = "原始表退档原因代码")]
        public long TdyydmOrigin { get; set; }
        [ForeignKey("TdyydmOrigin")]
        public virtual Td_tdyydm_Origin tdyyOrigin { get; set; }
        [Display(Name = "专业合格")]
        public bool Zyhg { get; set; }
        [Display(Name = "体检限制")]
        public bool Tjhg { get; set; }
        [Display(Name = "学科科目合格")]
        public bool Xkkmhg { get; set; }
        [Display(Name = "术科科目合格")]
        public bool Skkmhg { get; set; }
        [Display(Name = "录取序号")]
        public string Lqxh { get; set; }
        [Display(Name = "投档单编号")]
        public string Tddbh { get; set; }
        [Display(Name = "下载编号")]
        public string Xzbh { get; set; }
        [Display(Name = "投档单状态")]
        public string Tddzt { get; set; }
        [Display(Name = "锁定标志")]
        public string Sdbz { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
        [Display(Name = "志愿特征")]
        public long Zytz { get; set; }
        [ForeignKey("Zytz")]
        public virtual Td_zytzdm zytz { get; set; }
        [Display(Name = "原始表志愿特征代码")]
        public long ZytzOrigin { get; set; }
        [ForeignKey("ZytzOrigin")]
        public virtual Td_zytzdm_Origin zytzOrigin { get; set; }
        [Display(Name = "志愿成绩")]
        public double Zycj { get; set; }
        [Display(Name = "志愿成绩同分排位")]
        public int Zycjtfpw { get; set; }
        [Display(Name = "专业调剂类型代码")]
        public long Zytjlxdm { get; set; }
        [ForeignKey("Zytjlxdm")]
        public virtual Td_zytjlxdm zytjlx { get; set; }
        [Display(Name = "原始表专业调剂类型代码")]
        public long ZytjlxdmOrigin { get; set; }
        [ForeignKey("ZytjlxdmOrigin")]
        public virtual Td_zytjlxdm_Origin zytjlxOrigin { get; set; }
        [Display(Name = "考生临时操作标志")]
        public string Kslsczbz { get; set; }
        [Display(Name = "录取方式代码")]
        public long Lqfs { get; set; }
        [ForeignKey("Lqfs")]
        public virtual Td_lqfsdm lqfs { get; set; }
        [Display(Name = "原始表录取方式代码")]
        public long LqfsOrigin { get; set; }
        [ForeignKey("LqfsOrigin")]
        public virtual Td_lqfsdm_Origin lqfsOrigin { get; set; }
        [Display(Name = "考生状态")]
        public string Kszt { get; set; }
        [Display(Name = "专业志愿调剂")]
        public Boolean Zyzytj { get; set; }
        [Display(Name = "定向调剂")]
        public Boolean Dxtj { get; set; }
        [Display(Name = "专业调剂服从")]
        public Boolean Zytjfc { get; set; }
        //[Display(Name = "体检结论代码")]
        //public long Tjjldm { get; set; }
        //[ForeignKey("Tjjldm")]
        //public virtual Td_tjjldm tjjl { get; set; }
        [Display(Name = "原始表体检结论代码")]
        public long TjjldmOrigin { get; set; }
        [ForeignKey("TjjldmOrigin")]
        public virtual Td_tjjldm_Origin tjjlOrigin { get; set; }
        //[Display(Name = "编号")]
        //public string Bh { get; set; }


        public T_tdd()
        {
            Tdsj = Convert.ToDateTime("1905-03-14");
            TddwOrigin = 0;
            PcdmOrigin = 0;
            KldmOrigin = 0;
            JhxzdmOrigin = 0;
            JhlbdmOrigin = 0;
            Tdlxdm = 0; TdlxdmOrigin = 0;
            Tdyydm = 0; TdyydmOrigin = 0;
            Zytz = 0; ZytzOrigin = 0;
            ZytjlxdmOrigin = 0;
            Lqfs = 0; LqfsOrigin = 0;
            TjjldmOrigin = 0;
        }
    }

    #endregion
    #region 89 志愿库(T_zyk)
    /// <summary>
    /// 89 志愿库(T_zyk)
    /// </summary>
    [Table("T_zyk")]
    public class T_zyk : tblInfo
    {
        //public int YearCode { set; get; }
        public long RelationID1 { get; set; }
        [ForeignKey("RelationID1")]
        public virtual T_bmk bmk { get; set; }
        public long RelationID2 { get; set; }
        [ForeignKey("RelationID2")]
        public virtual T_jhk jhk { get; set; }
        [Display(Name = "专业序号")]
        public short Zyxh { get; set; }
        [Display(Name = "专业代号")]
        public long ZydhOrigin { get; set; }
        [ForeignKey("ZydhOrigin")]
        public virtual Td_zydh_Origin zydhOrigin { get; set; }
        [Display(Name = "投档单位代码")]
        public long TddwdmOrigin { get; set; }
        [ForeignKey("TddwdmOrigin")]
        public virtual T_tddw_Origin tddwOrigin { get; set; }
        [Display(Name = "批次代码")]
        public long PcdmOrigin { get; set; }
        [ForeignKey("PcdmOrigin")]
        public virtual Td_pcdm_Origin pcOrigin { get; set; }
        [Display(Name = "科类代码")]
        public long KldmOrigin { get; set; }
        [ForeignKey("KldmOrigin")]
        public virtual Td_kldm_Origin klOrigin { get; set; }
        [Display(Name = "计划性质代码")]
        public long JhxzdmOrigin { get; set; }
        [ForeignKey("JhxzdmOrigin")]
        public virtual Td_jhxzdm_Origin jhxzOrigin { get; set; }
        [Display(Name = "计划类别代码")]
        public long JhlbdmOrigin { get; set; }
        [ForeignKey("JhlbdmOrigin")]
        public virtual Td_jhlbdm_Origin jhlbOrigin { get; set; }


        public T_zyk()
        {

        }
    }
    #endregion
    #region 90 考生简历(T_ksjl)
    /// <summary>
    /// 90 考生简历(T_ksjl)
    /// </summary>
    [Table("T_ksjl")]
    public class T_ksjl : tblInfo
    {
        [Column(TypeName = "BIGINT")]
        public long RelationID { get; set; }
        [ForeignKey("RelationID")]
        public virtual T_bmk bmk { get; set; }
        [Display(Name = "起始日期")]
        public DateTime Qsrq { get; set; }
        [Display(Name = "终结日期")]
        public DateTime Zjrq { get; set; }
        [Display(Name = "简历内容")]
        public string Jl { get; set; }
        [Display(Name = "任何职务")]
        public string Rhzw { get; set; }
        [Display(Name = "证明人")]
        public string Zmr { get; set; }
        
        public T_ksjl()
        {

        }
    }
    #endregion
    #region 91 考生学考成绩(T_ksxkcj)
    /// <summary>
    /// 91 考生学考成绩(T_ksxkcj)
    /// </summary>
    [Table("T_ksxkcj")]
    public class T_ksxkcj : tblInfo
    {
        [Column(TypeName = "BIGINT")]
        public long RelationID { get; set; }
        [ForeignKey("RelationID")]
        public virtual T_bmk bmk { get; set; }
        [Display(Name = "学考报名号")]
        public string Xkbmh { get; set; }
        [Display(Name = "科目代码")]
        public long Kmdm { get; set; }
        [ForeignKey("Kmdm")]
        public virtual Td_kmdm km { get; set; }
        [Display(Name = "原始表科目代码")]
        public long KmdmOrigin { get; set; }
        [ForeignKey("KmdmOrigin")]
        public virtual Td_kmdm_Origin kmOrigin { get; set; }
        [Display(Name = "成绩项代码")]
        public long Cjxdm { get; set; }
        [ForeignKey("Cjxdm")]
        public virtual Td_cjxdm cjx { get; set; }
        [Display(Name = "原始表成绩项代码")]
        public long CjxdmOrigin { get; set; }
        [ForeignKey("CjxdmOrigin")]
        public virtual Td_cjxdm_Origin cjxOrigin { get; set; }
        [Display(Name = "成绩类型，0：选考成绩 1：学考成绩 2：高考成绩")]
        public short Cjlx { get; set; }
        [Display(Name = "考试年月")]
        public DateTime? Ksny { get; set; }
        [Display(Name = "等第成绩")]
        public double Ddcj { get; set; }
        [Display(Name = "等级赋分")]
        public double Djff { get; set; }

        public T_ksxkcj()
        {
            Ddcj = 0;
        }
    }
    #endregion
    #region 92 体检信息 (T_tjxx)
    /// <summary>
    /// 92 体检信息 （T_tjxx)
    /// </summary>
    [Table("T_tjxx")]
    public class T_tjxx : tblDelInfo
    {
        [Column(TypeName = "BIGINT")]
        public long RelationID { get; set; }
        [ForeignKey("RelationID")]
        public virtual T_bmk bmk { get; set; }
        [Display(Name = "体检序号")]
        public string Tjxh { get; set; }
        [Display(Name = "既往病史标志（1-无，0-有）")]//注意默认值
        public string Jwbsbz { get; set; }
        [Display(Name = "既往病史")]
        public string Jwbs { get; set; }
        [Display(Name = "眼科，裸眼视力（右）")]
        public double Yk_lysly { get; set; }
        [Display(Name = "眼科，裸眼视力（左）")]
        public double Yk_lyslz { get; set; }
        [Display(Name = "眼科，矫正视力（右）")]
        public double Yk_jzsly { get; set; }
        [Display(Name = "眼科，矫正视力（左）")]
        public double Yk_jzslz { get; set; }
        [Display(Name = "眼科，矫正度数（右）")]
        public double Yk_jzdsy { get; set; }
        [Display(Name = "眼科，矫正度数（左）")]
        public double Yk_jzdsz { get; set; }
        [Display(Name = "眼科，彩色图案及数码检查（1-正常，2-其他）")]
        public string Yk_sjjc { get; set; }
        [Display(Name = "眼科，色觉检查图名称(1-喻自萍,2-其他)")]
        public string Yk_sjjct { get; set; }
        [Display(Name = "眼科.单色识别能力（红）（1-能识别，0-不能识别）)")]
        public string Yk_sjds1 { get; set; }
        [Display(Name = "眼科.单色识别能力（黄）（1-能识别，0-不能识别）)")]
        public string Yk_sjds2 { get; set; }
        [Display(Name = "眼科.单色识别能力（绿）（1-能识别，0-不能识别）)")]
        public string Yk_sjds3 { get; set; }
        [Display(Name = "眼科.单色识别能力（蓝）（1-能识别，0-不能识别）)")]
        public string Yk_sjds4 { get; set; }
        [Display(Name = "眼科.单色识别能力（紫）（1-能识别，0-不能识别）)")]
        public string Yk_sjds5 { get; set; }
        [Display(Name = "眼科医师意见(1-合格，2-专业受限，3-不合格)")]
        public string Yk_ysyj { get; set; }
        [Display(Name = "眼科.眼病（具体描述）")]
        public string Yk_yb { get; set; }
        [Display(Name = "内科.血压收缩压（高压）(单位:kpa)")]
        public double Nk_xyssy { get; set; }
        [Display(Name = "内科.血压舒张压（高压）(单位:kpa)")]
        public double Nk_xyszy { get; set; }
        [Display(Name = "内科.发育情况(1-良,2-中,3-差)")]
        public string Nk_fyqk { get; set; }
        [Display(Name = "内科.心脏及血管(1-正常,2-其他)")]
        public string Nk_xzjxg { get; set; }
        [Display(Name = "内科.呼吸系统(1-正常,2-其他)")]
        public string Nk_hxxt { get; set; }
        [Display(Name = "内科.神经系统(1-正常,2-其他)")]
        public string Nk_sjxt { get; set; }
        [Display(Name = "内科.肝(单位:厘米)")]
        public double Nk_g { get; set; }
        [Display(Name = "内科.肝性质(1-正常,2-其他)")]
        public string Nk_gxz { get; set; }
        [Display(Name = "内科.脾(单位:厘米)")]
        public double Nk_p { get; set; }
        [Display(Name = "内科.脾性质(1-正常,2-其他)")]
        public string Nk_pxz { get; set; }
        [Display(Name = "内科医师意见(1-合格，2-专业受限，3-不合格)")]
        public string Nk_ysyj { get; set; }
        [Display(Name = "内科.其他（具体描述）")]
        public string Nk_qt { get; set; }
        [Display(Name = "外科.身高(单位:厘米)")]
        public double Wk_sg { get; set; }
        [Display(Name = "外科.体重(单位:千克)")]
        public double Wk_tz { get; set; }
        [Display(Name = "外科.皮肤(1-正常,2-其他)")]
        public string Wk_pf { get; set; }
        [Display(Name = "外科.面部(1-正常,2-其他)")]
        public string Wk_mb { get; set; }
        [Display(Name = "外科.颈部(1-正常,2-其他")]
        public string Wk_jb { get; set; }
        [Display(Name = "外科.脊柱(1-正常,2-其他)")]
        public string Wk_jz { get; set; }
        [Display(Name = "外科.四肢(1-正常,2-其他)")]
        public string Wk_sz { get; set; }
        [Display(Name = "外科.关节(1-正常,2-其他)")]
        public string Wk_gj { get; set; }
        [Display(Name = "外科医师意见(1-合格，2-专业受限，3-不合格)")]
        public string Wk_ysyj { get; set; }
        [Display(Name = "外科.其他（具体描述）")]
        public string Wk_qt { get; set; }
        [Display(Name = "耳鼻喉科.左耳听力(单位:米)")]
        public double Eb_zetl { get; set; }
        [Display(Name = "耳鼻喉科.右耳听力(单位:米)")]
        public double Eb_yetl { get; set; }
        [Display(Name = "耳鼻喉科.嗅觉(1-正常,0-迟钝)")]
        public string Eb_xj { get; set; }
        [Display(Name = "耳鼻喉科医师意见(1-合格，2-专业受限，3-不合格)")]
        public string Eb_ysyj { get; set; }
        [Display(Name = "耳鼻喉科.耳鼻咽喉（具体描述）")]
        public string Eb_ebyh { get; set; }
        [Display(Name = "口腔科.唇腭(1-正常,2-其他)")]
        public string Kq_ce { get; set; }
        [Display(Name = "口腔科.是否口吃(1-否,0-是)")]
        public string Kq_sfkc { get; set; }
        [Display(Name = "口腔科.牙齿(1-正常,2-其他)")]
        public string Kq_yc { get; set; }
        [Display(Name = "口腔科医师意见(1-合格，2-专业受限，3-不合格)")]
        public string Kq_ysyj { get; set; }
        [Display(Name = "口腔科.其他（具体描述）")]
        public string Kq_qt { get; set; }
        [Display(Name = "胸透(1-正常,2-其他)")]
        public string Xt { get; set; }
        [Display(Name = "胸透医师意见(1-合格，2-专业受限，3-不合格)")]
        public string Xt_ysyj { get; set; }
        [Display(Name = "胸透.其他（具体描述）")]
        public string Xt_qt { get; set; }
        [Display(Name = "肝功能.转氨酶(1-正常,2-其他)")]
        public string Gg_zam { get; set; }
        [Display(Name = "肝功能.乙肝表面抗原(1-正常,2-其他)")]
        public string Gg_ygky { get; set; }
        [Display(Name = "肝功医师意见(1-合格，2-专业受限，3-不合格)")]
        public string Gg_ysyj { get; set; }
        [Display(Name = "肝功能.其他（具体描述）")]
        public string Gg_qt { get; set; }
        [Display(Name = "体检专业受限代码1")]
        public string Zysxdm1 { get; set; }
        [Display(Name = "体检专业受限代码2")]
        public string Zysxdm2 { get; set; }
        [Display(Name = "体检专业受限代码3")]
        public string Zysxdm3 { get; set; }
        [Display(Name = "体检专业受限代码4")]
        public string Zysxdm4 { get; set; }
        [Display(Name = "体检专业受限代码5")]
        public string Zysxdm5 { get; set; }
        [Display(Name = "体检专业受限代码6")]
        public string Zysxdm6 { get; set; }

        //[Display(Name ="体检结论代码(1-合格，2-专业受限，3-不合格)")]
        //public long Tjjldm { get; set; }
        public string Remark { get; set; }
        public T_tjxx()
        {

        }

    }

    #endregion

    public class BaseContext : DbContext
    {
        public BaseContext() : base(AppSettingValues.DB_CONN_STRING)
        {

        }
        public DbSet<tblUserInfo> UserInfo { get; set; }
        public DbSet<tblUserAuthorRelation> UserAuthorRelation { get; set; }
        //public DbSet<tblChannelAuthorRelation> ChannelAuthorRelation { get; set; }
        public DbSet<tblOperationInfo> OperationInfo { get; set; }
        public DbSet<tblAuthorizationInfo> AuthorizationInfo { get; set; }
        public DbSet<tblOperationAuthorRelation> OperationAuthorRelation { get; set; }
        public DbSet<tblDepartmentInfo> DepartmentInfo { get; set; }
        public DbSet<tblDutyInfo> DutyInfo { get; set; }

        public DbSet<tblOriginDataPackageInfo> OriginDataPackageInfo { get; set; }
        public DbSet<tblReportInfo> ReportInfo { get; set; }
        public DbSet<Td_bylbdm_Origin> bylbdmOriginTable { get; set; }
        public DbSet<Td_bylbdm> bylbdmTable { get; set; }
        public DbSet<Td_ccdm_Origin> ccdmOriginTable { get; set; }
        public DbSet<Td_ccdm> ccdmTable { get; set; }
        public DbSet<Td_cjxdm_Origin> cjxdmOriginTable { get; set; }
        public DbSet<Td_cjxdm> cjxdmTable { get; set; }
        public DbSet<Td_czlbdm_Origin> czlbdmOriginTable { get; set; }
        public DbSet<Td_czlbdm> czlbdmTable { get; set; }
        public DbSet<Td_dqdm_Origin> dqdmOriginTable { get; set; }
        public DbSet<Td_dqdm> dqdmTable { get; set; }
        public DbSet<Td_hjlbdm_Origin> hjlbdmOriginTable { get; set; }
        public DbSet<Td_hjlbdm> hjlbdmTable { get; set; }
        public DbSet<Td_jhlbdm_Origin> jhlbdmOriginTable { get; set; }
        public DbSet<Td_jhlbdm> jhlbdmTable { get; set; }
        public DbSet<Td_jhxzdm_Origin> jhxzdmOriginTable { get; set; }
        public DbSet<Td_jhxzdm> jhxzdmTable { get; set; }
        public DbSet<Td_kldm_Origin> kldmOriginTable { get; set; }
        public DbSet<Td_kldm> kldmTable { get; set; }
        public DbSet<Td_kmdm_Origin> kmdmOriginTable { get; set; }
        public DbSet<Td_kmdm> kmdmTable { get; set; }
        public DbSet<Td_kslbdm_Origin> kslbdmOriginTable { get; set; }
        public DbSet<Td_kslbdm> kslbdmTable { get; set; }
        public DbSet<Td_kslxdm_Origin> kslxdmOriginTable { get; set; }
        public DbSet<Td_kslxdm> kslxdmTable { get; set; }
        public DbSet<Td_kstzdm_Origin> kstzdmOriginTable { get; set; }
        public DbSet<Td_kstzdm> kstzdmTable { get; set; }
        public DbSet<Td_kszgdm_Origin> kszgdmOriginTable { get; set; }
        public DbSet<Td_kszgdm> kszgdmTable { get; set; }
        public DbSet<Td_lqfsdm_Origin> lqfsdmOriginTable { get; set; }
        public DbSet<Td_lqfsdm> lqfsdmTable { get; set; }
        public DbSet<Td_mzdm_Origin> mzdmOriginTable { get; set; }
        public DbSet<Td_mzdm> mzdmTable { get; set; }
        public DbSet<Td_pcdm_Origin> pcdmOriginTable { get; set; }
        public DbSet<Td_pcdm> pcdmTable { get; set; }
        public DbSet<Td_tdlxdm_Origin> tdlxdmOriginTable { get; set; }
        public DbSet<Td_tdlxdm> tdlxdmTable { get; set; }
        public DbSet<Td_tdyydm_Origin> tdyydmOriginTable { get; set; }
        public DbSet<Td_tdyydm> tdyydmTable { get; set; }
        public DbSet<Td_tjjldm_Origin> tjjldmOriginTable { get; set; }
        public DbSet<Td_tjjldm> tjjldmTable { get; set; }
        public DbSet<Td_wyyzdm_Origin> wyyzdmOriginTable { get; set; }
        public DbSet<Td_wyyzdm> wyyzdmTable { get; set; }
        public DbSet<Td_xtdwdm_Origin> xtdwdmOriginTable { get; set; }
        public DbSet<Td_xtdwdm> xtdwdmTable { get; set; }
        //public DbSet<Td_zcdm_Origin> zcdmOriginTable { get; set; }
        //public DbSet<Td_zcdm> zcdwdmTable { get; set; }
        public DbSet<Td_xzdm_Origin> xzdmOriginTable { get; set; }
        public DbSet<Td_xzdm> xzdwdmTable { get; set; }
        public DbSet<Td_zjlxdm_Origin> zjlxdmOriginTable { get; set; }
        public DbSet<Td_zjlxdm> zjlxdmTable { get; set; }
        public DbSet<Td_zklxdm_Origin> zklxdmOriginTable { get; set; }
        public DbSet<Td_zklxdm> zklxdmTable { get; set; }
        public DbSet<Td_zylbdm_Origin> zylbdmOriginTable { get; set; }
        public DbSet<Td_zylbdm> zylbdmTable { get; set; }
        public DbSet<Td_zytjlxdm_Origin> zytjlxdmOriginTable { get; set; }
        public DbSet<Td_zytjlxdm> zytjlxdmTable { get; set; }
        public DbSet<Td_zytzdm_Origin> zytzdmOriginTable { get; set; }
        public DbSet<Td_zytzdm> zytzdmTable { get; set; }
        public DbSet<Td_zzmmdm_Origin> zzmmdmOriginTable { get; set; }
        public DbSet<Td_zzmmdm> zzmmdmTable { get; set; }
        public DbSet<Td_byxxdm_Origin> byxxdmOriginTable { get; set; }
        public DbSet<Td_byxxdm> byxxdmTable { get; set; }
        public DbSet<Td_gxdm> gxdmTable { get; set; }
        public DbSet<Td_xbdm_Origin> xbdmOriginTable { get; set; }
        public DbSet<Td_xbdm> xbdmTable { get; set; }
        public DbSet<tblReportType> ReportType { get; set; }
        public DbSet<T_tddw_Origin> tddwOriginTable { get; set; }
        public DbSet<T_tddw> tddwTable { get; set; }
        public DbSet<Td_zydh_Origin> zydhOriginTable { get; set; }
        public DbSet<T_jhk> jhkTable { get; set; }
        public DbSet<T_bmk_Origin> bmkOriginTable { get; set; }
        public DbSet<T_bmk> bmkTable { get; set; }
        public DbSet<T_tdd> tddTable { get; set; }
        public DbSet<T_zyk> zykTable { get; set; }
        public DbSet<T_ksjl> ksjlTable { get; set; }
        public DbSet<T_ksxkcj> ksxkcjTable { get; set; }
        public DbSet<T_tjxx> tjxxTable { get; set; }
        


        public void InitData()
        {
            var db = new BaseContext();

            var author = new List<tblAuthorizationInfo>
            {
                new tblAuthorizationInfo { ID = 1, Name = "超级管理员", TheOrder = 1, DelFlag = false },

            };
            author.ForEach(s => db.AuthorizationInfo.Add(s));

            var duty = new List<tblDutyInfo>
            {
                new tblDutyInfo { ID = 1, Name = "管理员", TheOrder = 1, DelFlag = false },

            };
            duty.ForEach(s => db.DutyInfo.Add(s));


            var user = new tblUserInfo { ID = 1, UserICode = "admin", Name = "admin", UserIPsd = "QRxuC4I7poBegu/OyCNkvA==", UserIRegisterDate = DateTime.Now, UserIBirth = DateTime.Now, DelFlag = false, UserIDutyIID = 1, UserIDepartmentIID = 1 };
            db.UserInfo.Add(user);

            var userauthor = new tblUserAuthorRelation { AuthorID = 1, UserID = 1 };
            db.UserAuthorRelation.Add(userauthor);

            var oper = new List<tblOperationInfo>
            {
                new tblOperationInfo { ID = 1, Name = "用户权限管理", Code="adminSystemManage",  ParentIndex = -1, TheLevel = 1, TheOrder = 6, ChildNum = 7, IsLeaf = false, DelFlag = false },
                new tblOperationInfo { ID = 2, Name = "菜单管理", Code="OperationInfo", ParentIndex = 1, TheLevel = 2, TheOrder = 2, ChildNum = 0, IsLeaf = true, DelFlag = false },
                new tblOperationInfo { ID = 3, Name = "角色管理", Code="/adminMain/SimpleDlg/?key=AuthorizationInfo", ParentIndex = 1, TheLevel = 2, TheOrder = 3, ChildNum = 0, IsLeaf = true, DelFlag = false },
                new tblOperationInfo { ID = 4, Name = "职务管理", Code="/adminMain/SimpleDlg/?key=DutyInfo", ParentIndex = 1, TheLevel = 2, TheOrder = 4, ChildNum = 0, IsLeaf = true, DelFlag = false },
                new tblOperationInfo { ID = 5, Name = "角色权限设置", Code="RoleAuthorization", ParentIndex = 1, TheLevel = 2, TheOrder = 5, ChildNum = 0, IsLeaf = true, DelFlag = false },
                new tblOperationInfo { ID = 6, Name = "用户基本信息", Code="UserInfo", ParentIndex = 1, TheLevel = 2, TheOrder = 7, ChildNum = 0, IsLeaf = true, DelFlag = false },
                new tblOperationInfo { ID = 7, Name = "全局常量配置", Code="SetConstValue", ParentIndex = 1, TheLevel = 2, TheOrder = 1, ChildNum = 0, IsLeaf = true, DelFlag = false },
                new tblOperationInfo { ID = 8, Name = "部门班级管理", Code="DepartmentInfo", ParentIndex = 1, TheLevel = 2, TheOrder = 6, ChildNum = 0, IsLeaf = true, DelFlag = false }
            };
            oper.ForEach(s => db.OperationInfo.Add(s));

            var AuthorOper = new List<tblOperationAuthorRelation>
            {
                new tblOperationAuthorRelation { AuthorID = 1, OperationID = 2, AddFlag = true, DeleteFlag = true, EditFlag = true, SearchFlag = true },
                new tblOperationAuthorRelation { AuthorID = 1, OperationID = 3, AddFlag = true, DeleteFlag = true, EditFlag = true, SearchFlag = true },
                new tblOperationAuthorRelation { AuthorID = 1, OperationID = 4, AddFlag = true, DeleteFlag = true, EditFlag = true, SearchFlag = true },

     
            };
            AuthorOper.ForEach(s => db.OperationAuthorRelation.Add(s));


            db.SaveChanges();
        }


        protected override void Dispose(bool disposing)
        {
            Dispose();
            base.Dispose(disposing);
        }

    }
}
#endregion