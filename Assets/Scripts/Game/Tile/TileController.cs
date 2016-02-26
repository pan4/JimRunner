using UnityEngine;
using System.Collections;
using Core;

namespace JimRunner.Tile
{
    public class TileController : BaseMonoBehaviour
    {
        public GameObject SpawnTrigger;
        public Transform SpawnLocation;
        public bool IsUsed = false;
        public LocationType LocationType;

        public virtual string GameObjectName
        {
            get
            {
                return "Tile";
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            SpawnTrigger = Transform.Find("SpawnTrigger").gameObject;
            SpawnLocation = Transform.Find("SpawnLocation").gameObject.transform;
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
        }

    }
}
