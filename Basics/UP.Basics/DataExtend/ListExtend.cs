using System;
using System.Collections.Generic;
using UP.Basics.LayuiModels;

namespace UP.Basics
{
    /// <summary>
    /// 列表扩展
    /// </summary>
    public static class ListExtend
    {
        #region 转为Json树
        /// <summary>
        /// 将List转换为TreeJson数据
        /// </summary>
        /// <typeparam name="T">解决的类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="IdName">ID字段名称</param>
        /// <param name="parentName">父级字段名称</param>
        /// <param name="titleName">标题名称</param>
        /// <param name="urlName">URL名称</param>
        /// <param name="checkedName">选中状态的名称</param>
        /// <param name="disabledName">禁止列的名称</param>
        /// <returns></returns>
        public static string ToTreeJson<T>(this IList<T> list, string IdName,
                                            string parentName, string titleName,
                                            string urlName = "", string checkedName = "", string disabledName = "")
        {
            if (IdName.IsNullOrEmpty() || parentName.IsNullOrEmpty() || titleName.IsNullOrEmpty())
            {//检查必填项目
                throw new ArgumentException("ID列，上级列，标题列是必须指定项目");
            }


            var type = typeof(T);
            if (list != null)
            {//所有集合
                var alltreeList = new List<TreeModel>();
                var resultList = new List<TreeModel>();
                foreach (var item in list)
                {//循环每一行记录 
                    var id = GetObjectValue(item, type, IdName);//主键
                    var parent = GetObjectValue(item, type, parentName);//上级ID
                    var title = GetObjectValue(item, type, titleName);//标题
                    var url = GetObjectValue(item, type, urlName);//url链接
                    var check = GetObjectValue(item, type, checkedName);//选择状态
                    var disabled = GetObjectValue(item, type, disabledName);//禁用状态

                    var model = new TreeModel
                    {
                        Id = id?.ToString(),
                        ParentId = parent?.ToString(),
                        Title = title?.ToString(),
                        Href = url?.ToString(),
                        Checked = check?.ToInt32() == 1,
                        Disabled = disabled?.ToInt32() == 1
                    };

                    //添加到集合中
                    alltreeList.Add(model);
                }

                //查找第一级节点(根节点)
                var firstNode = alltreeList.FindAll(p => p.ParentId.IsNullOrEmpty() || p.ParentId == "0");
                if (firstNode != null)
                {
                    firstNode.ForEach(p =>
                    {
                        FindSubNode(p, alltreeList);
                        //添加到节点集合中
                        resultList.Add(p);
                    });

                    //将list结果转换为json
                    return resultList.ToJson();
                }
            }

            //返回空字符串
            return string.Empty;
        }

        //获取对象中的值
        private static object GetObjectValue(object obj, Type type, string attrName)
        {
            var propObj = type.GetProperty(attrName);
            if (propObj != null)
            {
                return propObj.GetValue(obj);
            }

            return null;
        }

        //查找子级节点
        private static void FindSubNode(TreeModel model, List<TreeModel> allList)
        {
            var list = allList.FindAll(p => p.ParentId == model.Id);
            if (list != null)
            {
                model.Children = list;
                list.ForEach(p =>
                {//所有子级查询
                    FindSubNode(p, allList);
                });
            }
        }

        #endregion
    }
}
