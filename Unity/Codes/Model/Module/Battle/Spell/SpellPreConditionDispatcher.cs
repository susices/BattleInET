using System.Collections.Generic;

namespace ET
{
    public class SpellPreConditionDispatcher :Entity
    {
        public static SpellPreConditionDispatcher Instance;

        public Dictionary<int, ISpellPreCondition> SpellPreConditions = new Dictionary<int, ISpellPreCondition>();
    }
}