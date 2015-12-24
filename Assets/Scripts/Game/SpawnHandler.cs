using UnityEngine;
using System.Collections.Generic;
using Core;
using JimRunner.Tile;
using System.Linq;

namespace JimRunner
{
    public class SpawnHandler : BaseMonoBehaviour
    {
        private const string SpawnLocation = "SpawnLocation";

        [SerializeField]
        private GameObject[] groundCollection;
        [SerializeField]
        private GameObject mainClouds;
        [SerializeField]
        private GameObject firstRock;
        [SerializeField]
        private GameObject clouds;
        [SerializeField]
        private GameObject secondRocks;

        [SerializeField]
        private GameObject transitionGround;
        [SerializeField]
        private GameObject transitionMainClouds;
        [SerializeField]
        private GameObject transitionFirstRock;
        [SerializeField]
        private GameObject transitionClouds;
        [SerializeField]
        private GameObject transitionSecondRocks;

        private Queue<GameObject> lastGrounds = new Queue<GameObject>();
        private GameObject lastMainClouds;
        private GameObject lastFirstRock;
        private GameObject lastClouds;
        private GameObject lastSecondRocks;

        protected override void OnCreate()
        {
            base.OnCreate();
            TileGroundView [] groundsOnScene = FindObjectsOfType<TileGroundView>();
            groundsOnScene.OrderBy(ground => ground.Transform.position.x);
            foreach (var ground in groundsOnScene)
                if (!ground.IsUsed)
                    lastGrounds.Enqueue(ground.GameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(IsCorrectTag(other.gameObject.tag))
            {
                var stage = other.gameObject;
                var root = stage.transform.parent.transform.parent;
                Transform spawnLocation = stage.transform.parent.Find(SpawnLocation);

                if (other.gameObject.tag == "PlatformSpawnTrigger")
                {
                    if(lastGrounds.Count != 0)
                        lastGrounds.Dequeue().GetComponent<TileView>().IsUsed = true;
                    lastGrounds.Enqueue(SpawnTile(groundCollection[(Random.Range(0, groundCollection.Length))], spawnLocation, "Ground", root));
                }
                else if (other.gameObject.tag == "MainCloudSpawnTrigger")
                {
                    lastMainClouds = SpawnTile(mainClouds, spawnLocation, "MainCloud", root);
                }
                else if (other.gameObject.tag == "FirstRockSpawnTrigger")
                {
                    lastFirstRock = SpawnTile(firstRock, spawnLocation, "FirstRock", root);
                }
                else if (other.gameObject.tag == "CloudSpawnTrigger")
                {
                    lastClouds = SpawnTile(clouds, spawnLocation, "Cloud", root);
                }
                else if (other.gameObject.tag == "SecondRockSpawnTrigger")
                {
                    lastSecondRocks = SpawnTile(secondRocks, spawnLocation, "SecondRock", root);
                }
            }            
        }

        private bool IsCorrectTag(string tag)
        {
            if (tag == "PlatformSpawnTrigger" ||
                tag == "MainCloudSpawnTrigger" ||
                tag == "FirstRockSpawnTrigger" ||
                tag == "CloudSpawnTrigger" ||
                tag == "SecondRockSpawnTrigger")
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
            GameObject lastGround = lastGrounds.Dequeue();
            lastGround.GetComponent<TileView>().IsUsed = true;
            Transform spawnLocation = lastGround.transform.Find(SpawnLocation);
            SpawnTile(transitionGround, spawnLocation, "Ground", lastGround.transform.parent);
        }
    }
}
