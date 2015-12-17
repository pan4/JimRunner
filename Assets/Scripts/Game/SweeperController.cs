using UnityEngine;
using System.Collections;
using Scene;

namespace JimRunner
{
    public class SweeperController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "PlatformSpawnLocation")
            {
                Destroy(other.gameObject.transform.parent.gameObject);
            }

            if (other.gameObject.name == "MainCloudSpawnLocation")
            {
                Destroy(other.gameObject.transform.parent.gameObject);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                Destroy(other.gameObject);
            }

            if(other.gameObject.tag == "Player")
            {
                SceneLoader.LoadScene(typeof(EndlessSceneController));
            }

        }
    }
}