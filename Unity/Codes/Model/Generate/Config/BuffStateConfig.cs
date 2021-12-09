using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class BuffStateConfigCategory : ProtoObject
    {
        public static BuffStateConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, BuffStateConfig> dict = new Dictionary<int, BuffStateConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<BuffStateConfig> list = new List<BuffStateConfig>();
		
        public BuffStateConfigCategory()
        {
            Instance = this;
        }
		
        public override void EndInit()
        {
            foreach (BuffStateConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public BuffStateConfig Get(int id)
        {
            this.dict.TryGetValue(id, out BuffStateConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (BuffStateConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, BuffStateConfig> GetAll()
        {
            return this.dict;
        }

        public BuffStateConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class BuffStateConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(3)]
		public int[] ConflictStates { get; set; }

	}
}
