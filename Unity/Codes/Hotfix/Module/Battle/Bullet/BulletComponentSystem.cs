namespace ET
{
    [ObjectSystem]
    public class BulletComponentAwakeSystem:AwakeSystem<BulletComponent,int>
    {
        public override void Awake(BulletComponent self, int configId)
        {
            self.BulletInfoConfigId = configId;
        }
    }

    public class BulletComponentSystem
    {
        
    }
}