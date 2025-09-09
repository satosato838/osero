using TMPro;
using UnityEngine;

public class OseroGameScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txt_whiteDiskCount;
    [SerializeField] private TextMeshProUGUI _txt_blackDiskCount;

    public void Refresh(int whiteCount, int blackCount)
    {
        _txt_whiteDiskCount.text = whiteCount.ToString();
        _txt_blackDiskCount.text = blackCount.ToString();
    }

}
