namespace UP.Web.Models.Admin.ModulesManager
{
    public class ModulesParam : BaseModel
    {
        ///<summary>
        ///ID[修改时必填项目]
        ///
        public int id { get; set; }

        ///<summary>
        ///上级ID
        ///</summary>
        public int parentid { get; set; }

        ///<summary>
        ///模块编码
        ///</summary>
        public string code { get; set; }

        ///<summary>
        ///模块名称
        ///</summary>
        public string name { get; set; }

        ///<summary>
        ///模块路径
        ///</summary>
        public string path { get; set; }

        ///<summary>
        ///是否停用
        ///</summary>
        public int disable { get; set; }

        ///<summary>
        ///排列序号
        ///</summary>
        public int sort { get; set; }

        ///<summary>
        ///模块图标
        ///</summary>
        public string icon { get; set; }

        /// <summary>
        /// 是否首页
        /// </summary>
        public int homepage { get; set; }
    }
}
