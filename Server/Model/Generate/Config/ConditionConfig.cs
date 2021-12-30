using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ConditionConfigCategory : ProtoObject
    {
        public static ConditionConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ConditionConfig> dict = new Dictionary<int, ConditionConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ConditionConfig> list = new List<ConditionConfig>();
		
        public ConditionConfigCategory()
        {
            Instance = this;
        }
		
        public override void EndInit()
        {
            foreach (ConditionConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ConditionConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ConditionConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ConditionConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ConditionConfig> GetAll()
        {
            return this.dict;
        }

        public ConditionConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ConditionConfig: ProtoObject, IConfig
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
