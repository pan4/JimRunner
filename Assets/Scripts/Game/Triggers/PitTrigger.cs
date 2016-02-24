using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JimRunner.Triggers
{
    public class PitTrigger : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collider.GetComponent<JimController>().GroundColliders.gameObject.SetActive(false);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collider.GetComponent<JimController>().GroundColliders.gameObject.SetActive(true);
            }
        }
    }
}
