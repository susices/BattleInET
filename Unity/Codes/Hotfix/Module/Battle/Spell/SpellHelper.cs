namespace ET
{
    public static class SpellHelper
    {
        /// <summary>
        /// 检查技能释放前提条件是否满足
        /// </summary>
        /// <param name="spellConfigId"></param>
        /// <param name="spellComponent"></param>
        /// <returns></returns>
        public static bool CheckSpellPreCondition(int spellConfigId, SpellComponent spellComponent)
        {
            int[] preConditions = SpellConfigCategory.Instance.Get(spellConfigId).PreCondition;
            if (preConditions==null || preConditions.Length==0)
            {
                return true;
            }
            
            for (int i = 0; i < preConditions.Length; i++)
            {
                ConditionConfig spellConditionConfig = ConditionConfigCategory.Instance.Get(preConditions[i]);
                int baseSpellPreConditionId = spellConditionConfig.BaseConditionId;
                int[] args = spellConditionConfig.ConditionArgs;

                if (ConditionDispatcher.Instance.CheckCondition(spellComponent, baseSpellPreConditionId, args))
                {
                    return true;
                }
            }
            return false;
        }

        public static async ETTask<bool> TryCastSpell(int spellConfigId, SpellComponent spellComponent)
        {
            await ETTask.CompletedTask;
            if (!CheckSpellPreCondition(spellConfigId, spellComponent))
            {
                return false;
            }
            
            // wenchao 执行施法流程

            return true;
            
        }
    }
}