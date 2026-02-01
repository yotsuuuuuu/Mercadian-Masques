using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI turnText;
    int turn = 0;
    [SerializeField] TextMeshProUGUI ActionText;
    [SerializeField] TextMeshProUGUI maskText;
    [SerializeField] Image maskImage;
    [SerializeField] Sprite[] maskSprites;

    public void UpdateMaskImage(maskType type_)
    {
        int index = 0;
        string action = "";
        string mask = "Mask:\t\t";

        maskImage.sprite = maskSprites[index];
        maskText.SetText(mask);
        UpdateTurn();

        switch (type_)
        {
            case maskType.Null:
                index = 0;
                action = "";
                return;

            case maskType.Bull:
                index = 1;
                action = "BREAK ROCK";
                mask += "BULL";
                break;
            case maskType.Deer:
                action = "THROUGH THE HEDGE";
                index = 2;
                mask += "DEER";
                break;
            case maskType.Frog:
                action = "LEAP OVER";
                index = 3;
                mask += "FROG";
                break;
            case maskType.Bird:
                action = "FLYING";
                index = 4;
                mask += "BIRD";
                break;
            case maskType.Snake:
                action = "DOUBLE YOUR NEXT MOVE";
                index = 5;
                mask += "SNAKE";
                break;
            default:
                index = 0;
                action = "";
                break;
        }

        UpdateActiontext(action);
        maskImage.sprite = maskSprites[index];
        maskText.SetText(mask);
    }

    public void UpdateActiontext(string action_)
    {
        ActionText.SetText(action_);
    }

    public void UpdateActionText(Queue<CardMove> list)
    {
        //ActionText.SetText("");
        string action = "";

        foreach(var c in list)
        {
            action += c.direction + ":" + c.amount + "\n";
        }
        ActionText.SetText(action);
        int index = 0;
        string mask = "Mask:\t\t";

        maskImage.sprite = maskSprites[index];
        maskText.SetText(mask);
        //UpdateMaskImage(maskType.Null);
    }
    private void UpdateTurn(bool restart = false) 
    {
        if (restart)
        {
            turn = -1;
        }
        turnText.text = "Turn: \t\t" + turn++;
    }

    public void UpdateLevelText(int level_)
    {
        levelText.text = "Level: \t\t" + level_;
    }
}
