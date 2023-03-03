using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

namespace Peashooters
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _bulletSpeed;
        private void Start()
        {
            Destroy(gameObject, 5f);
        }
        // Update is called once per frame
        void Update()
        {
            transform.localPosition += transform.forward * _bulletSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                AudioManager.Instance?.Play("HitEnemy");
                other.GetComponent<EnemyController>().LoseHeath();
                Destroy(gameObject);
            }
        }
        public void SetBulletSpeed(float speed)
        {
            _bulletSpeed = speed;
        }
    }
}

