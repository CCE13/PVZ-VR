using SeedPlanting;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SeedManager : MonoBehaviour
{
    [SerializeField] private GameObject _seedSlotToSpawn;
    public static SeedData _currentSeedSelected;
    public static GameObject _currentSeedSlotSelected;
    private void Start()
    {
        Seeds.ShowOnUI += AddSeedObject;
    }
    private void OnDestroy()
    {
        Seeds.ShowOnUI -= AddSeedObject;
    }
    private void AddSeedObject(SeedData SeedData)
    {
        if (SeedData.seedName == transform.Find(SeedData.seedName)?.name)
        {
            int currentCount = int.Parse(transform.Find(SeedData.seedName).GetComponentInChildren<TMP_Text>().text);
            int newCount = currentCount + SeedData.numberOfSeeds;
            SeedData.numberOfSeeds = newCount;
            transform.Find(SeedData.seedName).GetComponentInChildren<TMP_Text>().text = newCount.ToString();
            Button b = transform.Find(SeedData.seedName).GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => SelectSeed(SeedData, transform.Find(SeedData.seedName).gameObject));
        }
        else
        {
            GameObject seedSlot = Instantiate(_seedSlotToSpawn, transform);
            Button button = seedSlot.GetComponent<Button>();
            Image image = seedSlot.transform.GetChild(0).GetComponent<Image>();
            TMP_Text textCount = seedSlot.GetComponentInChildren<TMP_Text>();
            seedSlot.name = SeedData.seedName;
            image.sprite = SeedData.SeedSprite;
            textCount.text = SeedData.numberOfSeeds.ToString();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => SelectSeed(SeedData, seedSlot));
        }
        //Spawn a seedslot object and set its variables.
    }
    private void SelectSeed(SeedData dataSelected,GameObject _currentSeatSlotSelected)
    {
        _currentSeedSelected = dataSelected;
        _currentSeedSlotSelected= _currentSeatSlotSelected;
    }
    public static void UpdateCount()
    {
        _currentSeedSelected.numberOfSeeds--;
        _currentSeedSlotSelected.GetComponentInChildren<TMP_Text>().text = _currentSeedSelected.numberOfSeeds.ToString();
        if (_currentSeedSelected.numberOfSeeds <= 0)
        {
            Destroy(_currentSeedSlotSelected);
        }
        _currentSeedSelected = null;
    }
}
