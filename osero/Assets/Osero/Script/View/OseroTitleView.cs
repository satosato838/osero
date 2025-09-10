using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class OseroTitleView : MonoBehaviour
{
    [SerializeField] OseroView _oseroView;
    [SerializeField] OnlineView _onlineView;
    [SerializeField] Button _gameStartbtn;
    [SerializeField] Button _onlinebtn;
    [SerializeField] private GameObject _view;
    void Start()
    {
        PhotonNetwork.Disconnect();
        _gameStartbtn.onClick.AddListener(() =>
        {
            _oseroView.GameStart();
            Hide();

        });

        _onlinebtn.onClick.AddListener(() =>
        {
            _onlineView.Show();
            Hide();
        });
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
