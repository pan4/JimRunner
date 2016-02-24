using UnityEngine;
using System;
using System.Collections.Generic;


namespace Triggers
{
	public enum TriggerMode { OnEnter, OnExit, OnStay };
	public enum TriggerDoAfter { Nothing, SelfDisable, DisableGameObject };

    [RequireComponent(typeof(Collider))]
    public class Trigger : MonoBehaviour
    {
        [SerializeField]
        protected TriggerMode Mode = TriggerMode.OnEnter;

        [SerializeField]
        protected float Delay = 0f;

        [SerializeField]
        private TriggerDoAfter _doAfter = TriggerDoAfter.Nothing;

        [SerializeField]
        private List<string> _tagsForTrigger = new List<string>();

        [SerializeField]
        private List<string> _layersForTrigger = new List<string>();

        private float _triggerTime;
        private bool _triggerActivated;

        protected virtual void OnCreate() { }
        protected virtual void OnStart() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnReleaseResource() { }
        protected virtual void OnTrigger() { }

        private void Awake()
        {
            OnCreate();
        }

        private void Start()
        {
            OnStart();
        }

        private void Update()
        {
            OnUpdate();

            if (_triggerActivated)
            {
                if (Time.time >= _triggerTime)
                {
                    OnTrigger();
                    DoAfter();
                    _triggerActivated = false;
                }
            }
        }

        private void OnDestroy()
        {
            OnReleaseResource();
        }

        private void DoAfter()
        {
            switch (_doAfter)
            {
                case TriggerDoAfter.SelfDisable:
                    {
                        this.enabled = false;
                        break;
                    }
                case TriggerDoAfter.DisableGameObject:
                    {
                        gameObject.SetActive(false);
                        break;
                    }
            }
        }

        private void ActivateTrigger()
        {
            _triggerTime = Time.time + Delay;
            _triggerActivated = true;
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            if (Mode == TriggerMode.OnEnter)
            {
                if (ActivateByTags(collider)) return;
                if (ActivateByTriggers(collider)) return;
            }
        }

        protected virtual void OnTriggerExit(Collider collider)
        {
            if (Mode == TriggerMode.OnExit)
            {
                if (ActivateByTags(collider)) return;
                if (ActivateByTriggers(collider)) return;
            }
        }

        protected virtual void OnTriggerStay(Collider collider)
        {
            if (Mode == TriggerMode.OnStay)
            {
                if (ActivateByTags(collider)) return;
                if (ActivateByTriggers(collider)) return;
            }
        }

        protected bool ActivateByTags(Collider collider)
        {
            foreach (string tag in _tagsForTrigger)
                if (collider.gameObject.tag == tag)
                {
                    ActivateTrigger();
                    return true;
                }
            return false;
        }

        protected bool ActivateByTriggers(Collider collider)
        {
            foreach (string layer in _layersForTrigger)
                if (collider.gameObject.layer == LayerMask.NameToLayer(layer))
                {
                    ActivateTrigger();
                    return true;
                }
            return false;
        }

        public void AddLayer(string layer)
        {
            if (_layersForTrigger == null)
                _layersForTrigger = new List<string>();
            _layersForTrigger.Add(layer);
        }
    }
}
