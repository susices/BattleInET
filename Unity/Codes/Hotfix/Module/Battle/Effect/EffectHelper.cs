namespace ET
{
    public static class EffectHelper
    {
        /// <summary>
        /// 技能实体施放效果
        /// </summary>
        public static void CastEffect(this SpellEntity self, int effectConfigId)
        {
            EffectFactory.Create(self, self, effectConfigId);
        }
        
        /// <summary>
        /// 技能实体施放效果
        /// </summary>
        public static void CastEffect(this SpellEntity self, int[] effectConfigIds)
        {
            for (int i = 0; i < effectConfigIds.Length; i++)
            {
                EffectFactory.Create(self, self, effectConfigIds[i]);
            }
        }

        /// <summary>
        /// Buff实体施放效果
        /// </summary>
        public static void CastEffect(this BuffEntity self, int effectConfigId)
        {
            EffectFactory.Create(self, self, effectConfigId);
        }
        
        /// <summary>
        /// Buff实体施放效果
        /// </summary>
        public static void CastEffect(this BuffEntity self, int[] effectConfigIds)
        {
            for (int i = 0; i < effectConfigIds.Length; i++)
            {
                EffectFactory.Create(self, self, effectConfigIds[i]);
            }
        }
    }
}