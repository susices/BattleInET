using UnityEngine;

namespace ET
{
    public static class UnitFactory
    {
        public static Unit Create(Entity domain, UnitInfo unitInfo)
        {
	        UnitComponent unitComponent = domain.GetComponent<UnitComponent>();
	        Unit unit = unitComponent.AddChildWithId<Unit, int>(unitInfo.UnitId, unitInfo.ConfigId);
	        unitComponent.Add(unit);
	        
	        unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);
	        unit.Forward = new Vector3(unitInfo.ForwardX, unitInfo.ForwardY, unitInfo.ForwardZ);
	        
	        NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
	        for (int i = 0; i < unitInfo.Ks.Count; ++i)
	        {
		        numericComponent.Set((NumericType)unitInfo.Ks[i], unitInfo.Vs[i]);
	        }
	        
	        unit.AddComponent<MoveComponent>();
	        if (unitInfo.MoveInfo != null)
	        {
		        if (unitInfo.MoveInfo.X.Count > 0)
		        {
			        using (ListComponent<Vector3> list = ListComponent<Vector3>.Create())
			        {
				        list.Add(unit.Position);
				        for (int i = 0; i < unitInfo.MoveInfo.X.Count; ++i)
				        {
					        list.Add(new Vector3(unitInfo.MoveInfo.X[i], unitInfo.MoveInfo.Y[i], unitInfo.MoveInfo.Z[i]));
				        }

				        unit.MoveToAsync(list).Coroutine();
			        }
		        }
	        }

	        unit.AddComponent<ObjectWait>();

	        switch ((UnitType)unitInfo.Type)
	        {
		        case UnitType.Player:
			        HandlePlayerUnit(domain, unitInfo, unit);
			        break;
		        case UnitType.Bullect:
			        HandleBulletUnit(domain, unitInfo, unit);
			        break;
	        }
	        
	        Game.EventSystem.Publish(new EventType.AfterUnitCreate() {Unit = unit});
            return unit;
        }

        private static void HandlePlayerUnit(Entity domain, UnitInfo unitInfo, Unit unit)
        {
	        unit.AddComponent<XunLuoPathComponent>();
        }
        
        private static void HandleBulletUnit(Entity domain, UnitInfo unitInfo, Unit unit)
        {
	        var bulletInfo = BulletInfoConfigCategory.Instance.Get(unit.Config.TypeConfigId);
	        if (bulletInfo==null)
	        {
		        Log.Error($"找不到bulletInfo  bulletInfoId: {unit.Config.TypeConfigId.ToString()}");
		        return;
	        }
	        
	        unit.AddComponent<BulletComponent, int>(unit.Config.TypeConfigId);
        }
    }
}
