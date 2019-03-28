using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class TilePositionComparator : IEqualityComparer<Vector3Int>
    {
        public bool Equals(Vector3Int a, Vector3Int b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public int GetHashCode(Vector3Int obj)
        {
            return obj.x.GetHashCode() ^ obj.y.GetHashCode() ^ obj.z.GetHashCode();
        }
    }

    /*
    [Serializable]
    public class TilePosition
    {
        public int x;
        public int y;
        public int z;

        public Vector3Int ToVec3()
        {
            return new Vector3Int(x, y, z);
        }

        public TilePosition(Vector3Int pos)
        {
            this.x = pos.x;
            this.y = pos.y;
            this.z = pos.z;
        }

        public override bool Equals(object other)
        {
            if(other is TilePosition tp)
            {
                return x == tp.x && y == tp.y && z == tp.z;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    */
}
