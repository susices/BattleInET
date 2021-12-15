using System;

namespace ET
{
    
    
    [ObjectSystem]
    public class SpellPreConditionDispatcherLoadSystem : LoadSystem<SpellPreConditionDispatcher>
    {
        public override void Load(SpellPreConditionDispatcher self)
        {
            
        }
    }
    
    [ObjectSystem]
    public class SpellPreConditionDispatcherAwakeSystem : AwakeSystem<SpellPreConditionDispatcher>
    {
        public override void Awake(SpellPreConditionDispatcher self)
        {
            
        }
    }
    
    [ObjectSystem]
    public class SpellPreConditionDispatcherDestorySystem : DestroySystem<SpellPreConditionDispatcher>
    {
        public override void Destroy(SpellPreConditionDispatcher self)
        {
            
        }
    }
    
    public static class SpellPreConditionDispatcherSystem
    {
        public static void Load(this SpellPreConditionDispatcher self)
        {
            self.SpellPreConditions.Clear();

            var types = Game.EventSystem.GetTypes(typeof (BaseSpellPreConditionAttribute));
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof (BaseSpellPreConditionAttribute), false);

                if (attrs.Length == 0)
                {
                    Log.Error($"{type.Name} do not has a BaseSpellPreConditionAttribute!");
                    continue;
                }
                
                BaseSpellPreConditionAttribute spellPreConditionAttribute = attrs[0] as BaseSpellPreConditionAttribute;
                ISpellPreCondition spellPreCondition = Activator.CreateInstance(type) as ISpellPreCondition;
                if (spellPreCondition == null)
                {
                    Log.Error($"{type.Name} is not a spellPreCondition!");
                    continue;
                }

                if (self.SpellPreConditions.ContainsKey(spellPreConditionAttribute.Id))
                {
                    Type sameIdType = self.SpellPreConditions[spellPreConditionAttribute.Id].GetType();
                    Log.Error($"{type.Name} has same id with {sameIdType.Name} : {spellPreConditionAttribute.Id.ToString()}");
                    continue;
                }

                self.SpellPreConditions.Add(spellPreConditionAttribute.Id, spellPreCondition);
            }
            
            Log.Debug("SpellPreConditionDispatcherSystem Load Success");
        }

        /// <summary>
        /// 执行技能前提条件检查
        /// </summary>
        /// <param name="self"></param>
        /// <param name="spellComponent"></param>
        /// <returns></returns>
        public static bool CheckSpellPreCondition(this SpellPreConditionDispatcher self, SpellComponent spellComponent, int baseSpellPreConditionId, int[] args)
        {
            if (!self.SpellPreConditions.TryGetValue(baseSpellPreConditionId, out ISpellPreCondition baseSpellPreCondition))
            {
                Log.Error($"baseSpellPreConditionId {baseSpellPreConditionId.ToString()} is not exist in SpellPreConditions!");
            }

            return baseSpellPreCondition.Check(spellComponent, args);
        }
    }
}