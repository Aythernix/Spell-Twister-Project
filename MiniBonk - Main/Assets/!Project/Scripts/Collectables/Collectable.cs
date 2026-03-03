using System;
using UnityEngine;

namespace Minibonk.Collectable
{
    public abstract class Collectable : MonoBehaviour
    {
        [SerializeField] float _frequnecy;
        [SerializeField] float _Magnitude;

        private Vector3 _startPos;
        
        #region Unity Methods

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _startPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = _startPos + transform.up * (Mathf.Sin(Time.time * _frequnecy) * _Magnitude);
        }

        #endregion

        #region Private Methods

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
            if (other.CompareTag("Player"))
            {
                Collected();
                HandleCollection();
            }
        }

        private void HandleCollection()
        {
            Destroy(gameObject);
        }

        #endregion
        
        protected abstract void Collected();
    }
}
