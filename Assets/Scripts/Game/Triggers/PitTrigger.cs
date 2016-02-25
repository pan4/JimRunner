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
                JimController jim = collider.GetComponent<JimController>();
                jim.GroundColliders.gameObject.SetActive(false);
                jim.OnPit = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                JimController jim = collider.GetComponent<JimController>();
                jim.GroundColliders.gameObject.SetActive(true);
                jim.OnPit = false;
            }
        }
    }
}
