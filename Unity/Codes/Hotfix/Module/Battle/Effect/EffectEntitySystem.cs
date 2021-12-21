namespace ET
{
    
    
    [ObjectSystem]
    public class EffectEntityDestorySystem : DestroySystem<EffectEntity>
    {
        public override void Destroy(EffectEntity self)
        {
            self.EffectConfigId = 0;
        }
    }

    public static class EffectEntitySystem
    {
        
    }
}