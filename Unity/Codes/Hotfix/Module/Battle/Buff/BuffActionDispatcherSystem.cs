using System;
using System.Collections.Generic;

namespace ET
{
    [ObjectSystem]
    public class BuffActionDispatcherLoadSystem: LoadSystem<BuffActionDispatcher>
    {
        public override void Load(BuffActionDispatcher self)
        {
            self.Load();
        }
    }

    [ObjectSystem]
    public class BuffActionDispatcherAwakeSystem: AwakeSystem<BuffActionDispatcher>
    {
        public override void Awake(BuffActionDispatcher self)
        {
            BuffActionDispatcher.Instance = self;
            self.Load();
        }
    }

    [ObjectSystem]
    public class BuffActionDispatcherDestroySystem: DestroySystem<BuffActionDispatcher>
    {
        public override void Destroy(BuffActionDispatcher self)
        {
            self.BuffActions.Clear();
            BuffActionDispatcher.Instance = null;
        }
    }

    public static class BuffActionDispatcherSystem
    {
        public static void Load(this BuffActionDispatcher self)
        {
            self.BuffActions.Clear();

            var types = Game.EventSystem.GetTypes(typeof (BaseBuffActionAttribute));
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof (BaseBuffActionAttribute), false);

                if (attrs.Length == 0)
                {
                    Log.Error($"{type.Name} do not has a BaseBuffActionAttribute!");
                    continue;
                }

                BaseBuffActionAttribute buffActionAttribute = attrs[0] as BaseBuffActionAttribute;
                IBuffAction buffAction = Activator.CreateInstance(type) as IBuffAction;
                if (buffAction == null)
                {
                    Log.Error($"{type.Name} is not a BuffAction!");
                    continue;
                }

                if (self.BuffActions.ContainsKey(buffActionAttribute.Id))
                {
                    Type sameIdType = self.BuffActions[buffActionAttribute.Id].GetType();
                    Log.Error($"{type.Name} has same id with {sameIdType.Name} : {buffActionAttribute.Id.ToString()}");
                    continue;
                }

                self.BuffActions.Add(buffActionAttribute.Id, buffAction);
            }
            
            Log.Debug("BuffActionDispatcherSystem Load Success");
        }

        /// <summary>
        /// 运行BuffAction方法
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        /// <param name="baseBuffActionId"></param>
        /// <param name="args"></param>
        public static void RunBuffAction(this BuffActionDispatcher self, BuffEntity buffEntity, int baseBuffActionId, int[] args)
        {
            if (!self.BuffActions.TryGetValue(baseBuffActionId, out IBuffAction baseBuffAction))
            {
                Log.Error($"buffActionId {baseBuffActionId.ToString()} is not exist in buffActions!");
                return;
            }
            

            baseBuffAction.Run(buffEntity, args);
        }

        /// <summary>
        /// 运行BuffAction方法
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        /// <param name="buffActionIds"></param>
        public static bool RunBuffActions(this BuffActionDispatcher self, BuffEntity buffEntity, int[] buffActionIds)
        {
            if (buffActionIds==null)
            {
                return false;
            }
            
            foreach (int buffActionId in buffActionIds)
            {
                BuffActionConfig buffActionConfig = BuffActionConfigCategory.Instance.Get(buffActionId);
                int baseBuffActionId = buffActionConfig.BaseActionId;
                int[] args = buffActionConfig.actionArgs;
                self.RunBuffAction(buffEntity, baseBuffActionId, args);
            }

            return true;

        }

        /// <summary>
        /// Buff添加后执行
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void RunBuffAddAction(this BuffActionDispatcher self, BuffEntity buffEntity)
        {
            int[] buffAddActionIds = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffAddActions;
            if (self.RunBuffActions(buffEntity, buffAddActionIds))
            {
                Log.Debug($"BuffAdded BuffConfigId: {buffEntity.BuffConfigId.ToString()}  BuffEntityId: {buffEntity.Id.ToString()}");
            }
        }

        /// <summary>
        /// Buff移除前执行
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void RunBuffRemoveAction(this BuffActionDispatcher self, BuffEntity buffEntity)
        {
            int[] buffRemoveActionIds = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffRemoveActions;
            if (self.RunBuffActions(buffEntity, buffRemoveActionIds))
            {
                Log.Debug($"BuffRemoveed BuffConfigId: {buffEntity.BuffConfigId.ToString()}  BuffEntityId: {buffEntity.Id.ToString()}");
            }
        }

        /// <summary>
        /// Buff刷新时执行
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void RunBuffRefreshAction(this BuffActionDispatcher self, BuffEntity buffEntity)
        {
            
            int[] buffRefreshActionIds = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffRefreshActions;
            if (self.RunBuffActions(buffEntity, buffRefreshActionIds))
            {
                Log.Debug($"BuffRefreshed BuffConfigId: {buffEntity.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            }
        }

        /// <summary>
        /// 添加BuffTick组件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void AddBuffTickComponent(this BuffActionDispatcher self, BuffEntity buffEntity)
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

        public static void RunBuffTickAction(this BuffActionDispatcher self, BuffEntity buffEntity)
        {
            int[] buffTickActions = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffTickActions;
            if (self.RunBuffActions(buffEntity, buffTickActions))
            {
                Log.Debug($"BuffTicked BuffConfigId: {buffEntity.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            }
        }

        /// <summary>
        /// Buff时间截至时执行
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEntity"></param>
        public static void RunBuffTimeOutAction(this BuffActionDispatcher self, BuffEntity buffEntity)
        {
            int[] buffTimeOutActionIds = BuffConfigCategory.Instance.Get(buffEntity.BuffConfigId).BuffTimeOutActions;
            if (self.RunBuffActions(buffEntity, buffTimeOutActionIds))
            {
                Log.Debug($"BuffTimeOut BuffConfigId: {buffEntity.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            }
        }
    }
}