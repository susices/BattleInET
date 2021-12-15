using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class SpellConfigCategory : ProtoObject
    {
        public static SpellConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SpellConfig> dict = new Dictionary<int, SpellConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SpellConfig> list = new List<SpellConfig>();
		
        public SpellConfigCategory()
        {
            Instance = this;
        }
		
        public override void EndInit()
        {
            foreach (SpellConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SpellConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SpellConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SpellConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SpellConfig> GetAll()
        {
            return this.dict;
        }

        public SpellConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SpellConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(3)]
		public int[] PreCondition { get; set; }
		[ProtoMember(4)]
		public int[] Effects { get; set; }

	}
}
