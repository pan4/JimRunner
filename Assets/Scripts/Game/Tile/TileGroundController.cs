using UnityEngine;
using System.Collections;

namespace JimRunner.Tile
{
    public class TileGroundController : TileController
    {
        [SerializeField]
        private LocationType _type;
        public LocationType Type
        {
            get
            {
                return _type;
            }
        }

        public override string GameObjectName
        {
            get
            {
                return "Ground";
            }
        }
    }
}
