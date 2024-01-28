using PitchPerfect.Core;
using UnityEngine;
using UnityEngine.UI;

namespace PitchPerfect.UI
{
    public class UITablet : MonoBehaviour
    {
        private static readonly float[] LEVELS_Y =
        {
            -350f,
            -180f,
            0f,
            180f,
            350f
        };
        
        private static readonly float[] LEVELS_X =
        {
            -500f,
            -250f,
            0f,
            250f,
            500f
        };

        private static readonly float[] LINES_OFFSET =
        {
            0f,
            10f,
            -10f,
            20f,
            -20f
        };
        
        [SerializeField] private GameObject _voteTab;
        [SerializeField] private Image _voteButtonBg;
        [Space]
        [SerializeField] private GameObject _trendTab;
        [SerializeField] private Image _trendButtonBg;
        [SerializeField] private LineRenderer[] _trendsLines;
        [SerializeField] private Image[] _trendsIcons;

        private void Start()
        {
            MatchDataManager.Instance.OnTrendsUpdated += OnTrendsUpdated;
            MatchDataManager.Instance.OnPlayerSelectedCardUpdated += OnPlayerSelectedUpdated;
            
            SwitchTab(true);
        }

        private void OnDestroy()
        {
            MatchDataManager.Instance.OnTrendsUpdated -= OnTrendsUpdated;
            MatchDataManager.Instance.OnPlayerSelectedCardUpdated -= OnPlayerSelectedUpdated;
        }

        private void SwitchTab(bool isTrendTab)
        {
            _trendTab.SetActive(isTrendTab);
            _trendButtonBg.enabled = isTrendTab;
            _voteTab.SetActive(!isTrendTab);
            _voteButtonBg.enabled = !isTrendTab;

            if (isTrendTab)
            {
                SetupTrends(true);
            }
        }

        private void OnTrendsUpdated()
        {
            var trends = MatchDataManager.Instance.CategoryTrends;
            
            
        }

        private void OnPlayerSelectedUpdated()
        {
            SwitchTab(false);
        }

        private void SetupTrends(bool fakeTrends = false)
        {
            if (fakeTrends)
            {
                int lineId = 0;
                foreach (var line in _trendsLines)
                {
                    for (int i = 0; i < LEVELS_X.Length; i++)
                    {
                        var x = LEVELS_X[i];
                        var y = LEVELS_Y[Random.Range(0, LEVELS_Y.Length)];
                        
                        line.SetPosition(i, new Vector3(x, y + LINES_OFFSET[lineId], 0f));
                    }

                    lineId++;
                }
            }
        }
        
        public void OnGreenlightClick()
        {
            
        }

        public void OnApprovedClick()
        {
            MatchDataManager.Instance.VoteCurrentSelection(true);
        }

        public void OnDeniedClick()
        {
            MatchDataManager.Instance.VoteCurrentSelection(false);
        }
    }
}