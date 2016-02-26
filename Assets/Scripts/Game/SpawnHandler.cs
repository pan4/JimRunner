using UnityEngine;
using System.Collections.Generic;
using Core;
using JimRunner.Tile;
using System.Linq;

namespace JimRunner
{
    public class SpawnHandler : BaseMonoBehaviour
    {
        IEnumerable<BackgroundTileController> _disappearedTile;
        List<TileController> _appearedTile = new List<TileController>();
        bool _transparencyInProgress;
        float _alpha = 1f;

        private Queue<GameObject> unusedGrounds = new Queue<GameObject>();

        protected override void OnCreate()
        {
            base.OnCreate();
            GameFactory.LoadAll();
        }


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
                else
                    tileController.IsUsed = true;

                Transform root = tileController.Transform.parent;

                if (other.gameObject.tag == "PlatformSpawnTrigger")
                {
                    DequeueUsedTile(unusedGrounds);
                    int index = Random.Range(0, GameFactory.GetGroundsCount());
                    GameObject ground = GameFactory.GetGround(index);
                    unusedGrounds.Enqueue(SpawnTile(ground, tileController.SpawnLocation, tileController.GameObjectName, root));

                    index = Random.Range(0, (int)ObstacleType.Size);
                    GameObject obstacle = GameFactory.GetObstacel((ObstacleType)index);
                    GameObject obstacleClone = Instantiate(obstacle, tileController.SpawnLocation.position, Quaternion.identity) as GameObject;

                    //Vector3 pos =  obstacleClone.transform.position;
                    //pos.x = tileController.SpawnLocation.position.x;
                    //obstacleClone.transform.position = pos;
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
            BackgroundTileController[] allTile = FindObjectsOfType<BackgroundTileController>();
            LocationType disappearedLocation = LocationManager.PrevioustLocation;
            _disappearedTile = allTile.Where(
                (tile) => 
                {
                    if (tile != null)
                        return tile.LocationType == disappearedLocation;
                    return false; 
                }
            );
            foreach (BackgroundTileController tc in _disappearedTile)
            {
                tc.SpriteRenderer.sortingOrder = tc.SpriteRenderer.sortingOrder + 1;
                Destroy(tc.SpawnTrigger.gameObject);
                Destroy(tc.SpawnLocation.gameObject);
            }

            LocationManager.PrevioustLocation = LocationManager.CurrentLocation;

            foreach (BackgroundTileController disappered in _disappearedTile)
            {
                GameObject spawnedTile = null;

                if (disappered is TileMainCloudController)
                    spawnedTile = SpawnTile(GameFactory.GetMainCloud(), disappered.Transform, disappered.GameObjectName, disappered.Transform.parent);
                else if (disappered is TileCloudController)
                    spawnedTile = SpawnTile(GameFactory.GetCloud(), disappered.Transform, disappered.GameObjectName, disappered.Transform.parent);
                else if (disappered is TileFirstRockController)
                    spawnedTile = SpawnTile(GameFactory.GetFirstRock(), disappered.Transform, disappered.GameObjectName, disappered.Transform.parent);
                else if (disappered is TileSecondRockController)
                    spawnedTile = SpawnTile(GameFactory.GetSecondRock(), disappered.Transform, disappered.GameObjectName, disappered.Transform.parent);
                else if (disappered is TileSkyController)
                    spawnedTile = SpawnTile(GameFactory.GetSky(), disappered.Transform, disappered.GameObjectName, disappered.Transform.parent);

                if (spawnedTile != null)
                {
                    BackgroundTileController appered = spawnedTile.GetComponent<BackgroundTileController>();

                    if (appered != null)
                    {
                        _appearedTile.Add(appered);

                        Color color = appered.SpriteRenderer.color;
                        color.a = 0;
                        appered.SpriteRenderer.color = color;

                        if (disappered.IsUsed)
                            Destroy(appered.SpawnTrigger.gameObject);
                    }
                }
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
                foreach (BackgroundTileController tc in _disappearedTile)
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

                foreach (BackgroundTileController tc in _appearedTile)
                {
                    if (tc != null)
                    {
                        if (_alpha > 0)
                        {
                            Color c = tc.SpriteRenderer.color;
                            c.a = 1 - _alpha;
                            tc.SpriteRenderer.color = c;
                        }
                    }
                }

                if (_alpha <= 0)
                {
                    _alpha = 1f;
                    _transparencyInProgress = false;
                    _appearedTile.Clear();
                }
                _alpha -= 0.33f * Time.deltaTime;
            }
        }

    }
}
