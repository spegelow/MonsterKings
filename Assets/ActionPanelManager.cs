using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanelManager : MonoBehaviour
{
    public static ActionPanelManager instance;

    public List<Button> buttons;
    public GameObject actionPanel;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public static void InitializePanel(BattleUnit activeUnit)
    {
        instance._InitializePanel(activeUnit);
    }

    private void _InitializePanel(BattleUnit activeUnit)
    {
        //Set the text for each button and disable ones that have no associated action
        Button currentButton;
        for (int i = 0; i<8; i++)
        {
            currentButton = buttons[i];
            if (activeUnit.actions.Count>i)
            {
                currentButton.gameObject.GetComponentInChildren<Text>().text = (i+1) + " " + activeUnit.actions[i].actionName;
                currentButton.gameObject.SetActive(true);
            }
            else
            {
                currentButton.gameObject.SetActive(false);
            }
        }

        //Activate the panel
        actionPanel.SetActive(true);
    }

    public static void HidePanel()
    {
        instance.actionPanel.SetActive(false);
    }

    public void ActionButtonPressed(int i)
    {
        BattleManager.ActionSelected(i);
    }
}
