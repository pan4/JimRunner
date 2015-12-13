using UnityEngine;
using System.Collections;

namespace JimRunner
{
    public class PickupItemController : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D other)
        {           
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Destroy(gameObject);
            }            
        }
    }
}