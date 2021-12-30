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
        /// Buff配置
        /// </summary>
        public BuffConfig BuffConfig
        {
            get
            {
                return BuffConfigCategory.Instance.Get(this.BuffConfigId);
            }
        }

        /// <summary>
        /// Buff状态
        /// </summary>
        public int State;

        /// <summary>
        /// 当前Buff层数
        /// </summary>
        public int CurrentLayer;

        /// <summary>
        /// Buff施加来源的实体id
        /// </summary>
        public long SourceEntityId;
        
    }
}