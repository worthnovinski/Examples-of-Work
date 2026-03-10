using UnityEngine;

public class Corn : MonoBehaviour, CropInterface
{
    public ShopScript shop = new ShopScript();
    public bool grown = false;
    public int value;
    public float grownNum = 900;
    public string name = "corn";
    public IA_AtmosphereManager ATMOS = new IA_AtmosphereManager();
    public GameObject body;
    public GameObject body1;
    public GameObject body2;
    public GameObject body3;
    public float height = 1.0f;
    public float timePlanted;
    public int tix = 0;

    public bool Chop() {
        if (grown)
        {
            Debug.Log("Chopped Corn");
            shop.increaseCornAmount();



            Destroy(gameObject);
            Debug.Log("chopped corn");
        }
        return grown;
    }
    public void Plant(Vector3 position) {

         GameObject crop = Instantiate(gameObject, position, new Quaternion(0, 0, 0, 0));
         crop.SetActive(true);


    }
    public int Value => value;
    public float grownCap => grownNum;
    public string cropType => name;

    void Start()
    {
        timePlanted = ATMOS.whatTime();
        height = 1.0f;
        tix = 0;
    }

    void Update()
    {
        if (tix == grownNum)
        {
            grown = true;
        }
        else
        {
            height = 1.0f + tix / grownNum;
        }

        if ((int)ATMOS.whatTime() % 10 == 0 && !grown)
        {
            tix++;
            body.transform.localScale = new Vector3(1, height, 1);
            if ((height) >= 1.2 && (height) < 1.45)
            {
                body.SetActive(false);
                body1.SetActive(true);
                body1.transform.localScale = new Vector3(5, 5f + height, 5);


            }
            if ((height) >= 1.45 && (height) < 1.90)
            {
                body1.SetActive(false);
                body2.SetActive(true);
                body2.transform.localScale = new Vector3(2.5f, 2.5f + height, 2.5f);


            }
            if ((height) >= 1.90 && (height) <= 2)
            {
                body2.SetActive(false);
                body3.SetActive(true);
                body3.transform.localScale = new Vector3(5, 5f + height, 5);


            }
        }


    }
}
