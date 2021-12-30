namespace ET
{
    public class EffectEntity : Entity
    {
        /// <summary>
        /// 效果id
        /// </summary>
        public int EffectConfigId;

        /// <summary>
        /// 效果配置
        /// </summary>
        public EffectConfig EffectConfig
        {
            get
            {
                return EffectConfigCategory.Instance.Get(this.EffectConfigId);
            }
        }
        
    }
}