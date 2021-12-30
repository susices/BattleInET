namespace ET
{
    public static class BuffHelper
    {
        /// <summary>
        /// 添加Buff
        /// </summary>
        public static bool AddBuff(int buffConfigId, Entity sourceEntity, Entity targetEntity)
        {
            if (targetEntity.GetComponent<BuffComponent>()==null)
            {
                return false;
            }
            return targetEntity.GetComponent<BuffComponent>().TryAddBuff(buffConfigId, sourceEntity);
        }
    }
}