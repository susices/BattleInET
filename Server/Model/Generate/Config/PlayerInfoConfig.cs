using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class PlayerInfoConfigCategory : ProtoObject
    {
        public static PlayerInfoConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PlayerInfoConfig> dict = new Dictionary<int, PlayerInfoConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PlayerInfoConfig> list = new List<PlayerInfoConfig>();
		
        public PlayerInfoConfigCategory()
        {
            Instance = this;
        }
		
        public override void EndInit()
        {
            foreach (PlayerInfoConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PlayerInfoConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PlayerInfoConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PlayerInfoConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PlayerInfoConfig> GetAll()
        {
            return this.dict;
        }

        public PlayerInfoConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PlayerInfoConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(3)]
		public int[] BornPos { get; set; }

	}
}
