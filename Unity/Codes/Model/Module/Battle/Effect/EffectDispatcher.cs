using System.Collections.Generic;

namespace ET
{
    public class EffectDispatcher : Entity
    {
        public static EffectDispatcher Instance;

        public Dictionary<int, IEffect> Effects = new Dictionary<int, IEffect>();
        
    }
}