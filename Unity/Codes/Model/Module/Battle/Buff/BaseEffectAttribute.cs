using System;

namespace ET
{
    
    public class BaseEffectAttribute: BaseAttribute
    {
        public int Id;
        public BaseEffectAttribute(int BaseBuffActionId)
        {
            this.Id = BaseBuffActionId;
        }
    }
}