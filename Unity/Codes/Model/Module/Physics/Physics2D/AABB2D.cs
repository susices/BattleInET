using UnityEngine;

namespace ET
{
    /// <summary>
    /// AABB包围盒 2D版
    /// </summary>
    public class AABB2D : Entity
    {
        /// <summary>
        /// 包围盒中心点坐标
        /// </summary>
        public Vector2 CenterPoint;

        /// <summary>
        /// 包围盒 X轴 Z轴 半径
        /// </summary>
        public Vector2 Radius;
    }
}