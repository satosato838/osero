using UnityEngine;
using UnityEngine.UI;

public class OseroTitleView : MonoBehaviour
{
    [SerializeField] OseroView _oseroView;
    [SerializeField] Button _gameStartbtn;
    [SerializeField] Button _onlinebtn;
    [SerializeField] private GameObject _view;
    void Start()
    {
        _gameStartbtn.onClick.AddListener(() =>
        {
            _oseroView.GameStart();
            Hide();

        });
        //todoかえる
        _onlinebtn.onClick.AddListener(() => { _oseroView.GameStart(); });
    }

    public void Show()
    {
        this._view.SetActive(true);
    }
    public void Hide()
    {
        this._view.SetActive(false);
    }

}
