using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    // TextMeshProUGUI for displaying main game over message
    [SerializeField] private TextMeshProUGUI mainText;

    // TextMeshProUGUI for displaying additional information or sub-message
    [SerializeField] private TextMeshProUGUI subText;

    // Method to set up the game over screen based on the result
    public void SetUpScreen(bool hasWon)
    {
        // Activate the game over screen
        gameObject.SetActive(true);

        // Check if the player has won or lost and display the appropriate message
        if (hasWon)
        {
            GameWon();
        }
        else
        {
            GameLost();
        }
    }

    // Method to set up the game over screen for a win
    private void GameWon()
    {
        mainText.text = "You Win!";
        subText.text = "You Destroyed the Enemy Tower!";
    }

    // Method to set up the game over screen for a loss
    private void GameLost()
    {
        mainText.text = "Game Over";
        subText.text = "You Ran out of Boulders!";
    }

    // Method to retry the game by reloading the scene
    public void Retry()
    {
        SceneManager.LoadScene("DwarvenCatapult");
    }

    // Method to quit the game
    public void Quit()
    {
        Application.Quit();
    }
}
