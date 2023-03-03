using Spawner;
using UnityEngine;
using UnityEngine.EventSystems;
using Peashooters;

public class Node : MonoBehaviour
{
    [SerializeField] private Color _hoverColor;
    [SerializeField] private Color _normalColor;

    private Renderer _renderer;
    private PeaShooter _currentTurret;

    private void Start()
    {
        TryGetComponent(out _renderer);
        _normalColor = _renderer.material.color;
        Color hoverColor = new Color(_normalColor.r-0.3f, _normalColor.g - 0.3f, _normalColor.b - 0.3f, _normalColor.a);
        _hoverColor = hoverColor;
    }
    public Vector3 PositionToSpawn(GameObject turret)
    {
       return new Vector3(transform.position.x,turret.transform.localScale.y/2 + transform.lossyScale.y/2 + transform.position.y,transform.position.z);
    }
    private void SelectNode()
    {
        GameObject currentTurretOnNode = BuildManager.instance.SpawnObject(this);
        _currentTurret = currentTurretOnNode.GetComponent<PeaShooter>();
    }
    private void OnMouseDown()
    {
        if (_currentTurret == null)
        {
            SelectNode();
        }
        else
        {
            Debug.Log(_currentTurret + " is Selected");
        }

    }
    private void OnMouseEnter()
    {
        ChangeMaterialColour(_hoverColor);
    }
    private void OnMouseExit()
    {
        ChangeMaterialColour(_normalColor);
    }
    private void ChangeMaterialColour(Color colorToSet)
    {
        _renderer.material.color = colorToSet;
    }
}
