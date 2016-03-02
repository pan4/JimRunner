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

            if (collider.gameObject.layer == LayerMask.NameToLayer("PitLayer"))
            {
                collider.transform.parent.GetComponent<Collider2D>().enabled = false;
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

            if (collider.gameObject.layer == LayerMask.NameToLayer("PitLayer"))
            {
                collider.transform.parent.GetComponent<Collider2D>().enabled = true;
            }
        }
    }
}
