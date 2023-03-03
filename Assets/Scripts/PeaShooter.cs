using UnityEngine;

namespace Peashooters
{
    public class PeaShooter : BasePlant
    {
        [SerializeField] private float _turretRange;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private Transform _firePoint;
        
        public Transform FirePoint { get { return _firePoint; } }
        public bool IsEnemyInFront()
        {
            Debug.Log(Physics.Raycast(_firePoint.position, transform.forward, _turretRange, _enemyLayer));
            return Physics.Raycast(_firePoint.position, transform.forward, _turretRange, _enemyLayer);
        }
    }

}
