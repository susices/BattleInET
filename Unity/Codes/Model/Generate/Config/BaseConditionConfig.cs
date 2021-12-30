using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class BaseConditionConfigCategory : ProtoObject
    {
        public static BaseConditionConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, BaseConditionConfig> dict = new Dictionary<int, BaseConditionConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<BaseConditionConfig> list = new List<BaseConditionConfig>();
		
        public BaseConditionConfigCategory()
        {
            Instance = this;
        }
		
        public override void EndInit()
        {
            foreach (BaseConditionConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public BaseConditionConfig Get(int id)
        {
            this.dict.TryGetValue(id, out BaseConditionConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (BaseConditionConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, BaseConditionConfig> GetAll()
        {
            return this.dict;
        }

        public BaseConditionConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class BaseConditionConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }

	}
}
