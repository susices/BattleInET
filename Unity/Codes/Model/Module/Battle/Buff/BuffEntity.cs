using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// Buff实体
    /// </summary>
    public class BuffEntity : Entity
    {
        /// <summary>
        /// buff配置表Id
        /// </summary>
        public int BuffConfigId;

        /// <summary>
        /// Buff状态
        /// </summary>
        public int State;

        /// <summary>
        /// 当前Buff层数
        /// </summary>
        public int CurrentLayer;

        /// <summary>
        /// Buff施加来源的实体
        /// </summary>
        public Entity SourceEntity;

        /// <summary>
        /// 被添加到的Buff管理组件
        /// </summary>
        public BuffComponent BuffComponent;
    }
}