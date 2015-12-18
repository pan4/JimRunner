using UnityEngine;
using System.Collections;

namespace JimRunner
{
    public class SpawnHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] stageCollection;

        [SerializeField]
        private GameObject mainClouds;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "PlatformSpawnTrigger")
            {
                var stage = other.gameObject;
                Transform spawnLocation = stage.transform.parent.Find("PlatformSpawnLocation");

                GameObject obj = Instantiate(stageCollection[(Random.Range(0, stageCollection.Length))],
                    spawnLocation.position,
                    Quaternion.identity) as GameObject;

                obj.name = "SpawnedStage";
            }

            if (other.gameObject.tag == "MainCloudSpawnTrigger")
            {
                var stage = other.gameObject;
                Transform spawnLocation = stage.transform.parent.Find("MainCloudSpawnLocation");

                GameObject obj = Instantiate(mainClouds, spawnLocation.position, Quaternion.identity) as GameObject;
                obj.name = "SpawnedMainCloud";
                var root = stage.transform.parent.transform.parent;
                obj.transform.parent = root;
            }
        }
    }
}
