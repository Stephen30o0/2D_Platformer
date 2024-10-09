using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public GameObject gameOverPanel; // Reference to the Game Over UI panel
    public GameObject startGamePanel; // Reference to the Start Game UI panel
    public GameObject playerPrefab; // Reference to the Player prefab
    public int playerLives = 3; // Number of lives
    private Vector2 respawnPoint; // Respawn point for the player

    private GameObject player; // Reference to the player GameObject

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        gameOverPanel.SetActive(false); // Hide the Game Over panel at the start
        startGamePanel.SetActive(true); // Show the Start Game panel
    }

    public void StartGame() {
        startGamePanel.SetActive(false); // Hide Start Game panel
        playerLives = 3; // Reset lives

        // Instantiate the player if it doesn't already exist
        if (player == null) {
            player = Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
            player.tag = "Player"; // Ensure it's tagged as Player
        } else {
            player.SetActive(true); // Activate the player if already exists
        }
    }

    public void PlayerEnteredWater(Vector2 position) {
        Debug.Log($"Player entered water. Current Lives: {playerLives}");
        if (playerLives > 0) {
            playerLives--; // Decrease lives
            respawnPoint = position; // Set the respawn point
            RespawnPlayer(); // Call the respawn function
        } else {
            ShowGameOver(); // Show Game Over panel if no lives left
        }
    }

    private void RespawnPlayer() {
        if (player != null) {
            player.transform.position = respawnPoint; // Move the player to the water entry point
            player.SetActive(true); // Activate the player if it was inactive
            Debug.Log("Player respawned at " + respawnPoint);
        } else {
            Debug.LogError("Player reference is null! Cannot respawn.");
        }
    }

    private void ShowGameOver() {
        Debug.Log("Game Over! Displaying UI.");
        gameOverPanel.SetActive(true); // Show the Game Over panel
        Time.timeScale = 0; // Pause the game
    }

    public void ReplayGame() {
    Debug.Log("Replay button clicked");
    gameOverPanel.SetActive(false); // Hide the Game Over panel
    Time.timeScale = 1; // Resume the game
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
}

    public void QuitGame() {
    Debug.Log("Quit button clicked");
    gameOverPanel.SetActive(false); // Hide the Game Over panel
    startGamePanel.SetActive(true); // Show the Start Game panel
    Time.timeScale = 1; // Resume game time if paused
    SceneManager.LoadScene("GameScene-ALU"); 
}
}
