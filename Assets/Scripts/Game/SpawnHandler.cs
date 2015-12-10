using UnityEngine;
using System.Collections;

namespace JimRunner
{
    public class SpawnHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] stageCollection;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "SpawnTrigger")
            {
                var stage = other.gameObject;
                Transform spawnLocation = stage.transform.parent.Find("SpawnLocation");

                GameObject obj = Instantiate(stageCollection[(Random.Range(0, stageCollection.Length))],
                    spawnLocation.position,
                    Quaternion.identity) as GameObject;

                obj.name = "SpawnedStage";
            }

            if(other.gameObject.name == "SpawnLocation")
            {
                Destroy(other.gameObject.transform.parent.gameObject);
            }
        }
    }
}
