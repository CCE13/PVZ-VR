using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peashooters
{
    public class PeaShooterGun : MonoBehaviour
    {
        [SerializeField] private float _fireRate;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _bulletSpeed;
        private float _nextFire;
        private PeaShooter _peashooter;
        // Start is called before the first frame update
        void Start()
        {
            _nextFire = _fireRate;
            TryGetComponent(out _peashooter);
        }

        // Update is called once per frame
        void Update()
        {
            if (!_peashooter.IsEnemyInFront())
            {
                _nextFire = _fireRate;
                return;
            }
            Shoot();
        }
        private void Shoot()
        {
            _nextFire -= Time.deltaTime;
            if (_nextFire <= 0f)
            {
                Shooting();
                _nextFire = _fireRate;
            }
        }
        private void Shooting()
        {
            GameObject bullet = Instantiate(_bulletPrefab, _peashooter.FirePoint.position, _peashooter.transform.rotation);
            bullet.GetComponent<Bullet>().SetBulletSpeed(_bulletSpeed);
            bullet.transform.parent = transform.parent?.parent;
        }
    }

}
