using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform contentPanel;    
    public GameObject Flower;
    public GameObject startScreen;
    public GameObject windPrefab;
    private List<SkinItem> skinItems;
    private List<SkinItem> treeSkins;
    private List<SkinItem> windSkins;
    private List<SkinItem> backgroundSkins;


    void Start()
    {
        skinItems = new List<SkinItem>();
        windSkins = new List<SkinItem>();
        backgroundSkins = new List<SkinItem>();

        LoadSkinsFromFilter();
        PopulateFlowerShop();
    }

    public void Return()
    {
            gameObject.SetActive(false);
            startScreen.SetActive(true);
    }

    void LoadSkinsFromFilter()
    {
        skinItems.Add(new SkinItem("Skin 1", 100, Resources.Load<Sprite>("FlowerSkins/BaseFlower/flower1"), Resources.Load<RuntimeAnimatorController>("FlowerSkins/BaseFlower/FlowerAnimation")));
        skinItems.Add(new SkinItem("Skin 2", 100, Resources.Load<Sprite>("FlowerSkins/SpecialFlower/test"), Resources.Load<RuntimeAnimatorController>("FlowerSkins/SpecialFlower/FlowerAnimation1")));


        // Wind
        windSkins.Add(new SkinItem("wind", 100, Resources.Load<Sprite>("WindSkins/air1")));
        windSkins.Add(new SkinItem("Red wind", 100, Resources.Load<Sprite>("WindSkins/air2")));
        windSkins.Add(new SkinItem("Red wind", 100, Resources.Load<Sprite>("WindSkins/air_black")));

        // Backgrounds
        backgroundSkins.Add(new SkinItem("base", 100, Resources.Load<Sprite>("Backgrounds/base")));
        backgroundSkins.Add(new SkinItem("base", 100, Resources.Load<Sprite>("Backgrounds/pink")));
        backgroundSkins.Add(new SkinItem("base", 100, Resources.Load<Sprite>("Backgrounds/orange")));



    }

    void clearContent()
    {
        foreach (Transform child in contentPanel)
            {
                Destroy(child.gameObject);
            }
    }

    public void PopulateFlowerShop()
    {
        clearContent();
        foreach (SkinItem skinItem in skinItems)
        {
            GameObject item = Instantiate(itemPrefab) as GameObject;
            item.transform.SetParent(contentPanel);
            Component[] components = itemPrefab.GetComponentsInChildren<Component>(true);
            Image skinImage = item.transform.Find("SkinImage").GetComponent<Image>();
            skinImage.sprite = skinItem.sprite;
            TextMeshProUGUI skinName = item.transform.Find("SkinName").GetComponent<TextMeshProUGUI>();
            skinName.text = skinItem.name;

            Button skinButton = item.transform.Find("SkinButton").GetComponent<Button>();
            skinButton.onClick.AddListener(delegate { SelectSkin(skinItem); });
            // button.skinName.text = skinItem.name;
            // button.skinPrice.text = skinItem.price.ToString();
            // button.skinImage.sprite = skinItem.sprite;
            // button.skinImage.preserveAspect = true;
            // button.skinItem = skinItem;
            // button.shop = this;
        }
    }

    public void PopulateTreeShop()
    {
        clearContent();
    }

    public void PopulateWindShop()
    {
        clearContent();
        foreach (SkinItem skinItem in windSkins)
        {
            GameObject item = Instantiate(itemPrefab) as GameObject;
            item.transform.SetParent(contentPanel);
            Component[] components = itemPrefab.GetComponentsInChildren<Component>(true);
            Image skinImage = item.transform.Find("SkinImage").GetComponent<Image>();
            skinImage.sprite = skinItem.sprite;
            TextMeshProUGUI skinName = item.transform.Find("SkinName").GetComponent<TextMeshProUGUI>();
            skinName.text = skinItem.name;

            Button skinButton = item.transform.Find("SkinButton").GetComponent<Button>();
            skinButton.onClick.AddListener(delegate { SelectWindSkin(skinItem); });
        }
    }

    public void PopulateBackgroundShop()
    {
        clearContent();
        clearContent();
        foreach (SkinItem skinItem in backgroundSkins)
        {
            GameObject item = Instantiate(itemPrefab) as GameObject;
            item.transform.SetParent(contentPanel);
            Component[] components = itemPrefab.GetComponentsInChildren<Component>(true);
            Image skinImage = item.transform.Find("SkinImage").GetComponent<Image>();
            skinImage.sprite = skinItem.sprite;
            TextMeshProUGUI skinName = item.transform.Find("SkinName").GetComponent<TextMeshProUGUI>();
            skinName.text = skinItem.name;

            Button skinButton = item.transform.Find("SkinButton").GetComponent<Button>();
            skinButton.onClick.AddListener(delegate { SelectBackground(skinItem); });
        }
    }


    public void SelectSkin(SkinItem skinItem)
    {

        GameObject obj = GameObject.FindWithTag("Flower");
        Transform transform = obj.transform;
        Quaternion quat = obj.transform.rotation;
        Destroy(obj);
        SpriteRenderer sr = this.Flower.GetComponent<SpriteRenderer>();
        sr.sprite = skinItem.sprite;
        Animator objAnimator = this.Flower.GetComponent<Animator>();
        objAnimator.runtimeAnimatorController = skinItem.animator;
        Instantiate(Flower, transform.position, quat);

    }

    public void SelectWindSkin(SkinItem skinItem)
    {
        SpriteRenderer sr = windPrefab.GetComponent<SpriteRenderer>();
        sr.sprite = skinItem.sprite;
    }

    public void SelectBackground(SkinItem skinItem) {
        GameObject bg = GameObject.Find("Background");
        SpriteRenderer sr = bg.GetComponent<SpriteRenderer>();
        sr.sprite = skinItem.sprite;
    }
}

public class SkinItem
{
    public string name;
    public int price;
    public Sprite sprite;
    public RuntimeAnimatorController animator = null;
    public SkinItem(string name, int price, Sprite sprite, RuntimeAnimatorController animator = null)
    {
        this.name = name;
        this.price = price;
        this.sprite = sprite;
        this.animator = animator;
    }
}

