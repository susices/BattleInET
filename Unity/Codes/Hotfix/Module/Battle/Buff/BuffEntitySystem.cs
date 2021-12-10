namespace ET
{
    [ObjectSystem]
    public class BuffEntityAwakeSystem: AwakeSystem<BuffEntity, Entity, int>
    {
        public override void Awake(BuffEntity self, Entity sourceEntity, int buffConfigId)
        {
            BuffConfig buffConfig = BuffConfigCategory.Instance.Get(buffConfigId);
            self.SourceEntityId = sourceEntity.Id;
            self.BuffConfigId = buffConfigId;
            self.CurrentLayer++;
            self.State = buffConfig.State;
            Log.Debug($"BuffAwaked BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
        }
    }

    [ObjectSystem]
    public class BuffEntityDestroySystem: DestroySystem<BuffEntity>
    {
        public override void Destroy(BuffEntity self)
        {
            Log.Info($"BuffEntity Destroyed BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            self.SetContainerBuffStateOnRemove();
            self.Clear();
        }
    }

    public static class BuffEntitySystem
    {

        public static void Clear(this BuffEntity self)
        {
            self.CurrentLayer = 0;
            self.SourceEntityId = 0;
            self.BuffConfigId = 0;
            self.State = BuffState.None;
        }
        
        public static void SetContainerBuffStateOnRemove(this BuffEntity self)
        {
            if (self.State == BuffState.None)
            {
                return;
            }

            var parentBuffComponent = self.GetParent<BuffComponent>();
            if (parentBuffComponent==null)
            {
                Log.Error($"buffEntity 无法获取Buff组件");
                return;
            }

            if (parentBuffComponent.BuffStateSets.ContainsKey(self.State))
            {
                parentBuffComponent.BuffStateSets[self.State] -= 1;
                if (parentBuffComponent.BuffStateSets[self.State]==0)
                {
                    parentBuffComponent.BuffStateSets.Remove(self.State);
                }
            }
            
        }
    }
}