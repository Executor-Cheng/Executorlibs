namespace Executorlibs.Shared.Protocol.Models.Danmaku
{
    /// <summary>
    /// 全平台通用的头衔消息接口
    /// </summary>
    public interface ITitleMessage<TTitle> where TTitle : ITitle
    {
        TTitle? Title { get; }
    }
}
