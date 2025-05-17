using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public Text tourLabel;
    public Text dice1, dice2, dice3;
    public Text victoryLabel;
    public Button restartButton;
    public Button rollDiceButton;

    public void UpdateUI(ChessGameManager manager)
    {
        tourLabel.text = "Tour : " + manager.currentTurn.ToString();

        // Copy of remaining usable types
        var usable = manager.GetUsableTypes();

        // Update dice display
        dice1.text = "Dé 1 : " + (GetUsedStatus(manager.diceResults, usable, 0));
        dice2.text = "Dé 2 : " + (GetUsedStatus(manager.diceResults, usable, 1));
        dice3.text = "Dé 3 : " + (GetUsedStatus(manager.diceResults, usable, 2));

        // Victory & restart
        if (manager.gameOver)
        {
            victoryLabel.text = manager.victoryMessage;
            restartButton.gameObject.SetActive(true);
        }
        else
        {
            victoryLabel.text = "";
            restartButton.gameObject.SetActive(false);
        }

        // Disable roll button if already rolled
        if (rollDiceButton != null)
            rollDiceButton.interactable = !manager.HasRolledDice();
    }

    string GetUsedStatus(string[] results, System.Collections.Generic.List<PieceType> usable, int index)
    {
        if (results.Length <= index || results[index] == "-")
            return "-";

        PieceType type;
        if (System.Enum.TryParse(results[index], out type))
        {
            if (usable.Contains(type))
                return results[index];
            else
                return "-";
        }
        return "-";
    }
}