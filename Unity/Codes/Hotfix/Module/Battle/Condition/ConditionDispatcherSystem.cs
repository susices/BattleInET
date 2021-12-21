using System;

namespace ET
{
    [ObjectSystem]
    public class ConditionDispatcherLoadSystem : LoadSystem<ConditionDispatcher>
    {
        public override void Load(ConditionDispatcher self)
        {
            self.Load();
        }
    }
    
    [ObjectSystem]
    public class ConditionDispatcherAwakeSystem : AwakeSystem<ConditionDispatcher>
    {
        public override void Awake(ConditionDispatcher self)
        {
            ConditionDispatcher.Instance = self;
            self.Load();
        }
    }
    
    [ObjectSystem]
    public class ConditionDispatcherDestorySystem : DestroySystem<ConditionDispatcher>
    {
        public override void Destroy(ConditionDispatcher self)
        {
            self.Conditions.Clear();
            ConditionDispatcher.Instance = null;
        }
    }
    
    public static class ConditionDispatcherSystem
    {
        public static void Load(this ConditionDispatcher self)
        {
            self.Conditions.Clear();

            var types = Game.EventSystem.GetTypes(typeof (BaseConditionAttribute));
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof (BaseConditionAttribute), false);

                if (attrs.Length == 0)
                {
                    Log.Error($"{type.Name} do not has a BaseConditionAttribute!");
                    continue;
                }
                
                BaseConditionAttribute conditionAttribute = attrs[0] as BaseConditionAttribute;
                ICondition condition = Activator.CreateInstance(type) as ICondition;
                if (condition == null)
                {
                    Log.Error($"{type.Name} is not a Condition!");
                    continue;
                }

                if (self.Conditions.ContainsKey(conditionAttribute.Id))
                {
                    Type sameIdType = self.Conditions[conditionAttribute.Id].GetType();
                    Log.Error($"{type.Name} has same id with {sameIdType.Name} : {conditionAttribute.Id.ToString()}");
                    continue;
                }

                self.Conditions.Add(conditionAttribute.Id, condition);
            }
            
            Log.Debug("ConditionDispatcherSystem Load Success");
        }

        /// <summary>
        /// 执行条件检查
        /// </summary>
        /// <returns></returns>
        public static bool CheckCondition(this ConditionDispatcher self, Entity entity, int baseConditionId, int[] args)
        {
            if (!self.Conditions.TryGetValue(baseConditionId, out ICondition baseSpellPreCondition))
            {
                Log.Error($"baseConditionId {baseConditionId.ToString()} is not exist in Conditions!");
            }

            return baseSpellPreCondition.Check(entity, args);
        }
    }
}