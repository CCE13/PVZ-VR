using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SP_SphereWrapper
{
    public GameObject sphereObj;
    public float activeTime = 0.0f;
}

public class SP_HotspotWrapper
{
    public GameObject hotspotObj;
    //Any other information

}

public class SP_DataCapturing : MonoBehaviour
{

    public GameObject m_MenuPanel;
    public SP_SphereWrapper m_CurrentSphereWrapper;
    private static SP_DataCapturing _instance;
    public static SP_DataCapturing Instance { get { return _instance; } }

    //private Dictionary<int, SP_HotspotWrapper> m_HotSpotCollection = new Dictionary<int, SP_HotspotWrapper>();
    private Dictionary<int, SP_SphereWrapper> m_SphereCollection = new Dictionary<int, SP_SphereWrapper>();

    private void Awake()
    {
        //if there memory allocated to _instance //the memeory allocated is not myself
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    //public void AddHotSpotToCollection(GameObject hotspot)
    //{
    //    if (!m_HotSpotCollection.ContainsKey(hotspot.GetInstanceID()))
    //    {
    //        SP_HotspotWrapper tempWrapper = new SP_HotspotWrapper();
    //        tempWrapper.hotspotObj = hotspot;
    //        m_HotSpotCollection.Add(hotspot.GetInstanceID(), tempWrapper);
    //    }
    //}

    //public void RemoveHotSpotFromCollection(int hotspotID)
    //{
    //    if (m_HotSpotCollection.ContainsKey(hotspotID))
    //    {
    //        m_HotSpotCollection.Remove(hotspotID);
    //    }
    //}

    public void RemoveSphereFromCollection(int gameObjID)
    {
        if (m_SphereCollection.ContainsKey(gameObjID))
        {
            m_SphereCollection.Remove(gameObjID);
        }
    }


    public void AddSphereToCollection(GameObject sphere)
    {
        if (!m_SphereCollection.ContainsKey(sphere.GetInstanceID()))
        {
            SP_SphereWrapper tempWrapper = new SP_SphereWrapper();
            tempWrapper.sphereObj = sphere;
            m_SphereCollection.Add(sphere.GetInstanceID(), tempWrapper);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_MenuPanel.SetActive(!m_MenuPanel.activeSelf);
            SP_HotspotManager.Instance.UpdateRecords();
            //Pause the Video in the BG

        }

        ComputeNearestSphere();
        m_CurrentSphereWrapper.activeTime += Time.deltaTime;

    }

    void ComputeNearestSphere()
    {
        if (m_SphereCollection.Count > 0)
        {
            int count = 0;
            float nearestDistance = -1.0f;
            SP_SphereWrapper nearestSphereWrapper = null;
            foreach (KeyValuePair<int, SP_SphereWrapper> obj in m_SphereCollection)
            {
                GameObject sphr = obj.Value.sphereObj;
                float distanceSqr = Vector3.SqrMagnitude((sphr.transform.position - this.transform.position));
                if (count == 0)
                {
                    nearestSphereWrapper = obj.Value;
                    nearestDistance = distanceSqr;
                    count++;
                    continue;
                }

                if (distanceSqr < nearestDistance)
                {
                    nearestSphereWrapper = obj.Value;
                    nearestDistance = distanceSqr;
                }
                count++;
            }
            m_CurrentSphereWrapper = nearestSphereWrapper;
        }

    }

}
