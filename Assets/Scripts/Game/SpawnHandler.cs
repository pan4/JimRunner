using UnityEngine;
using System.Collections;
using Core;

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

        private GameObject lastGround;
        private GameObject lastMainClouds;
        private GameObject lastFirstRock;
        private GameObject lastClouds;
        private GameObject lastSecondRocks;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(IsCorrectTag(other.gameObject.tag))
            {
                var stage = other.gameObject;
                var root = stage.transform.parent.transform.parent;
                Transform spawnLocation = stage.transform.parent.Find(SpawnLocation);

                if (other.gameObject.tag == "PlatformSpawnTrigger")
                {
                    lastGround = SpawnTile(groundCollection[(Random.Range(0, groundCollection.Length))], spawnLocation, "Ground", root);
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
            Transform spawnLocation = lastGround.transform.Find(SpawnLocation);
            SpawnTile(transitionGround, spawnLocation, "Ground", lastGround.transform.parent);
        }

    }
}
