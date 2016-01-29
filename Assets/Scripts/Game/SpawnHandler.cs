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
        private GameObject[] transitionGrounds;

        private Queue<GameObject> unusedGrounds = new Queue<GameObject>();

        protected override void OnEnabled()
        {
            base.OnEnabled();

            Clear();

            TileGroundController[] groundsOnScene = FindObjectsOfType<TileGroundController>();
            InitTileQueue(unusedGrounds, groundsOnScene);
        }

        private void Clear()
        {
            unusedGrounds.Clear();
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
                    GameObject ground = groundCollection[_groundIndex];
                    unusedGrounds.Enqueue(SpawnTile(ground, tileController.SpawnLocation, tileController.GameObjectName, root));                    
                }
                else if (other.gameObject.tag == "MainCloudSpawnTrigger")
                {
                    foreach (GameObject mc in mainCloud)
                        SpawnTile(mc, tileController.SpawnLocation, tileController.GameObjectName, root);
                }
                else if (other.gameObject.tag == "FirstRockSpawnTrigger")
                {
                    foreach (GameObject fr in firstRock)
                        SpawnTile(fr, tileController.SpawnLocation, tileController.GameObjectName, root);
                }
                else if (other.gameObject.tag == "CloudSpawnTrigger")
                {
                    foreach (GameObject c in cloud)
                        SpawnTile(c, tileController.SpawnLocation, tileController.GameObjectName, root);
                }
                else if (other.gameObject.tag == "SecondRockSpawnTrigger")
                {
                    foreach (GameObject sr in secondRock)
                        SpawnTile(sr, tileController.SpawnLocation, tileController.GameObjectName, root);
                }
                else if (other.gameObject.tag == "SkySpawnTrigger")
                {
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
                unusedGrounds.Enqueue(SpawnTile(transitionGrounds[_groundIndex], controller.SpawnLocation, controller.GameObjectName, controller.Transform.parent));
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
