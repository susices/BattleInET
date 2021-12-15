using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class SpellBaseConditionConfigCategory : ProtoObject
    {
        public static SpellBaseConditionConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SpellBaseConditionConfig> dict = new Dictionary<int, SpellBaseConditionConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SpellBaseConditionConfig> list = new List<SpellBaseConditionConfig>();
		
        public SpellBaseConditionConfigCategory()
        {
            Instance = this;
        }
		
        public override void EndInit()
        {
            foreach (SpellBaseConditionConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SpellBaseConditionConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SpellBaseConditionConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SpellBaseConditionConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SpellBaseConditionConfig> GetAll()
        {
            return this.dict;
        }

        public SpellBaseConditionConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SpellBaseConditionConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }

	}
}
