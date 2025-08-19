using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Databases;

namespace Databases.UI
{
    /// <summary>
    /// Simple UI controller for testing database functionality
    /// </summary>
    public class DatabaseTestUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button addHighScoreButton;
        [SerializeField] private Button showHighScoresButton;
        [SerializeField] private Button clearDataButton;
        [SerializeField] private TextMeshProUGUI displayText;
        [SerializeField] private TMP_InputField playerNameInput;
        [SerializeField] private TMP_InputField scoreInput;

        private void Start()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            // TODO: Students will connect these button events
            if (addHighScoreButton != null)
                addHighScoreButton.onClick.AddListener(OnAddHighScore);

            if (showHighScoresButton != null)
                showHighScoresButton.onClick.AddListener(OnShowHighScores);

            if (clearDataButton != null)
                clearDataButton.onClick.AddListener(OnClearData);

            // Set default values
            if (playerNameInput != null)
                playerNameInput.text = "TestPlayer";

            if (scoreInput != null)
                scoreInput.text = "1000";

            UpdateDisplay("Database Test UI Ready\nClick buttons to test database operations");
        }

        /// TODO: Students will implement this method
        private void OnAddHighScore()
        {
            try
            {
                string playerName = playerNameInput?.text ?? "Unknown";
                string scoreText = scoreInput?.text ?? "0";

                if (int.TryParse(scoreText, out int score))
                {
                    // TODO: Use GameDataManager to add the high score
                    GameDataManager.Instance.AddHighScore(playerName, score);

                    UpdateDisplay($"High score added: {playerName} - {score} points");

                    // Generate random score for next test
                    if (scoreInput != null)
                        scoreInput.text = Random.Range(500, 2000).ToString();
                }
                else
                {
                    UpdateDisplay("Error: Invalid score format");
                }
            }
            catch (System.Exception ex)
            {
                UpdateDisplay($"Error adding high score: {ex.Message}");
            }
        }

        /// TODO: Students will implement this method
        private void OnShowHighScores()
        {
            try
            {
                // TODO: Use GameDataManager to get high scores
                var scores = GameDataManager.Instance.GetTopHighScores();

                if (scores.Count == 0)
                {
                    UpdateDisplay("No high scores found in database");
                }
                else
                {
                    string displayText = "Top High Scores:\n";
                    for (int i = 0; i < scores.Count; i++)
                    {
                        var score = scores[i];
                        displayText += $"{i + 1}. {score.PlayerName}: {score.Score} pts\n";
                    }
                    UpdateDisplay(displayText);
                }
            }
            catch (System.Exception ex)
            {
                UpdateDisplay($"Error loading high scores: {ex.Message}");
            }
        }

        /// TODO: Students will implement this method
        private void OnClearData()
        {
            try
            {
                // TODO: Use GameDataManager to clear all high scores
                GameDataManager.Instance.ClearAllHighScores();

                UpdateDisplay("All high scores cleared from database");
            }
            catch (System.Exception ex)
            {
                UpdateDisplay($"Error clearing data: {ex.Message}");
            }
        }

        private void UpdateDisplay(string message)
        {
            if (displayText != null)
            {
                displayText.text = message;
            }
        }
    }
}