namespace UP.Basics
{
    /// <summary>
    /// 所有响应Json的格式
    /// </summary>
    public enum ResponseCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,

        /// <summary>
        /// 语义有误，当前请求无法被服务器理解 或 请求参数有误
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// 无权限访问
        /// </summary>
        NotAuthority = 401,

        /// <summary>
        /// 服务器拒绝请求
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// 未找到资源
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// 服务器内部异常
        /// </summary>
        Error = 500
    }

    /// <summary>
    /// 定义分组的名的枚举(用于swagger的分组显示)
    /// </summary>
    public enum ApiGroupNames
    {
        /// <summary>
        /// 平台管理
        /// </summary>
        [GroupInfo(Title = "平台管理", Description = "平台登录、角色权限相关接口", Version = "v1.0")]
        Admin,

        /// <summary>
        /// 业务系统管理
        /// </summary>
        [GroupInfo(Title = "业务系统管理", Description = "业务系统管理接口", Version = "v1.0")]
        Business,

        /// <summary>
        /// 基础数据管理
        /// </summary>
        [GroupInfo(Title = "基础数据管理", Description = "基础数据管理接口", Version = "v1.0")]
        Basics,

        /// <summary>
        /// 公共接口
        /// </summary>
        [GroupInfo(Title = "公共接口", Description = "所有业务公共部分接口，比如发短信、上传附件等", Version = "v1.0")]
        PUBLIC
    }

    /// <summary>
    /// 操作符定义
    /// </summary>
    public enum Operational
    {
        /// <summary>
        /// 值相等
        /// </summary>
        Equal,

        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual,

        /// <summary>
        /// 大于
        /// </summary>
        Greater,

        /// <summary>
        /// 小于
        /// </summary>
        Less,

        /// <summary>
        /// In的泛围值
        /// </summary>
        In,

        /// <summary>
        /// 不在指定泛围
        /// </summary>
        NotIn,

        /// <summary>
        /// 模糊查询
        /// </summary>
        Like,

        /// <summary>
        /// 左侧模糊查询
        /// </summary>
        LeftLike,

        /// <summary>
        /// 右侧模糊查询
        /// </summary>
        RightLike
    }

    /// <summary>
    /// 使用条件运行符类型
    /// </summary>
    public enum WhereType
    {
        /// <summary>
        /// And条件
        /// </summary>
        And,

        /// <summary>
        /// Or条件
        /// </summary>
        Or
    }

    /// <summary>
    /// 数据库操作类型
    /// </summary>
    public enum DBAction
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        Insert,

        /// <summary>
        /// 更新数据
        /// </summary>
        Update,

        /// <summary>
        /// 删除数据
        /// </summary>
        Delete,

        /// <summary>
        /// 查询数据
        /// </summary>
        Select
    }

    /// <summary>
    /// 用户类型 管理员=1,医生=2,其他=3
    /// </summary>
    public enum UserType
    {
        管理员 = 1, 医生 = 2, 其他 = 3
    }

    /// <summary>
    /// 使用平台 PC = 1, H5 = 2, 安卓 = 3,IOS=4
    /// </summary>
    public enum UserSource
    {
        PC = 1, H5 = 2, 安卓 = 3, IOS = 4
    }

    /// <summary>
    /// 字典表类型: b_民族 = 1, b_社会性别 = 2,b_学历 = 3, b_职务 = 4, b_abo血型=5, b_rh血型 = 6, b_国籍 = 7, b_过敏源（F） = 8,b_疾病编码分类（F） = 9,b_社会关系=10,b_疾病编码类别（F） = 11,b_疾病编码目录 （F）= 12,b_卫生机构类别（F） = 13,b_行政区划（F） = 14,b_职业 = 15,
    /// </summary>
    public enum DicType
    {
        b_民族 = 1, 
        b_社会性别 = 2,
        b_学历 = 3, 
        b_职务 = 4, 
        b_abo血型=5, 
        b_rh血型 = 6, 
        b_国籍 = 7, 
        b_过敏源 = 8,
        b_疾病编码分类 = 9,
        b_社会关系=10,
        b_疾病编码类别 = 11,
        b_疾病编码目录 = 12,
        b_卫生机构类别 = 13,
        b_行政区划 = 14,
        b_职业 = 15,
    }
}