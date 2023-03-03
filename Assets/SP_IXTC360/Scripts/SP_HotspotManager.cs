using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_HotspotManager: MonoBehaviour
{
    public GameObject hotspotPrefab;
    public GameObject content;
    private Dictionary<int, SP_HotspotWrapper> m_HotSpotCollection = new Dictionary<int, SP_HotspotWrapper>();
    private Dictionary<int, GameObject> m_HotSpotPrefabRecord =  new Dictionary<int, GameObject>();
    private static SP_HotspotManager _instance;
    public static SP_HotspotManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public void AddHotSpotToCollection(GameObject hotspot)
    {
        if (!m_HotSpotCollection.ContainsKey(hotspot.GetInstanceID()))
        {
            SP_HotspotWrapper tempWrapper = new SP_HotspotWrapper();
            tempWrapper.hotspotObj = hotspot;
            m_HotSpotCollection.Add(hotspot.GetInstanceID(), tempWrapper);

            GameObject tempRecord = Instantiate(hotspotPrefab, content.transform);
            m_HotSpotPrefabRecord.Add(hotspot.GetInstanceID(), tempRecord);
        }
    }

    public void UpdateRecords()
    {
        foreach (KeyValuePair<int,GameObject> keyPair in m_HotSpotPrefabRecord)
        {
            SP_HotspotWrapper spWrap;
            if (m_HotSpotCollection.TryGetValue(keyPair.Key, out spWrap))
            {
                SP_HotspotRecord rec = keyPair.Value.GetComponent<SP_HotspotRecord>();
                if (rec)
                {
                    SP_Hotspot hotspotInfo = spWrap.hotspotObj.GetComponent<SP_Hotspot>();
                    rec.hotspotImage.texture = hotspotInfo.myInfo.displayImage.texture;
                    rec.hotspotName.text = hotspotInfo.myInfo.hotspotName; ;
                    rec.foucsedTime.text = hotspotInfo.ActiveTime.ToString();
                }
            }

        }
    }
    
    public void RemoveHotSpotFromCollection(int hotspotID)
    {
        if (m_HotSpotCollection.ContainsKey(hotspotID))
        {
            m_HotSpotCollection.Remove(hotspotID);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

}
