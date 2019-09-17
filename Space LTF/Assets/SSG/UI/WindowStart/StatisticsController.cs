using UnityEngine;
using System.Collections;
using TMPro;

public class StatisticsController : MonoBehaviour
{
    private bool _isInited;
    public Transform Layout;
    public StatisticResultElement PrefabResultElement;
    public StatisticResultElement LastResultElement;
    public TextMeshProUGUI NoResults;
    public TextMeshProUGUI LastResultResults;
    public TextMeshProUGUI NoLastResult;
    public void Init()
    {
        var stats = MainController.Instance.Statistics.EndGameStatistics;
        if (!_isInited)
        {
            NoResults.text = Namings.StatisticNoResult;
            NoLastResult.text = Namings.StatisticNoLastResult;
            LastResultResults.text = Namings.StatisticLastResult;
            _isInited = true;
            stats.OnAddResult += OnAddResult;
            DrawCurrent(stats);
            UpdateCurResult();
        }
    }

    private void DrawCurrent(EndGameStatistics stats)
    {
        var haveStats = stats.AllResults.Count > 0;
        if (haveStats)
        {
            NoResults.gameObject.SetActive(false);
            foreach (var result in stats.AllResults)
            {
                CreateResult(result);
            }
        }
        else
        {
            NoResults.gameObject.SetActive(true);
        }

    }

    private void CreateResult(EndGameResult result)
    {
        var element = DataBaseController.GetItem(PrefabResultElement);
        element.transform.SetParent(Layout,false);
        element.Init(result);
    }

    private void OnAddResult(EndGameResult result)
    {
        CreateResult(result);
        UpdateCurResult();
        NoResults.gameObject.SetActive(false);
    }

    private void UpdateCurResult()
    {
        var lastResult = MainController.Instance.Statistics.EndGameStatistics.LastResult;
        var haveResult = lastResult != null;
        NoLastResult.gameObject.SetActive(!haveResult);
        LastResultElement.gameObject.SetActive(haveResult);
        if (haveResult)
            LastResultElement.Init(lastResult);
    }

    public void Dispose()
    {
//        MainController.Instance.Statistics.EndGameStatistics.OnAddResult -= OnAddResult;
    }
}
