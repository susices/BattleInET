using System;

namespace ET
{
    public static class EffectHelper
    {
        public const int LogicEffectStartIndex = 10000;

        public const int ViewEffectStartIndex = 60000;
        
        /// <summary>
        /// 施放效果
        /// </summary>
        public static void CastEffect(Entity castEffectEntity, int effectConfigId)
        {
            

            Type castEffectEntityType = castEffectEntity.GetType();
            if (castEffectEntityType!= typeof(SpellEntity) && castEffectEntityType!=typeof(BuffEntity))
            {
                return;
            }

            var effectEntity =  EffectFactory.Create(castEffectEntity, effectConfigId);
            EffectDispatcher.Instance.RunEffect(effectEntity, effectConfigId);
            effectEntity.Dispose();
        }
    }
}