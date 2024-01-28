using TMPro;
using UnityEngine;

namespace PitchPerfect.UI
{
    public class UILeaderboardRecord : MonoBehaviour
    {
        [SerializeField] private TMP_Text _username;
        [SerializeField] private TMP_Text _boardScore;
        [SerializeField] private TMP_Text _publishersScore;
        [SerializeField] private TMP_Text _budgetsScore;
        [SerializeField] private TMP_Text _totalScore;

        public void Setup(string username, int boardScore, int publishersScore, int budgetsScore, int totalScore)
        {
            _username.text = username;
            _boardScore.text = boardScore.ToString();
            _publishersScore.text = publishersScore.ToString();
            _budgetsScore.text = budgetsScore.ToString();
            _totalScore.text = totalScore.ToString();
        }
    }
}