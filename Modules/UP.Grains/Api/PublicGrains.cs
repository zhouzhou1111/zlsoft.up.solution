using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Basics.Models;
using UP.Interface.Api;
using UP.Logics.Api.Public;
using UP.Models.Api;

namespace UP.Grains.Api
{
  public  class PublicGrains : BasicGrains<PublicLogic>, IPublic
    {
        /// <summary>
        /// 查询数据字典信息
        /// </summary>
        /// <returns></returns>
        public Task<JsonMsg<List<DicInfo>>> GetDicItems(DicParam entity) {
            return Task.FromResult(this.Logic.GetDicItems(entity));
        }

    }
}
