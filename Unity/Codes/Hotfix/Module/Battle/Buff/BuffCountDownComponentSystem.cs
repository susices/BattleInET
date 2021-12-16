namespace ET
{
    [ObjectSystem]
    public class BuffCountDownComponentAwakeSystem: AwakeSystem<BuffCountDownComponent>
    {
        public override void Awake(BuffCountDownComponent self)
        {
            self.BuffConfigId = self.GetParent<BuffEntity>().BuffConfigId;
            self.IsCountDownEnd = false;
            self.BuffCountDownCancellationToken = new ETCancellationToken();
            self.CountDown().Coroutine();
        }
    }
    
    [ObjectSystem]
    public class BuffCountDownComponentDestroySystem:DestroySystem<BuffCountDownComponent>
    {
        public override void Destroy(BuffCountDownComponent self)
        {
            if (!self.IsCountDownEnd)
            {
                self.BuffCountDownCancellationToken.Cancel();
                self.BuffCountDownCancellationToken = null;
            }
            self.BuffConfigId = 0;
        }
    }

    public static class BuffCountDownComponentSystem
    {
        public static async ETTask CountDown(this BuffCountDownComponent self)
        {
            if (await TimerComponent.Instance.WaitAsync(BuffConfigCategory.Instance.Get(self.BuffConfigId).DurationMillsecond,self.BuffCountDownCancellationToken))
            {
                self.GetParent<BuffEntity>().RunBuffTimeOutEffect();
                self.IsCountDownEnd = true;
                self.GetParent<BuffEntity>().Dispose();
            }
        }
    }
}