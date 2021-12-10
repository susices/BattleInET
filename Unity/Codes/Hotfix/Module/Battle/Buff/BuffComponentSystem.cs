﻿using System.Collections.Generic;

namespace ET
{
    [ObjectSystem]
    public class BuffComponentAwakeSystem: AwakeSystem<BuffComponent>
    {
        public override void Awake(BuffComponent self)
        {
            self.BuffStateSets.Clear();
        }
    }

    [ObjectSystem]
    public class BuffComponentDestroySystem: DestroySystem<BuffComponent>
    {
        public override void Destroy(BuffComponent self)
        {
            self.BuffStateSets.Clear();
        }
    }

    /// <summary>
    /// Buff组件系统
    /// </summary>
    public static class BuffComponentSystem
    {
        /// <summary>
        /// 添加Buff到Buff容器
        /// </summary>
        public static bool TryAddBuff(this BuffComponent self, int buffConfigId, Entity sourceEntity)
        {
            //检测是否存在冲突状态
            if (CheckBuffConflict(self, buffConfigId, sourceEntity))
            {
                return false;
            }

            using (var buffEntityList = ListComponent<BuffEntity>.Create())
            {
                //检测是否含有该种Buff
                if (!GetBuffByConfigId(self, buffConfigId, buffEntityList))
                {
                    AddBuff(self, buffConfigId, sourceEntity);
                    return true;
                }

                // 判断是否存在同一来源 buff
                BuffConfig buffConfig = BuffConfigCategory.Instance.Get(buffConfigId);
                BuffEntity sameSourceBuffEntity = null;
                foreach (BuffEntity buffEntity in buffEntityList)
                {
                    if (buffEntity.SourceEntityId == sourceEntity.Id)
                    {
                        sameSourceBuffEntity = buffEntity;
                        break;
                    }
                }
                //检测是否可以刷新该同来源Buff
                if (sameSourceBuffEntity != null)
                {
                    if (!buffConfig.IsEnableRefresh)
                    {
                        return false;
                    }
                    if (sameSourceBuffEntity.CurrentLayer < buffConfig.MaxLayerCount)
                    {
                        sameSourceBuffEntity.CurrentLayer++;
                    }
                    BuffActionDispatcher.Instance.RunBuffRefreshAction(sameSourceBuffEntity);
                    return true;
                }
            
                //检测是否可以添加新Buff
                if (buffEntityList.Count < buffConfig.MaxSourceCount)
                {
                    AddBuff(self, buffConfigId, sourceEntity);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 从Buff组件移除Buff
        /// </summary>
        public static bool TryRemoveBuff(this BuffComponent self, long buffEntityId)
        {
            BuffEntity buffEntity = self.GetChild<BuffEntity>(buffEntityId);
            if (buffEntity == null)
            {
                return false;
            }
            BuffActionDispatcher.Instance.RunBuffRemoveAction(buffEntity);
            Log.Debug($"BuffRemoved BuffConfigId: {buffEntity.BuffConfigId.ToString()}  BuffEntityId: {self.Id.ToString()}");
            buffEntity.Dispose();
            return true;
        }

        private static void AddBuff(this BuffComponent self, int buffConfigId, Entity sourceEntity)
        {
            var buffEntity =  BuffFactory.Create(self, sourceEntity, buffConfigId);
            if (buffEntity ==null)
            {
                return;
            }

            self.BuffStateSets[buffEntity.State] += 1;
        }

        /// <summary>
        /// 检查Buff是否与容器Buff冲突
        /// </summary>
        /// <returns></returns>
        private static bool CheckBuffConflict(this BuffComponent self, int buffConfigId, Entity sourceEntity)
        {
            BuffConfig buffConfig = BuffConfigCategory.Instance.Get(buffConfigId);
            // 检测BuffState 冲突
            if (!BuffStateConfigCategory.Instance.Contain(buffConfig.State))
            {
                return false;
            }
            int[] conflictStates = BuffStateConfigCategory.Instance.Get(buffConfig.State).ConflictStates;
            if (conflictStates==null)
            {
                return false;
            }
            foreach (int conflictState in conflictStates)
            {
                if (self.BuffStateSets.ContainsKey(conflictState))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查是否存在指定configId的Buff  并返回符合的buffEntity列表
        /// </summary>
        public static bool GetBuffByConfigId(this BuffComponent self, int buffConfigId, List<BuffEntity> buffEntities)
        {
            
            bool value = false;
            foreach (var child in self.Children)
            {
                if (child.Value is BuffEntity buffEntity && buffEntity.BuffConfigId == buffConfigId)
                {
                    buffEntities.Add(buffEntity);
                    value = true;
                }
            }
            return value;
        }
        
        public static void RemoveBuffs(this BuffComponent self, List<BuffEntity> buffEntities)
        {
            for (int i = 0; i < buffEntities.Count; i++)
            {
                if (!self.TryRemoveBuff(buffEntities[i].Id))
                {
                    Log.Info($"Remove Buff failed buffConfigId:{buffEntities[i].BuffConfigId.ToString()} buffEntityId:{buffEntities[i].Id.ToString()}");
                }
            }
        }
        
        /// <summary>
        /// 移除指定buff配置Id的所有buff实体
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffConfigId"></param>
        /// <returns></returns>
        public static bool TryRemoveBuff(this BuffComponent self, int buffConfigId)
        {
            using (var buffEntityList = ListComponent<BuffEntity>.Create())
            {
                if (self.GetBuffByConfigId(buffConfigId, buffEntityList))
                {
                    self.RemoveBuffs(buffEntityList);
                    return true;
                }
                return false;
            }
        }
    }
}