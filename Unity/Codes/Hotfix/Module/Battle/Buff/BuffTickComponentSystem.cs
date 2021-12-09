using System;

namespace ET
{
    [ObjectSystem]
    public class BuffTickComponentAwakeSystem:AwakeSystem<BuffTickComponent>
    {
        public override void Awake(BuffTickComponent self)
        {
            self.ParentBuffEntity = self.GetParent<BuffEntity>();
            self.BuffConfigId = self.ParentBuffEntity.BuffConfigId;
            self.BuffTickTimeSpan = BuffConfigCategory.Instance.Get(self.BuffConfigId).BuffTickTimeSpan;
            self.TickBuffActions = ListComponent<IBuffAction>.Create();
            self.TickBuffActionsArgs = ListComponent<int[]>.Create();
            if (!BuffActionDispatcher.Instance.GetBuffTickActions(self.GetParent<BuffEntity>(), self.TickBuffActions, self.TickBuffActionsArgs))
            {
                self.Dispose();
            }
            self.StartTick();
        }
    }

    [ObjectSystem]
    public class BuffTickComponentDestroySystem:DestroySystem<BuffTickComponent>
    {
        public override void Destroy(BuffTickComponent self)
        {
            TimerComponent.Instance.Remove(ref self.BuffTickTimerId);
            self.BuffTickTimerId = 0;
            self.BuffTickTimeSpan = 0;
            self.TickBuffActions.Dispose();
            self.TickBuffActions = null;
            self.TickBuffActionsArgs.Dispose();
            self.TickBuffActionsArgs = null;
        }
    }

    
    public static class BuffTickComponentSystem
    {
        public static void StartTick(this BuffTickComponent self)
        {
            self.Tick();
            self.BuffTickTimerId = TimerComponent.Instance.NewRepeatedTimer(self.BuffTickTimeSpan, TimerType.BuffTickTimer, self);
        }
        
        public static void Tick(this BuffTickComponent self)
        {
            for (int i = 0; i < self.TickBuffActions.Count; i++)
            {
                self.TickBuffActions[i].Run(self.ParentBuffEntity, self.TickBuffActionsArgs[i]);
            }
            Log.Debug($"BuffTicked BuffConfigId: {self.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
        }
    }
    
    [Timer(TimerType.BuffTickTimer)]
    public class BuffTickTimer : ATimer<BuffTickComponent>
    {
        public override void Run(BuffTickComponent self)
        {
            try
            {
                self.Tick();
            }
            catch (Exception e)
            {
                Log.Error($"BuffTickTimer error: {e}");
            }
        }
    }
}