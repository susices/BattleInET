namespace ET
{
    /// <summary>
    /// SpellPreCondition接口
    /// </summary>
    public interface ISpellPreCondition
    {
        bool Check(SpellComponent spellComponent, int[] args);
    }
}