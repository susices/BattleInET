using System.Collections.Generic;

namespace ET
{
    public class BuffActionDispatcher : Entity
    {
        public static BuffActionDispatcher Instance;

        public Dictionary<int, IBuffAction> BuffActions = new Dictionary<int, IBuffAction>();
        
    }
}