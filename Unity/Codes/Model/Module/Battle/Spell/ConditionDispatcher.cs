using System.Collections.Generic;

namespace ET
{
    public class ConditionDispatcher :Entity
    {
        public static ConditionDispatcher Instance;

        public Dictionary<int, ICondition> SpellPreConditions = new Dictionary<int, ICondition>();
    }
}