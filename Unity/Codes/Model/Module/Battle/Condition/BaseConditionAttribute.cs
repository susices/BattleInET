namespace ET
{
    public class BaseConditionAttribute : BaseAttribute
    {
        public int Id;

        public BaseConditionAttribute(int BaseSpellPreConditionId)
        {
            this.Id = BaseSpellPreConditionId;
        }
    }
}