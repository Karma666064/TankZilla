using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] GameObject scoreContainer;
    [SerializeField] GameObject textPrefab;

    List<ScoreType> players = new List<ScoreType>
    {
        new ScoreType {ID = 1, pseudo = "Karma", wins = 12},
        new ScoreType {ID = 2, pseudo = "Hugo", wins = 6},
        new ScoreType {ID = 3, pseudo = "Loïc", wins = 3},
        new ScoreType {ID = 4, pseudo = "Maeva", wins = 24},
        new ScoreType {ID = 5, pseudo = "Simon", wins = 48}
    };

    private void Start()
    {
        if (players.Count > 0)
        {
            for (var i = 1; i <= GetLeaderboardList().Count; i++)
            {
                ScoreType player = GetLeaderboardList()[i - 1];
                GameObject textPlayer = Instantiate(textPrefab);

                TextMeshProUGUI[] texts = textPlayer.GetComponentsInChildren<TextMeshProUGUI>();
                TextMeshProUGUI textPlace = texts[0];
                TextMeshProUGUI textPseudo = texts[1];
                TextMeshProUGUI textWins = texts[2];

                textPlace.text = i.ToString();
                textPseudo.text = player.pseudo;
                textWins.text = player.wins.ToString();

                textPlayer.transform.SetParent(scoreContainer.transform);
                textPlayer.transform.localScale = Vector3.one;
            }
        }
    }

    public ScoreType FoundPlayerInList(string pseudo)
    {
        ScoreType foundPlayer = players.Find(i => i.pseudo == pseudo);

        if (foundPlayer != null) return foundPlayer;
        else return null;
    }

    public bool CreatePlayer(string pseudo, int win)
    {
        int playerCount = players.Count;

        if (FoundPlayerInList(pseudo) == null)
        {
            players.Add(new ScoreType { ID = playerCount + 1, pseudo = pseudo, wins = win });

            return true;
        }
        else return false;
    }

    public void AddScore(string pseudo, int score = 1)
    {
        ScoreType player = FoundPlayerInList(pseudo);

        if (player != null) player.wins += score;
        else CreatePlayer(pseudo, score);
    }

    public List<ScoreType> GetLeaderboardList()
    {
        List<ScoreType> sortedPlayerList = players.OrderByDescending(player => player.wins).ToList();

        return sortedPlayerList;
    }

    void SetInPlayerPref()
    {

    }
}
