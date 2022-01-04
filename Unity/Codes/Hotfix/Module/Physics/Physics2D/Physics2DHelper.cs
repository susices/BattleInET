using System;

namespace ET
{
    public static class Physics2DHelper
    {
        /// <summary>
        /// AABB相交测试
        /// </summary>
        /// <returns>是否相交</returns>
        public static bool TestAABB(AABB2D aabb2DA, AABB2D aabb2DB)
        {
            if (Math.Abs(aabb2DA.CenterPoint.x-aabb2DB.CenterPoint.x)> (aabb2DA.Radius.x + aabb2DB.Radius.x))
            {
                return false;
            }
            
            if (Math.Abs(aabb2DA.CenterPoint.y-aabb2DB.CenterPoint.y)> (aabb2DA.Radius.y + aabb2DB.Radius.y))
            {
                return false;
            }

            return true;
        }
    }
}