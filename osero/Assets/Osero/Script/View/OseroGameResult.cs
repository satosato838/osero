using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OseroGameResult : MonoBehaviour
{
    [SerializeField] OseroView _oseroView;
    [SerializeField] OseroTitleView _oseroTitleView;
    [SerializeField] Button _retrybtn;
    [SerializeField] Button _titlebtn;
    [SerializeField] private GameObject _view;
    [SerializeField] Image _resultColor;
    [SerializeField] TextMeshProUGUI _txt_result;
    void Start()
    {
        Hide();
        _retrybtn.onClick.AddListener(() =>
        {
            _oseroView.GameStart();
            Hide();
        });
        _titlebtn.onClick.AddListener(() =>
        {
            _oseroView.Hide();
            _oseroTitleView.Show();
        });
    }

    public void Show(Osero.Result result, Color winnerColor)
    {
        this._view.SetActive(true);

        _resultColor.gameObject.SetActive(result == Osero.Result.BlackWin || result == Osero.Result.WhiteWin);
        _txt_result.text = result == Osero.Result.BlackWin || result == Osero.Result.WhiteWin ? "WIN" : "DRAW";
        _resultColor.color = winnerColor;
    }
    public void Hide()
    {
        this._view.SetActive(false);
    }
}
