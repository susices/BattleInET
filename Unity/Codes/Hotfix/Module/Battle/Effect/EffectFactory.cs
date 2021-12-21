namespace ET
{
    public static class EffectFactory
    {
        public static EffectEntity Create(Entity CastEffectEntity, int effectConfigId)
        {
            var effectEntity = CastEffectEntity.AddChild<EffectEntity>();
            effectEntity.EffectConfigId = effectConfigId;
            return effectEntity;
        }
    }
}