using System;
using System.Collections.Generic;

namespace ET
{
    [ObjectSystem]
    public class EffectDispatcherLoadSystem: LoadSystem<EffectDispatcher>
    {
        public override void Load(EffectDispatcher self)
        {
            self.Load();
        }
    }

    [ObjectSystem]
    public class EffectDispatcherAwakeSystem: AwakeSystem<EffectDispatcher>
    {
        public override void Awake(EffectDispatcher self)
        {
            EffectDispatcher.Instance = self;
            self.Load();
        }
    }

    [ObjectSystem]
    public class EffectDispatcherDestroySystem: DestroySystem<EffectDispatcher>
    {
        public override void Destroy(EffectDispatcher self)
        {
            self.Effects.Clear();
            EffectDispatcher.Instance = null;
        }
    }

    public static class EffectDispatcherSystem
    {
        public static void Load(this EffectDispatcher self)
        {
            self.Effects.Clear();

            var types = Game.EventSystem.GetTypes(typeof (BaseEffectAttribute));
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof (BaseEffectAttribute), false);

                if (attrs.Length == 0)
                {
                    Log.Error($"{type.Name} do not has a BaseEffectAttribute!");
                    continue;
                }

                BaseEffectAttribute effectAttribute = attrs[0] as BaseEffectAttribute;
                IEffect effect = Activator.CreateInstance(type) as IEffect;
                if (effect == null)
                {
                    Log.Error($"{type.Name} is not a BuffEffect!");
                    continue;
                }

                if (self.Effects.ContainsKey(effectAttribute.Id))
                {
                    Type sameIdType = self.Effects[effectAttribute.Id].GetType();
                    Log.Error($"{type.Name} has same id with {sameIdType.Name} : {effectAttribute.Id.ToString()}");
                    continue;
                }

                self.Effects.Add(effectAttribute.Id, effect);
            }
            
            Log.Debug("EffectDispatcherSystem Load Success");
        }

        /// <summary>
        /// 运行BaseEffect方法
        /// </summary>
        public static void RunBaseEffect(this EffectDispatcher self, EffectEntity effectEntity, int baseEffectId, int[] args)
        {
            if (!self.Effects.TryGetValue(baseEffectId, out IEffect baseBuffAction))
            {
                Log.Error($"baseEffectId {baseEffectId.ToString()} is not exist in buffActions!");
                return;
            }
            

            baseBuffAction.Run(effectEntity, args);
        }

        /// <summary>
        /// 运行Effect方法
        /// </summary>
        public static bool RunEffects(this EffectDispatcher self, EffectEntity effectEntity, int[] effectIds)
        {
            if (effectIds==null)
            {
                return false;
            }
            
            foreach (int effectId in effectIds)
            {
                EffectConfig effectConfig = EffectConfigCategory.Instance.Get(effectId);
                int baseEffectId = effectConfig.BaseEffectId;
                int[] args = effectConfig.EffectArgs;
                self.RunBaseEffect(effectEntity, baseEffectId, args);
            }
            return true;
        }

        /// <summary>
        /// Buff添加后执行
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void RunBuffAddAction(this EffectDispatcher self, BuffEntity buffEntity)
        {
            int[] buffAddActionIds = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffAddActions;
            if (self.RunEffects(buffEntity, buffAddActionIds))
            {
                Log.Debug($"BuffAdded BuffConfigId: {buffEntity.BuffConfigId.ToString()}  BuffEntityId: {buffEntity.Id.ToString()}");
            }
        }

        /// <summary>
        /// Buff移除前执行
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void RunBuffRemoveAction(this EffectDispatcher self, BuffEntity buffEntity)
        {
            int[] buffRemoveActionIds = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffRemoveActions;
            if (self.RunEffects(buffEntity, buffRemoveActionIds))
            {
                Log.Debug($"BuffRemoveed BuffConfigId: {buffEntity.BuffConfigId.ToString()}  BuffEntityId: {buffEntity.Id.ToString()}");
            }
        }

        /// <summary>
        /// Buff刷新时执行
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void RunBuffRefreshAction(this EffectDispatcher self, BuffEntity buffEntity)
        {
            
            int[] buffRefreshActionIds = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffRefreshActions;
            if (self.RunEffects(buffEntity, buffRefreshActionIds))
            {
                Log.Debug($"BuffRefreshed BuffConfigId: {buffEntity.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            }
        }

        /// <summary>
        /// 添加BuffTick组件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void AddBuffTickComponent(this EffectDispatcher self, BuffEntity buffEntity)
        {
            if (BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffTickActions == null)
            {
                return;
            }                                          

            if (BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffTickTimeSpan<=0)
            {
                Log.Error($"BuffConfig Tick间隔时间配置错误！ BuffConfigId: {buffEntity.BuffConfigId.ToString()}");
                return;
            }
            buffEntity.AddComponent<BuffTickComponent>();
        }

        public static void RunBuffTickAction(this EffectDispatcher self, BuffEntity buffEntity)
        {
            int[] buffTickActions = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffTickActions;
            if (self.RunEffects(buffEntity, buffTickActions))
            {
                Log.Debug($"BuffTicked BuffConfigId: {buffEntity.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            }
        }

        /// <summary>
        /// Buff时间截至时执行
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void RunBuffTimeOutAction(this EffectDispatcher self, BuffEntity buffEntity)
        {
            int[] buffTimeOutActionIds = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffTimeOutActions;
            if (self.RunEffects(buffEntity, buffTimeOutActionIds))
            {
                Log.Debug($"BuffTimeOut BuffConfigId: {buffEntity.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            }
        }
    }
}