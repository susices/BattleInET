namespace ET
{
    public static class SpellHelper
    {
        /// <summary>
        /// 检查技能释放前提条件是否满足
        /// </summary>
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

        /// <summary>
        /// 施放技能
        /// </summary>
        public static async ETTask CastSpell(int spellConfigId, SpellComponent spellComponent)
        {
            await ETTask.CompletedTask;
            var spellEntity = SpellFactory.Create(spellComponent, spellConfigId);
            var spellConfig = spellEntity.SpellConfig;
            
            for (int i = 0; i < spellConfig.CastEffects.Length; i+=2)
            {
                if (await TimerComponent.Instance.WaitAsync(spellConfig.CastEffects[i], spellEntity.SpellCancellationToken))
                {
                    EffectHelper.CastEffect(spellEntity,spellConfig.CastEffects[i+1]);
                }else
                {
                    return;
                }
            }
            return;
        }
        
        /// <summary>
        /// 打断技能
        /// </summary>
        public static bool InterruptSpell(SpellEntity InterruptedSpell)
        {
            if (!InterruptedSpell.SpellConfig.CanInterrpted || InterruptedSpell.SpellCancellationToken==null)
            {
                return false;
            }
            InterruptedSpell.SpellCancellationToken.Cancel();
            var SpellConfig = InterruptedSpell.SpellConfig;
            for (int i = 0; i < SpellConfig.InterruptedEffects.Length; i++)
            {
                EffectHelper.CastEffect(InterruptedSpell,SpellConfig.InterruptedEffects[i]);
            }
            InterruptedSpell.Dispose();
            return true;
        }
    }
}