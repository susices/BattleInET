namespace ET
{
    public class SpellEntity : Entity
    {
        public int ConfigId;

        public SpellConfig SpellConfig
        {
            get
            {
                return SpellConfigCategory.Instance.Get(this.ConfigId);
            }
        }

        public ETCancellationToken SpellCancellationToken;
    }
}