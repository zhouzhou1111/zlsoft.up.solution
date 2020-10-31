/*********************************************************
* 功能：数据表转换树形列表
* 描述：返回element-ui树形数据
* 作者：赵洪文
* 日期：2020-01-10
*********************************************************/
using QWPlatform.DataIntface.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace UP.Basics
{
    public static class TreeMemoExtend
    {
        private static T CreateNode<T>(IDataReader dr, List<string> fields)
        {
            T model = Activator.CreateInstance<T>();
            foreach (PropertyInfo pi in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
            {
                if (fields.Contains(pi.Name.ToLower()))
                {
                    var val = dr[pi.Name.ToLower()];
                    if (val is System.DBNull)
                    {
                        val = val.ToString();
                    }
                    pi.SetValue(model, val);
                }
            }
            return model;
        }
        public static List<T> GetModelListTree<T>(this ISelectBuilder source)
        {
            IDataReader dr = source.DataReader();
            TreeMemoAttribute attr = typeof(T).GetCustomAttributes(typeof(TreeMemoAttribute), false)[0] as TreeMemoAttribute;
            List<T> items = new List<T>();
            List<T> quene = new List<T>();
            List<string> fields = new List<string>();
            using (dr)
            {
                while (dr.Read())
                {
                    if (fields.Count == 0)
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            fields.Add(dr.GetName(i).ToLower());
                        }
                    if (dr[attr.PID].Equals("") || dr[attr.PID] is System.DBNull)
                    {
                        items.Add(CreateNode<T>(dr, fields));
                    }
                    else
                    {
                        var pid = items.Find(p => p.GetType().GetProperty(attr.ID).GetValue(p).Equals(dr[attr.PID].ToString()));
                        if (pid == null)
                        {
                            quene.Add(CreateNode<T>(dr, fields));
                        }
                        else
                        {
                            List<T> temp = pid.GetType().GetProperty(attr.Children).GetValue(pid) as List<T>;
                            if (temp == null)
                                temp = new List<T>();
                            temp.Add(CreateNode<T>(dr, fields));
                            pid.GetType().GetProperty(attr.Children).SetValue(pid, temp);

                        }
                    }
                    Create(items, quene, attr);
                }
            }
            return items;
        }
        /// <summary>
        /// 反射创建节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="dt"></param>
        /// <returns></returns>

        static void Create<T>(List<T> items, List<T> quene, TreeMemoAttribute attr)
        {
            if (quene.Count == 0)
                return;
            if (items == null || items.Count == 0)
                return;
            foreach (T item in items)
            {
                Predicate<T> filter = p => p.GetType().GetProperty(attr.PID).GetValue(p).Equals(item.GetType().GetProperty(attr.ID).GetValue(item));
                List<T> child = item.GetType().GetProperty(attr.Children).GetValue(item) as List<T>;
                var nodes = quene.FindAll(filter); //存在子节点
                if (nodes.Count > 0)
                {
                    if (child == null)
                        child = new List<T>();
                    child.AddRange(nodes);
                    item.GetType().GetProperty(attr.Children).SetValue(item, child);
                    quene.RemoveAll(filter);
                }
                if (quene.Count == 0)
                    break;
                Create(child, quene, attr);
            }
        }
    }
}
