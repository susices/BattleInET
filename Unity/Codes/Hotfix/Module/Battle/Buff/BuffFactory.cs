using System.Runtime.CompilerServices;

namespace ET
{
    /// <summary>
    /// Buff工厂  构造Buff实体
    /// </summary>
    public static class BuffFactory
    {
        public static BuffEntity Create(BuffComponent buffComponent,Entity sourceEntity, int buffConfigId)
        {
            var buffEntity = buffComponent.AddChild<BuffEntity, Entity, int>(sourceEntity, buffConfigId, true);
            if (buffEntity.IsDisposed)
            {
                return null;
            }

            BuffConfig buffConfig = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId);

            if (buffConfig.BuffTickActions == null || buffConfig.BuffTickTimeSpan<=0)
            {
                return buffEntity;
            }
            
            buffEntity.AddComponent<BuffTickComponent>();
            
            if (buffConfig.DurationMillsecond>0)
            {
                buffEntity.AddComponent<BuffCountDownComponent>();
            }
            
            return buffEntity;
        }
    }
}