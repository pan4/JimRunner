using UnityEngine;
using System.Collections;
using Core;

namespace JimRunner.Tile
{
    public class TileView : BaseMonoBehaviour
    {
        public GameObject SpawnTrigger;
        public GameObject SpawnLocation;
        public bool IsUsed = false;

        protected override void OnCreate()
        {
            base.OnCreate();
            SpawnTrigger = Transform.Find("SpawnTrigger").gameObject;
            SpawnLocation = Transform.Find("SpawnLocation").gameObject;
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

        }

    }
}
