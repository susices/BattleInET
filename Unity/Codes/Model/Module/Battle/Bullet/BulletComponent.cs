namespace ET
{
    public class BulletComponent:Entity
    {
        public int BulletInfoConfigId;

        public BulletInfoConfig Config
        {
            get
            {
                return BulletInfoConfigCategory.Instance.Get(BulletInfoConfigId);
            }
        }
    }
}