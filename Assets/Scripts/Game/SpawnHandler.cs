using UnityEngine;
using System.Collections;

namespace JimRunner
{
    public class SpawnHandler : MonoBehaviour
    {
        private const string SpawnLocation = "SpawnLocation";

        [SerializeField]
        private GameObject[] stageCollection;

        [SerializeField]
        private GameObject mainClouds;

        [SerializeField]
        private GameObject firstRock;

        [SerializeField]
        private GameObject clouds;

        [SerializeField]
        private GameObject secondRocks;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "PlatformSpawnTrigger")
            {
                var stage = other.gameObject;
                Transform spawnLocation = stage.transform.parent.Find(SpawnLocation);

                GameObject obj = Instantiate(stageCollection[(Random.Range(0, stageCollection.Length))],
                    spawnLocation.position,
                    Quaternion.identity) as GameObject;

                obj.name = "Platform";
                var root = stage.transform.parent.transform.parent;
                obj.transform.parent = root;
            }

            if (other.gameObject.tag == "MainCloudSpawnTrigger")
            {
                var stage = other.gameObject;
                Transform spawnLocation = stage.transform.parent.Find(SpawnLocation);

                GameObject obj = Instantiate(mainClouds, spawnLocation.position, Quaternion.identity) as GameObject;
                obj.name = "MainCloud";
                var root = stage.transform.parent.transform.parent;
                obj.transform.parent = root;
            }

            if (other.gameObject.tag == "FirstRockSpawnTrigger")
            {
                var stage = other.gameObject;
                Transform spawnLocation = stage.transform.parent.Find(SpawnLocation);

                GameObject obj = Instantiate(firstRock, spawnLocation.position, Quaternion.identity) as GameObject;
                obj.name = "FirstRock";
                var root = stage.transform.parent.transform.parent;
                obj.transform.parent = root;
            }

            if (other.gameObject.tag == "CloudSpawnTrigger")
            {
                var stage = other.gameObject;
                Transform spawnLocation = stage.transform.parent.Find(SpawnLocation);

                GameObject obj = Instantiate(clouds, spawnLocation.position, Quaternion.identity) as GameObject;
                obj.name = "Cloud";
                var root = stage.transform.parent.transform.parent;
                obj.transform.parent = root;
            }

            if (other.gameObject.tag == "SecondRockSpawnTrigger")
            {
                var stage = other.gameObject;
                Transform spawnLocation = stage.transform.parent.Find(SpawnLocation);

                GameObject obj = Instantiate(secondRocks, spawnLocation.position, Quaternion.identity) as GameObject;
                obj.name = "SecondRock";
                var root = stage.transform.parent.transform.parent;
                obj.transform.parent = root;
            }
        }
    }
}
