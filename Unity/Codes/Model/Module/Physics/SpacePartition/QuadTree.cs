using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class QuadTree : Entity
    {
        public Vector2 CenterPoint;

        public float HalfWidth;

        public QuadTree[] ChildNode;

        public List<Entity> Entities;
    }
}