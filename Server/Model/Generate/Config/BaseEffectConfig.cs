using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class BaseEffectConfigCategory : ProtoObject
    {
        public static BaseEffectConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, BaseEffectConfig> dict = new Dictionary<int, BaseEffectConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<BaseEffectConfig> list = new List<BaseEffectConfig>();
		
        public BaseEffectConfigCategory()
        {
            Instance = this;
        }
		
        public override void EndInit()
        {
            foreach (BaseEffectConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public BaseEffectConfig Get(int id)
        {
            this.dict.TryGetValue(id, out BaseEffectConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (BaseEffectConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, BaseEffectConfig> GetAll()
        {
            return this.dict;
        }

        public BaseEffectConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class BaseEffectConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }

	}
}
