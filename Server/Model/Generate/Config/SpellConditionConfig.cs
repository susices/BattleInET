using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class SpellConditionConfigCategory : ProtoObject
    {
        public static SpellConditionConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SpellConditionConfig> dict = new Dictionary<int, SpellConditionConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SpellConditionConfig> list = new List<SpellConditionConfig>();
		
        public SpellConditionConfigCategory()
        {
            Instance = this;
        }
		
        public override void EndInit()
        {
            foreach (SpellConditionConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SpellConditionConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SpellConditionConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SpellConditionConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SpellConditionConfig> GetAll()
        {
            return this.dict;
        }

        public SpellConditionConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SpellConditionConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(3)]
		public int BaseConditionId { get; set; }
		[ProtoMember(4)]
		public int[] ConditionArgs { get; set; }
		[ProtoMember(5)]
		public bool IsInvert { get; set; }

	}
}
