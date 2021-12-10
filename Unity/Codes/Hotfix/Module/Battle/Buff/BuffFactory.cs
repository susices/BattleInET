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
            BuffActionDispatcher.Instance.RunBuffAddAction(buffEntity);
            if (buffEntity.IsDisposed)
            {
                return null;
            }
            BuffActionDispatcher.Instance.AddBuffTickComponent(buffEntity);
            if (BuffConfigCategory.Instance.Get(buffConfigId).DurationMillsecond>0)
            {
                buffEntity.AddComponent<BuffCountDownComponent>();
            }
            return buffEntity;
        }
    }
}