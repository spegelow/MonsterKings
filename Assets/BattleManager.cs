using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public List<BattleUnit> units;
    public List<MapTile> tiles;

    public GameObject mapTilePrefab;

    private bool waitingForInput;
    private bool gettingTarget;
    private int width;
    private int height;

    private BattleUnit activeUnit;
    private BattleAction currentAction;

    public Color baseColor;
    public Color selectedColor;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        //Setup up the map and place units
        SetupMap(6, 3);
        units[0].SetTile(GetTile(4, 1));
        units[1].SetTile(GetTile(1, 1));

        //Start the battle
        waitingForInput = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


        units.ForEach(u => u.BattleUpdate());

        if (waitingForInput)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                ActionSelected(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ActionSelected(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ActionSelected(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ActionSelected(3);
            }
            else //if(Input.GetKey(KeyCode.Space))
            {
                MapTile m = null;
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    m = GetTile(activeUnit.currentTile.x, activeUnit.currentTile.y + 1);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    m = GetTile(activeUnit.currentTile.x - 1, activeUnit.currentTile.y);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    m = GetTile(activeUnit.currentTile.x + 1, activeUnit.currentTile.y);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    m = GetTile(activeUnit.currentTile.x, activeUnit.currentTile.y - 1);
                }

                if(m != null)
                {
                    ActionSelected(4);
                    TileClicked(m);
                }
            }
        }
        else if (gettingTarget)
        {
            MapTile m = null;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                m = GetTile(activeUnit.currentTile.x, activeUnit.currentTile.y + 1);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                m = GetTile(activeUnit.currentTile.x - 1, activeUnit.currentTile.y);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                m = GetTile(activeUnit.currentTile.x + 1, activeUnit.currentTile.y);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                m = GetTile(activeUnit.currentTile.x, activeUnit.currentTile.y - 1);
            }

            if (m != null)
            {
                TileClicked(m);
            }
        }
    }

    public void SetupMap(int width, int height)
    {
        this.width = width;
        this.height = height;

        for (int y = 0; y<height; y++)
        {
            for(int x=0; x<width; x++)
            {
                GameObject g = GameObject.Instantiate(mapTilePrefab, new Vector3(2 * x - width + 1, 0, 2 * y - height + 1), Quaternion.identity);
                MapTile newTile = g.GetComponent<MapTile>();

                newTile.x = x;
                newTile.y = y;
                this.tiles.Add(newTile);
            }
        }
    }

    public static bool IsBattlePaused()
    {
        return instance.waitingForInput || instance.gettingTarget;
    }

    public static void ActivateUnit(BattleUnit unit)
    {
        instance.waitingForInput = true;
        Time.timeScale = 0;
        ActionPanelManager.InitializePanel(unit);
        instance.activeUnit = unit;
        instance.activeUnit.baseSprite.color = instance.selectedColor;
    }

    public static void ActionSelected(int index)
    {
        
        ActionPanelManager.HidePanel();
        instance.waitingForInput = false;
        
        instance.gettingTarget = true;
        instance.currentAction = instance.activeUnit.actions[index];
        
    }

    public static MapTile GetTile(int x, int y)
    {
        int index = y * instance.width + x;

        if (x < 0 || x >= instance.width || y<0 || y>= instance.height)
        {
            Debug.Log("Invalid map coordinates:("+x+", "+y+")");
            return null;
        }

        return instance.tiles[index];
    }

    public static void TileClicked(MapTile tile)
    {
        if(instance.gettingTarget)
        {
            instance.gettingTarget = false;
            Time.timeScale = 1;
            instance.activeUnit.BeginAction(instance.currentAction, tile);
            instance.activeUnit.baseSprite.color = instance.baseColor;
        }
        Debug.Log("Tile clicked (" + tile.x + ", " + tile.y + ")");
    }
}
