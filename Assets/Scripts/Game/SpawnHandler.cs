using UnityEngine;
using System.Collections.Generic;
using Core;
using JimRunner.Tile;
using System.Linq;

namespace JimRunner
{
    public class SpawnHandler : BaseMonoBehaviour
    {
        [SerializeField]
        private GameObject[] groundCollection;

        private int _groundIndex = 0;

        [SerializeField]
        private GameObject[] mainCloud;
        [SerializeField]
        private GameObject[] firstRock;
        [SerializeField]
        private GameObject[] cloud;
        [SerializeField]
        private GameObject[] secondRock;
        [SerializeField]
        private GameObject[] sky;

        [SerializeField]
        private GameObject transitionGround;
        //[SerializeField]
        //private GameObject transitionMainCloud;
        //[SerializeField]
        //private GameObject transitionFirstRock;
        //[SerializeField]
        //private GameObject transitionCloud;
        //[SerializeField]
        //private GameObject transitionSecondRock;
        //[SerializeField]
        //private GameObject transitionSky;

        private Queue<GameObject> unusedGrounds = new Queue<GameObject>();
        //private Queue<GameObject> unusedMainClouds = new Queue<GameObject>();
        //private Queue<GameObject> unusedFirstRock = new Queue<GameObject>();
        //private Queue<GameObject> unusedClouds = new Queue<GameObject>();
        //private Queue<GameObject> unusedSecondRocks = new Queue<GameObject>();
        //private Queue<GameObject> unusedSkies = new Queue<GameObject>();

        protected override void OnEnabled()
        {
            base.OnEnabled();

            Clear();

            TileGroundController[] groundsOnScene = FindObjectsOfType<TileGroundController>();
            InitTileQueue(unusedGrounds, groundsOnScene);

            //TileMainCloudController[] mainGloudsOnScene = FindObjectsOfType<TileMainCloudController>();
            //InitTileQueue(unusedMainClouds, mainGloudsOnScene);

            //TileFirstRockController[] firstRockOnScene = FindObjectsOfType<TileFirstRockController>();
            //InitTileQueue(unusedFirstRock, firstRockOnScene);

            //TileCloudController[] cloudsOnScene = FindObjectsOfType<TileCloudController>();
            //InitTileQueue(unusedClouds, cloudsOnScene);

            //TileSecondRockController[] secondRocksOnScene = FindObjectsOfType<TileSecondRockController>();
            //InitTileQueue(unusedSecondRocks, secondRocksOnScene);

            //TileSkyController[] skiesOnScene = FindObjectsOfType<TileSkyController>();
            //InitTileQueue(unusedSkies, skiesOnScene);
        }

        private void Clear()
        {
            unusedGrounds.Clear();
            //unusedMainClouds.Clear();
            //unusedFirstRock.Clear();
            //unusedClouds.Clear();
            //unusedSecondRocks.Clear();
            //unusedSkies.Clear();
        }

        private void InitTileQueue(Queue<GameObject> queue,  TileController[] arr) 
        {
            arr = arr.OrderBy(tile => tile.Transform.position.x).ToArray();
            foreach (var tile in arr)
                if (!tile.IsUsed)
                    queue.Enqueue(tile.GameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(IsCorrectTag(other.gameObject.tag))
            {
                GameObject tile = other.gameObject;
                TileController tileController = tile.transform.parent.GetComponent<TileController>();
                if (tileController.IsUsed)
                    return;

                Transform root = tileController.Transform.parent;

                if (other.gameObject.tag == "PlatformSpawnTrigger")
                {
                    DequeueUsedTile(unusedGrounds);
                    GameObject ground = groundCollection[_groundIndex/*(UnityEngine.Random.Range(0, groundCollection.Length))*/];
                    unusedGrounds.Enqueue(SpawnTile(ground, tileController.SpawnLocation, tileController.GameObjectName, root));                    
                    //SpawnTile(ground, tileController.SpawnLocation, tileController.GameObjectName, root);
                }
                else if (other.gameObject.tag == "MainCloudSpawnTrigger")
                {
                    //DequeueUsedTile(unusedMainClouds);
                    //unusedMainClouds.Enqueue(SpawnTile(mainCloud, tileController.SpawnLocation, tileController.GameObjectName, root));
                    foreach (GameObject mc in mainCloud)
                        SpawnTile(mc, tileController.SpawnLocation, tileController.GameObjectName, root);
                }
                else if (other.gameObject.tag == "FirstRockSpawnTrigger")
                {
                    //DequeueUsedTile(unusedFirstRock);
                    //unusedFirstRock.Enqueue(SpawnTile(firstRock, tileController.SpawnLocation, tileController.GameObjectName, root));
                    foreach (GameObject fr in firstRock)
                        SpawnTile(fr, tileController.SpawnLocation, tileController.GameObjectName, root);
                }
                else if (other.gameObject.tag == "CloudSpawnTrigger")
                {
                    //DequeueUsedTile(unusedClouds);
                    //unusedClouds.Enqueue(SpawnTile(cloud, tileController.SpawnLocation, tileController.GameObjectName, root));
                    foreach (GameObject c in cloud)
                        SpawnTile(c, tileController.SpawnLocation, tileController.GameObjectName, root);
                }
                else if (other.gameObject.tag == "SecondRockSpawnTrigger")
                {
                    //DequeueUsedTile(unusedSecondRocks);
                    //unusedSecondRocks.Enqueue(SpawnTile(secondRock, tileController.SpawnLocation, tileController.GameObjectName, root));
                    foreach (GameObject sr in secondRock)
                        SpawnTile(sr, tileController.SpawnLocation, tileController.GameObjectName, root);
                }
                else if (other.gameObject.tag == "SkySpawnTrigger")
                {
                    //DequeueUsedTile(unusedSkies);
                    //unusedSkies.Enqueue(SpawnTile(sky, tileController.SpawnLocation, tileController.GameObjectName, root));
                    foreach (GameObject s in sky)
                        SpawnTile(s, tileController.SpawnLocation, tileController.GameObjectName, root);
                }

            }            
        }

        private TileController DequeueUsedTile(Queue<GameObject> queue)
        {
            if (queue.Count != 0)
            {
                GameObject due = queue.Dequeue();
                if (due != null)
                {
                    TileController controller = due.GetComponent<TileController>();
                    controller.GetComponent<TileController>().IsUsed = true;
                    return controller;
                }
            }
            return null;
        }

        private bool IsCorrectTag(string tag)
        {
            if (tag == "PlatformSpawnTrigger" ||
                tag == "MainCloudSpawnTrigger" ||
                tag == "FirstRockSpawnTrigger" ||
                tag == "CloudSpawnTrigger" ||
                tag == "SecondRockSpawnTrigger" ||
                tag == "SkySpawnTrigger")

                return true;
            return false;
        }

        private GameObject SpawnTile(GameObject tile, Transform spawnLocation, string name, Transform root)
        {
            GameObject obj = Instantiate(tile, spawnLocation.position, Quaternion.identity) as GameObject;
            obj.name = name;
            obj.transform.parent = root;
            return obj;
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();

            //TransitionController tc = FindObjectOfType<TransitionController>();
            //if (tc == null)
            //    return;

            //TileController controller =  DequeueUsedTile(unusedGrounds);
            //if (controller != null)
            //    SpawnTile(transitionGround, controller.SpawnLocation, controller.GameObjectName, controller.Transform.parent);

            //controller = DequeueUsedTile(unusedClouds);
            //if (controller != null)
            //     tc.transitionCloud = SpawnTile(transitionCloud, controller.SpawnLocation, controller.GameObjectName, controller.Transform.parent);

            //controller = DequeueUsedTile(unusedFirstRock);
            //if (controller != null)
            //    tc.transitionFirstRock = SpawnTile(transitionFirstRock, controller.SpawnLocation, controller.GameObjectName, controller.Transform.parent);

            //controller = DequeueUsedTile(unusedMainClouds);
            //if (controller != null)
            //    tc.transitionMainCloud = SpawnTile(transitionMainCloud, controller.SpawnLocation, controller.GameObjectName, controller.Transform.parent);

            //controller = DequeueUsedTile(unusedSecondRocks);
            //if (controller != null)
            //    tc.transitionSecondRock = SpawnTile(transitionSecondRock, controller.SpawnLocation, controller.GameObjectName, controller.Transform.parent);

            //controller = DequeueUsedTile(unusedSkies);
            //if (controller != null)
            //    tc.transitionSky = SpawnTile(transitionSky, controller.SpawnLocation, controller.GameObjectName, controller.Transform.parent);
        }

        IEnumerable<SpriteRenderer> _dayTile;
        bool _transparencyInProgress;
        float _alpha = 1f;
         
        public void SetTransparency()
        {
            _transparencyInProgress = true;
            SpriteRenderer[] allTile = FindObjectsOfType<SpriteRenderer>();
            _dayTile = allTile.Where(
                (tile) => 
                {
                    if (tile != null)
                        return tile.gameObject.layer == LayerMask.NameToLayer("Day");
                    return false; 
                }
            );
        }

        public void TileTransitonGround()
        {
            _groundIndex++;
            TileController controller = DequeueUsedTile(unusedGrounds);
            if (controller != null)
                unusedGrounds.Enqueue(SpawnTile(transitionGround, controller.SpawnLocation, controller.GameObjectName, controller.Transform.parent));
        }

        private void Update()
        {
            if(_transparencyInProgress)
            {
                foreach (SpriteRenderer r in _dayTile)
                {
                    if (r != null)
                    {
                        Color c = r.color;
                        c.a = _alpha;
                        r.color = c;
                    }
                }

                if (_alpha == 0)
                {
                    _alpha = 1f;
                    _transparencyInProgress = false;
                }
                _alpha -= 0.005f;
            }
        }

    }
}
