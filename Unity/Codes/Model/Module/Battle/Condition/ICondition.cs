namespace ET
{
    /// <summary>
    /// 条件接口
    /// </summary>
    public interface ICondition
    {
        bool Check(Entity entity, int[] args);
    }
}