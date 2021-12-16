namespace ET
{
    public static class EffectFactory
    {
        public static EffectEntity Create(Entity targetEntity, Entity sourceEntity, int effectConfigId)
        {
            var effectEntity = targetEntity.AddChild<EffectEntity, Entity, int>(sourceEntity, effectConfigId);
            return effectEntity;
        }
    }
}