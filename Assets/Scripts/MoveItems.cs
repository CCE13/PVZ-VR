
using System.Collections.Generic;
using UnityEngine;


namespace VR
{
    public class MoveItems : MonoBehaviour
    {
        [SerializeField] private Transform _player;

        private Transform _arrowButton;
        private List<Transform> _childObjects;
        private bool _needY = false;

        private void Awake()
        {
            _childObjects = new List<Transform>();
        }

        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<SP_TeleportBuilderScript>()) continue;
                _childObjects.Add(transform.GetChild(i));
            }
        }



        public void MoveItem(SP_TeleportBuilderScript _teleportor)
        {
            //Gets the vector difference between the player and the arrow to press
            var diffed = _arrowButton.position - _player.position;

            //for each child in the list, set the local position in the new sphere and offset it by the diffed vector.
            for (int i = 0; i < _childObjects.Count; i++)
            {
                var obj = _childObjects[i];
                if (obj == null) continue;
                var localPos = obj.localPosition;
                obj.parent = _teleportor.TargetSphere.transform;
                obj.transform.localPosition = localPos;
                obj.transform.position -= _needY ? new Vector3(diffed.x, diffed.y, diffed.z) : new Vector3(diffed.x, 0f, diffed.z);
                _teleportor.TargetSphere.GetComponent<MoveItems>()?.AddToList(obj);
            }
            _childObjects.Clear();
            _teleportor.Teleport();
            _needY = false;
        }
        public void SetArrow(Transform arrow)
        {
            _arrowButton = arrow;
        }
        public void RequireY(bool needY)
        {
            _needY = needY;
        }
        public void AddToList(Transform obj)
        {
            _childObjects.Add(obj);
        }
    }
}

