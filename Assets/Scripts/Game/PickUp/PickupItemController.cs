using UnityEngine;
using Core;

namespace JimRunner.Game.PickUp
{
    public class PickupItemController : BaseMonoBehaviour
    {
        protected void OnPickUp() { }
        private void OnTriggerEnter2D(Collider2D other)
        {           
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                OnPickUp();
                Destroy(gameObject);
            }            
        }
    }
}