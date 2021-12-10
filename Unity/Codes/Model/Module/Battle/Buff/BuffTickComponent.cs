namespace ET
{
    /// <summary>
    /// buff实体轮询组件
    /// </summary>
    public class BuffTickComponent:Entity
    {
        /// <summary>
        /// buff配置表Id
        /// </summary>
        public int BuffConfigId;

        /// <summary>
        /// buff轮询间隔时间
        /// </summary>
        public long BuffTickTimeSpan;

        /// <summary>
        /// buffTick计时器Id
        /// </summary>
        public long BuffTickTimerId;
        
    }
}