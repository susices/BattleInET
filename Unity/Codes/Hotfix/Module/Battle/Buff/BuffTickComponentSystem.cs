using System;

namespace ET
{
    [ObjectSystem]
    public class BuffTickComponentAwakeSystem:AwakeSystem<BuffTickComponent>
    {
        public override void Awake(BuffTickComponent self)
        {
            self.BuffConfigId = self.GetParent<BuffEntity>().BuffConfigId;
            self.BuffTickTimeSpan = BuffConfigCategory.Instance.Get(self.BuffConfigId).BuffTickTimeSpan;
            self.BuffTickTimerId = TimerComponent.Instance.NewRepeatedTimer(self.BuffTickTimeSpan, TimerType.BuffTickTimer, self);
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
        }
    }

    
    public static class BuffTickComponentSystem
    {
        public static void Tick(this BuffTickComponent self)
        {
            int[] effectIds = BuffConfigCategory.Instance.Get(self.BuffConfigId).BuffTickActions;
            for (int i = 0; i < effectIds.Length; i++)
            {
                
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