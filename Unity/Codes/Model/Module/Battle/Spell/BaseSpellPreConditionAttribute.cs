namespace ET
{
    public class BaseSpellPreConditionAttribute : BaseAttribute
    {
        public int Id;

        public BaseSpellPreConditionAttribute(int BaseSpellPreConditionId)
        {
            this.Id = BaseSpellPreConditionId;
        }
    }
}