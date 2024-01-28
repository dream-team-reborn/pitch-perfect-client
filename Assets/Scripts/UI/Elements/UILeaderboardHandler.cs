using UnityEngine;

namespace PitchPerfect.UI
{
    public class UILeaderboardHandler : MonoBehaviour
    {
        [SerializeField] private UILeaderboardRecord _recordPrefab;
        [SerializeField] private Transform _recordsContainer;

        public void PopulateLeaderboard()
        {
            var record = Instantiate(_recordPrefab, _recordsContainer);
            record.Setup("Mauro", 10, 15, 10, 35);
        }
    }
}