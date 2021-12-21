namespace ET
{
    public class SpellEntityDestorySystem : DestroySystem<SpellEntity>
    {
        public override void Destroy(SpellEntity self)
        {
            self.ConfigId = 0;
            self.SpellCancellationToken = null;
        }
    }

    public static class SpellEntitySystem
    {
        
    }
}