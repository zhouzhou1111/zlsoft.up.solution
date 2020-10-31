using Orleans;
using Orleans.Concurrency;
using QWPlatform.SystemLibrary.Provider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UP.Basics
{
    /// <summary>
    /// 基础Grains
    /// </summary>
    [Reentrant]
    public class BasicGrains<T> : Grain where T : new()
    {
        /// <summary>
        /// 获取逻辑层对象
        /// </summary>
        public T Logic
        {
            get
            {
                return SingletonProvider<T>.Instance;
            }
        }

        /// <summary>
        /// 创建一个单实例的对象
        /// </summary>
        /// <typeparam name="T1">类型名称</typeparam>
        /// <returns></returns>
        public T1 CreateLogic<T1>() where T1 : new()
        {
            return SingletonProvider<T1>.Instance;
        }

        /// <summary>
        /// 直接创建一个实例（由服务端调用其它服务的实例）
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public T2 CreateGrain<T2>() where T2 : IGrainWithIntegerKey, IGrainWithGuidKey
        {
            return this.GrainFactory.GetGrain<T2>(Guid.NewGuid());
        }
    }
}
