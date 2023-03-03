using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {

        [SerializeField] private float _detectRange;
        [SerializeField] private Transform _raycastPoint;
        [SerializeField] private LayerMask _attackable;
        [SerializeField] private Vector3 _overlapSquareSize;

        private float _speed;
        private int _health;

        [Header("--- Seed Dropping On Kill---")]
        private float DropChance;
        [SerializeField] private List<GameObject> _seedsDroppable;

        private Animator _animator;
        private Rigidbody _rb;
        private IAttackable _currentlyAttackingObject;
        private CapsuleCollider _collider;
        private bool _isDead;



        private void Awake()
        {
            TryGetComponent(out _animator);
            TryGetComponent(out _rb);
            TryGetComponent(out _collider);
        }
        // Start is called before the first frame update


        // Update is called once per frame
        private void Update()
        {
            if (_isDead)
            {
                _rb.velocity = Vector3.zero;
                return;
            }
            if (CheckIfAttackable())
            {
                _rb.velocity = Vector3.zero;
                _animator.Play("Attacking");
            }
            else
            {
                _currentlyAttackingObject = null;
                _rb.velocity = transform.forward * _speed;
                _animator.Play("Walking");
            }

        }
        public void PlayGroan()
        {
            AudioManager.Instance?.Play("ZombieGroan");
        }
        public void AttackPlant()
        {
            _currentlyAttackingObject.OnAttacked();
        }
        private bool CheckIfAttackable()
        {
            Collider[] plantsDetection = Physics.OverlapBox(_raycastPoint.transform.position, _overlapSquareSize,transform.rotation,_attackable);
            bool attackable = plantsDetection.Length > 0;
            if (attackable)
            {
                for (int i = 0; i < plantsDetection.Length; i++)
                {
                    if (plantsDetection[i].TryGetComponent(out IAttackable attack))
                    {
                        _currentlyAttackingObject = attack;
                        return attackable;
                    }
                }       
            }
            return false;
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_raycastPoint.transform.position, _overlapSquareSize);
        }
        public void LoseHeath(bool instantKill = false)
        {
            _health--;
            if (_health <= 0 || instantKill)
            {
                _isDead = true;
                _collider.enabled = false;
                PlayDeathAnimation();
            }
        }
        public void PlayDeathAnimation()
        {
            _animator.Play("Death");
        }
        public void CalculateDropSeedChance()
        {       
            int randomChance = Random.Range(0, 101);
            if(randomChance < DropChance)
            {
              GameObject seedToDrop =  _seedsDroppable[Random.Range(0, 2)];
              GameObject seed = Instantiate(seedToDrop,_raycastPoint.position,Quaternion.identity);
              seed.transform.parent = transform.parent;
              AudioManager.Instance.Play("SeedDropped");
            }
            Destroy(gameObject);
        }
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
        public void SetHealth(int health)
        {
            _health = health;
        }
        public void SetDropChance(float chance)
        {
            DropChance = chance;
        }
    }
}

