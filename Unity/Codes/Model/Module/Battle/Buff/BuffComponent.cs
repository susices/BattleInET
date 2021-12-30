using System.Collections.Generic;

namespace ET
{
    public class BuffComponent:Entity
    {
        /// <summary>
        /// Buff状态
        /// </summary>
        public Dictionary<int, int> BuffStateSets = new Dictionary<int, int>();
    }
}