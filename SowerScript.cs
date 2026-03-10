using UnityEngine;
using System.Collections;
using HD;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;

public class SowerScript : MonoBehaviour, Interactor
{
    [SerializeField] private string prompt;
    public Transform parentObject;
    [SerializeField] private GridLayout gridLayout;
    public TileManager tileManager;
    [SerializeField] private Tilemap farmTileMap;
    [SerializeField] private TileBase plowedTileUnwatered;
    [SerializeField] private TileBase FertilizedTile;
    [SerializeField] public GameObject wheat;
    public int wSeeds = 0;
    public ShopScript shop = new ShopScript();
    public GameObject auxJect;

    public bool isContainer()
    {
        return false;
    }


    public string InteractionPrompt => "(E) equip sower";
    public bool hasSower;
    public GameObject Sower;

    public bool Interact(PlayerInteractor interactor) {
        Debug.Log("Touching Sower");
        return true;
    }

    public void sowerWSeeds(int amount)
    {
        wSeeds = amount;
    }
    public void Interacted() {
            Sower.transform.position = parentObject.position;
            Sower.transform.rotation = parentObject.rotation;
            Sower.transform.parent = parentObject;
            auxJect.SetActive(false);
            hasSower = true;
    }
    public void Interacted(GameObject hitch)
    {
        parentObject = hitch.transform;
        Sower.transform.position = parentObject.position;
        Sower.transform.rotation = parentObject.rotation;
        Sower.transform.parent = parentObject;
        auxJect.SetActive(false);
        hasSower = true;
    }

    public void Unteracted() {
        if (hasSower)
        {
            auxJect.SetActive(true);
            auxJect.transform.position = new Vector3(parentObject.transform.position.x, parentObject.transform.position.y, parentObject.transform.position.z+2);
            auxJect.transform.rotation = Sower.transform.rotation;
            auxJect.GetComponent<Rigidbody>().angularVelocity = new Vector3(0,0,0);
            gameObject.transform.position = auxJect.transform.position;
            gameObject.transform.rotation = auxJect.transform.rotation;
            Sower.transform.SetParent(auxJect.transform);
            hasSower = false;
        }
        else {
            Debug.Log("nothing to drop");
        }
    
    }

    void Awake()
    {

    }

    void Update()
    {
        Vector3Int cellPosition = gridLayout.WorldToCell(parentObject.position);
        if (hasSower)
        {
            TileBase tile = farmTileMap.GetTile(cellPosition);
            if (tile != null)
            {
                if (tile.name != FertilizedTile.name){
                    
                    if (shop.handler.seedsInContainer() > 0 /*&& shop.setToWheat()*/ && shop.handler.containtersEquipped > 0) {
                        Debug.Log("Planting");
                        wSeeds = shop.handler.wheatSeeds;
                        farmTileMap.SetTile(cellPosition, FertilizedTile);
                        shop.handler.accessableContainer().GetComponent<SeedContainerScript>().whichcrop.GetComponent<CropInterface>().Plant(farmTileMap.GetCellCenterWorld(cellPosition));
                        shop.handler.deincrementWheatSeeds();
                        shop.seedsAmount.text = shop.handler.wheatSeeds.ToString();
                        shop.cornSeedsAmount.text = shop.handler.cornSeeds.ToString();

                    }

                }


            }
        }

    }
}

