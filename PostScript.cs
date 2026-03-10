using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PostScript : MonoBehaviour, BuildingInter
{
    public bool connected = false;
    public bool placed = false;
    public bool wallActive = true;
    public int attachActive = 0;
    public GameObject chain;
    public GameObject hitchLink;
    public GameObject towerlink;
    public GameObject auxJect;
    public TileManager tileManager;
    [SerializeField] private Tilemap farmTileMap;
    [SerializeField] private GridLayout gridLayout;
    public GameObject player;
    public BuildingManager buildingManager;
    public GameObject[] connectedWalls;

    public GameObject hitchTemp;
    public bool isPlaced()
    {
        return placed;
    }

    public string returnName() {
        if (placed)
        {
            return "(E) toggle electric fencepost";
        }
        return "(E) equip electric fencepost";
    }
    public void Start()
    {
        if (!chain.GetComponent<ChainScriptYDC>().set)
        {
            chain.GetComponent<ChainScriptYDC>().setLocalLinkPos();
            chain.GetComponent<ChainScriptYDC>().set = true;
        }
        connectedWalls = new GameObject[10];

    }
    public bool Interact(PlayerInteractor interactor) {
        return true;
    }
    
    public void Interacted() {
        if (!connected && !placed)
        {
            connected = true;
            buildingManager.setConnected(gameObject);
            Transform chainOriginPos = chain.GetComponent<ChainScriptYDC>().getChainOriginLoc();

            chain.transform.position = hitchTemp.transform.position;

            gameObject.transform.parent = auxJect.transform;
            Vector3 backward = -player.transform.forward;
            auxJect.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 50, player.transform.position.z) + (backward * 8);
            chain.transform.LookAt(auxJect.transform);
            chain.transform.position = hitchTemp.transform.position;
            chain.GetComponent<ChainScriptYDC>().positionCall();

            auxJect.transform.position = towerlink.transform.position;

            auxJect.transform.position = new Vector3(towerlink.transform.position.x, towerlink.transform.position.y, towerlink.transform.position.z);


            chain.SetActive(true);
            auxJect.SetActive(true);


        } if (!connected && placed) {
            for (int i = 0; i < connectedWalls.Length; i++) {
                if (connectedWalls[i] != null) { 
                    connectedWalls[i].SetActive(!wallActive);
                
                }
            
            }
            wallActive = !wallActive;
        }

    }

    void Update() { 
        if (connected && Input.GetKeyUp(KeyCode.P)) {
            Vector3Int cellPosition = gridLayout.WorldToCell(gameObject.transform.position);
            gameObject.transform.SetParent(null);
            auxJect.SetActive(false);
            auxJect.transform.position = new Vector3(40, 4, 15);
            foreach (Transform child in chain.transform) {
                Rigidbody linkRB = child.GetComponentInChildren<Rigidbody>();
                linkRB.isKinematic = true;
            }
            chain.SetActive(false);

            gameObject.transform.position = farmTileMap.GetCellCenterWorld(cellPosition);
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+2, gameObject.transform.position.z);
            gameObject.transform.localScale = new Vector3(1,3,1);
            GameObject wallHand = null;
            if (attachActive < 9) {
                connectedWalls.SetValue(wallHand = buildingManager.generateWall(), attachActive);
                attachActive++;
            }

            buildingManager.updatePosts(gameObject);
            buildingManager.setConnected(null);
            connected = false;
            tileManager.SetInteracted(gameObject.transform);
            placed = true;


        }

    }

}
