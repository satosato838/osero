using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OseroTitleView : MonoBehaviourPunCallbacks
{
    [SerializeField] OseroView _oseroView;
    [SerializeField] OnlineView _onlineView;
    [SerializeField] TMP_InputField _ipt_PlayerName;
    [SerializeField] Button _onlinebtn;

    [SerializeField] private GameObject _modalview;
    [SerializeField] private Button _gameStartbtn;
    [SerializeField] private Button _closebtn;
    [SerializeField] private GameObject _view;
    void Start()
    {
        var username = Environment.UserName;
        _ipt_PlayerName.text = username;
        PhotonNetwork.Disconnect();
        _onlinebtn.onClick.AddListener(() =>
        {
            PhotonNetwork.NickName = GetPlayerName();
            //PhotonNetwork.GetCustomRoomList(TypedLobby.Default,);
            // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
            PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
        });

        _gameStartbtn.onClick.AddListener(() =>
        {
            PhotonNetwork.ConnectUsingSettings();
        });

        _closebtn.onClick.AddListener(() =>
        {
            PhotonNetwork.Disconnect();
            _modalview.gameObject.SetActive(false);
        });
        Show();
    }

    void Update()
    {
        _onlinebtn.enabled = !_ipt_PlayerName.text.Equals(string.Empty);
    }
    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        _modalview.gameObject.SetActive(true);
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        _onlineView.Show();
        Hide();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"OnJoinRandomFailed:" + message);
    }

    public string GetPlayerName() => _ipt_PlayerName.text;

    public void Show()
    {
        this._view.SetActive(true);
        _modalview.gameObject.SetActive(false);
    }
    public void Hide()
    {
        this._view.SetActive(false);
        _modalview.gameObject.SetActive(false);
    }

}
