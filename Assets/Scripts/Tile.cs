using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Image icon, fog;
    public string type = "none";
    public GameObject button;
    public int number;
    public Map map;

    void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
    }

    public void Clicked()
    {
        if (map.free == true)
        {
            if (type == "none")
            {
                map.ClearFog(number);
                map.ClearTile(number);
            }
            else if (type == "fight")
            {
                map.free = false;
                map.ClearFog(number);
                map.current = number;
                map.FightClicked();
            }
            else if (type == "elite")
            {
                map.free = false;
                map.ClearFog(number);
                map.current = number;
                map.EliteClicked();
            }
            else
            {
                if (fog.enabled == true)
                    map.ClearFog(number);
                else
                {
                    map.free = false;
                    map.current = number;
                    if (type == "stash")
                        map.StashClicked();
                    else if (type == "waygate")
                        map.WaygateClicked();
                    else if (type == "treasure")
                        map.TreasureClicked();
                    else if (type == "campfire")
                        map.CampfireClicked();
                    else if (type == "merchant")
                        map.MerchantClicked();
                }
            }
        }
    }
}