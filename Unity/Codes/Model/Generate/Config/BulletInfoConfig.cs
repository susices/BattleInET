using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class BulletInfoConfigCategory : ProtoObject
    {
        public static BulletInfoConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, BulletInfoConfig> dict = new Dictionary<int, BulletInfoConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<BulletInfoConfig> list = new List<BulletInfoConfig>();
		
        public BulletInfoConfigCategory()
        {
            Instance = this;
        }
		
        public override void EndInit()
        {
            foreach (BulletInfoConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public BulletInfoConfig Get(int id)
        {
            this.dict.TryGetValue(id, out BulletInfoConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (BulletInfoConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, BulletInfoConfig> GetAll()
        {
            return this.dict;
        }

        public BulletInfoConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class BulletInfoConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(3)]
		public int TriggerEffect { get; set; }
		[ProtoMember(4)]
		public int BulletAIConfigId { get; set; }
		[ProtoMember(5)]
		public int BulletModelId { get; set; }

	}
}
