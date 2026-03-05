using System;
using System.Collections;
using UnityEngine;

namespace Minibonk.Collectable
{
    public abstract class Collectable : MonoBehaviour
    {
        [SerializeField] float _frequnecy;
        [SerializeField] float _Magnitude;
        [SerializeField] private float _lifeTime = 45f;

        private Vector3 _startPos;
        
        #region Unity Methods

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _startPos = transform.position;

            StartCoroutine(DestroyOnLifetime());
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
            if (other.gameObject.CompareTag("Player"))
            {
                Collected();
                HandleCollection();
            }
        }

        private void HandleCollection()
        {
            Destroy(gameObject);
        }

        private IEnumerator DestroyOnLifetime()
        {
            yield return new WaitForSeconds(_lifeTime);
            Destroy(gameObject);
        }

        #endregion
        
        protected abstract void Collected();
    }
}
