using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{


    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private TextMeshProUGUI leaderboardScoreText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI leaderboardNameText;

    private int leaderboardID = 5018;
    private int leaderboardTopCount = 10;
    private int score = 0;

    public void StopGame(int score)
    {
        //Time.timeScale = 0f;
        //gameOverCanvas.SetActive(true);
        this.score = score;
        scoreText.text = score.ToString();
        GetLeaderBoard();
        AddXP(score);
    }

  

    public void SubmitScore()
    {
        StartCoroutine(SubmitScoreToLeaderboard());
    }

    private IEnumerator SubmitScoreToLeaderboard()
    {
        bool? nameSet = null;

        LootLockerSDKManager.SetPlayerName(inputField.text, (response) => {
            if (response.success)
            {
                Debug.Log("Succesfully set the player name");
                nameSet = true;
            }
            else
            {
                Debug.Log("Error setting the player name");
                nameSet = false;
            }
               
        }); 

        yield return new WaitUntil(() => nameSet.HasValue);
        //if (!nameSet.Value) yield break;

        bool? scoreSubmitted = null;

        LootLockerSDKManager.SubmitScore("",score,leaderboardID,(response) => {
            if (response.success)
            {
                Debug.Log("Successfully submitted the scoree");
                scoreSubmitted = true;
            }
            else
            {
                Debug.Log("Error setting the score");
                scoreSubmitted = false;
            }

        });

        yield return new WaitUntil(() => scoreSubmitted.HasValue);
        if (!nameSet.Value) yield break;

        GetLeaderBoard();

    }

    private void GetLeaderBoard()
    {
        LootLockerSDKManager.GetScoreList(leaderboardID, leaderboardTopCount, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully retrieved scores from leaderboard.");
                string leaderboardName = "";
                string leaderboardScore = "";
                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i < members.Length; ++i)
                {
                    LootLockerPlayer player = members[i].player;
                        
                    if (player == null) continue;

                    if (player.name != "")
                    {
                        leaderboardName += player.name + "\n";
                        
                    }
                    else
                    {
                        leaderboardName += player.id + "\n";

                    }
                    leaderboardScore += members[i].score + "\n";
                }
                leaderboardNameText.SetText(leaderboardName);
                leaderboardScoreText.SetText(leaderboardScore);


            }
            else
            {
                Debug.Log("Failed to get scores from leaderboard.");
            }
        });
    }


    public void AddXP(int score)
    {
        LootLockerSDKManager.SubmitXp(score, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully submitted the XP");
            }

            else
            {
                Debug.Log("Failed adding XP");
            }
                
        });
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
