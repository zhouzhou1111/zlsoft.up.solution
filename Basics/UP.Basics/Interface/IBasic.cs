using Orleans;

namespace UP.Basics
{
    /// <summary>
    /// 所有服务的基类
    /// </summary>
    public interface IBasic : IGrainWithIntegerKey, IGrainWithGuidKey, IGrainWithGuidCompoundKey
    {

    }
}
