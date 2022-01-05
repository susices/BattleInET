using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class LooseQuadTreeNode : Entity
    {
        public LooseQuadTree Tree;
        
        public Vector2 CenterPoint;

        public float Radius;

        public LooseQuadTreeNode[] ChildNode;

        public List<Entity> Entities;
    }
}