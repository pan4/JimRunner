using UnityEngine;
using System.Collections.Generic;
using Core;
using JimRunner.Tile;
using System.Linq;

namespace JimRunner
{
    public class SpawnHandler : BaseMonoBehaviour
    {
        IEnumerable<TileController> _disappearedTile;
        bool _transparencyInProgress;
        float _alpha = 1f;

        private Queue<GameObject> unusedGrounds = new Queue<GameObject>();

        protected override void OnEnabled()
        {
            base.OnEnabled();

            TileGroundController[] groundsOnScene = FindObjectsOfType<TileGroundController>();
            InitTileQueue(unusedGrounds, groundsOnScene);
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
                    int index = Random.Range(0, GameFactory.GetGroundsCount());
                    GameObject ground = GameFactory.GetGround(index);
                    unusedGrounds.Enqueue(SpawnTile(ground, tileController.SpawnLocation, tileController.GameObjectName, root));                    
                }
                else if (other.gameObject.tag == "MainCloudSpawnTrigger")
                {
                    GameObject mc = GameFactory.GetMainCloud();
                    SpawnTile(mc, tileController.SpawnLocation, tileController.GameObjectName, root);
                }
                else if (other.gameObject.tag == "FirstRockSpawnTrigger")
                {
                    GameObject fr = GameFactory.GetFirstRock();
                    SpawnTile(fr, tileController.SpawnLocation, tileController.GameObjectName, root);
                }
                else if (other.gameObject.tag == "CloudSpawnTrigger")
                {
                    GameObject c = GameFactory.GetCloud();
                    SpawnTile(c, tileController.SpawnLocation, tileController.GameObjectName, root);
                }
                else if (other.gameObject.tag == "SecondRockSpawnTrigger")
                {
                    GameObject sr = GameFactory.GetSecondRock();
                    SpawnTile(sr, tileController.SpawnLocation, tileController.GameObjectName, root);
                }
                else if (other.gameObject.tag == "SkySpawnTrigger")
                {
                    GameObject s = GameFactory.GetSky();
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

        public void SetTransparency()
        {
            _transparencyInProgress = true;
            TileController[] allTile = FindObjectsOfType<TileController>();
            int disappearedLayer = LayerMask.NameToLayer(LocationManager.PrevioustLocation.ToString());
            _disappearedTile = allTile.Where(
                (tile) => 
                {
                    if (tile != null)
                        return tile.GameObject.layer == disappearedLayer;
                    return false; 
                }
            );
            foreach (TileController tc in _disappearedTile)
                tc.SpriteRenderer.sortingOrder = tc.SpriteRenderer.sortingOrder + 1;

            LocationManager.PrevioustLocation = LocationManager.CurrentLocation;

            foreach (TileController c in _disappearedTile)
            {
                if (c is TileMainCloudController)
                    SpawnTile(GameFactory.GetMainCloud(), c.Transform, c.GameObjectName, c.Transform.parent);
                else if (c is TileCloudController)
                    SpawnTile(GameFactory.GetCloud(), c.Transform, c.GameObjectName, c.Transform.parent);
                else if (c is TileFirstRockController)
                    SpawnTile(GameFactory.GetFirstRock(), c.Transform, c.GameObjectName, c.Transform.parent);
                else if (c is TileSecondRockController)
                    SpawnTile(GameFactory.GetSecondRock(), c.Transform, c.GameObjectName, c.Transform.parent);
                else if (c is TileSkyController)
                    SpawnTile(GameFactory.GetSky(), c.Transform, c.GameObjectName, c.Transform.parent);
            }
        }

        public void SpawnTransitionGround(LocationType type)
        {
            TileController controller = DequeueUsedTile(unusedGrounds);
            if (controller != null)
            {
                GameObject tg = GameFactory.GetTransitionGround((int)LocationManager.CurrentLocation);
                unusedGrounds.Enqueue(SpawnTile(tg, controller.SpawnLocation, controller.GameObjectName, controller.Transform.parent));
            }
        }

        private void Update()
        {
            if(_transparencyInProgress)
            {
                foreach (TileController tc in _disappearedTile)
                {
                    if (tc != null)
                    {
                        if (_alpha > 0)
                        {
                            Color c = tc.SpriteRenderer.color;
                            c.a = _alpha;
                            tc.SpriteRenderer.color = c;
                        }
                        else
                            Destroy(tc.GameObject);
                    }
                }

                if (_alpha <= 0)
                {
                    _alpha = 1f;
                    _transparencyInProgress = false;
                }
                _alpha -= 0.005f;
            }
        }

    }
}
