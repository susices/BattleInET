namespace ET
{
    /// <summary>
    /// Effect接口  
    /// </summary>
    public interface IEffect
    {
        /// <summary>
        /// 执行Effect
        /// </summary>
        void Run(EffectEntity entity, int[] args);
    }
}