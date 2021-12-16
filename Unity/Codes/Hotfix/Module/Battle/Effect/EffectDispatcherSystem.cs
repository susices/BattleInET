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
    }
}