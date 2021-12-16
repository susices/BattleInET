namespace ET
{
    [ObjectSystem]
    public class EffectEntityAwakeSystem : AwakeSystem<EffectEntity, Entity, int>
    {
        public override void Awake(EffectEntity self, Entity sourceEntity, int effectConfigId)
        {
            self.EffectConfigId = effectConfigId;
            self.SourceEntityId = sourceEntity.Id;
        }
    }
    
    [ObjectSystem]
    public class EffectEntityDestorySystem : DestroySystem<EffectEntity>
    {
        public override void Destroy(EffectEntity self)
        {
            self.EffectConfigId = 0;
            self.SourceEntityId = 0;
        }
    }

    public static class EffectEntitySystem
    {
        /// <summary>
        /// 执行效果
        /// </summary>
        /// <param name="self"></param>
        public static void RunEffect(this EffectEntity self)
        {
            
        }
    }
}