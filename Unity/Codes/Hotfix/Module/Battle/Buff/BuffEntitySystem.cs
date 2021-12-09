namespace ET
{
    [ObjectSystem]
    public class BuffEntityAwakeSystem: AwakeSystem<BuffEntity, Entity, int>
    {
        public override void Awake(BuffEntity self, Entity sourceEntity, int buffConfigId)
        {
            BuffConfig buffConfig = BuffConfigCategory.Instance.Get(buffConfigId);
            self.SourceEntity = sourceEntity;
            self.BuffComponent = self.Parent as BuffComponent;
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
            self.SourceEntity = null;
            self.BuffConfigId = 0;
            self.BuffComponent = null;
            self.State = BuffState.None;
        }
        
        public static void SetContainerBuffStateOnRemove(this BuffEntity self)
        {
            if (self.State == BuffState.None)
            {
                return;
            }

            if (self.BuffComponent.BuffStateSets.ContainsKey(self.State))
            {
                self.BuffComponent.BuffStateSets[self.State] -= 1;
                if (self.BuffComponent.BuffStateSets[self.State]==0)
                {
                    self.BuffComponent.BuffStateSets.Remove(self.State);
                }
            }
            
        }
    }
}