namespace ET
{
    public static class SpellFactory
    {
        public static SpellEntity Create(SpellComponent spellComponent, int spellConfigId)
        {
            var spellEntity = spellComponent.AddChild<SpellEntity>();
            spellEntity.ConfigId = spellConfigId;
            if (spellEntity.SpellConfig.CanInterrpted)
            {
                spellEntity.SpellCancellationToken = new ETCancellationToken();
            }
            return spellEntity;
        }
    }
}