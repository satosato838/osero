using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnlineView : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    [SerializeField] private OseroTitleView _titleView;
    [SerializeField] private OseroView _oseroView;
    [SerializeField] private TextMeshProUGUI _roomMembers;
    [SerializeField] private Button _createRoomBtn;

    [SerializeField] private Button _exitOnlineBtn;
    [SerializeField] private TMP_InputField _ipt_PlayerName;
    [SerializeField] private GameObject _view;

    void Start()
    {
        Hide();

        _createRoomBtn.onClick.AddListener(() =>
        {
            PhotonNetwork.NickName = _ipt_PlayerName.text;
            // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
            PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
        });
        _exitOnlineBtn.onClick.AddListener(() => { ExitOnline(); });
    }

    private void RefreshPlayers()
    {
        _roomMembers.text = "";
        foreach (var item in PhotonNetwork.CurrentRoom.Players)
        {
            _roomMembers.text += item.Value.NickName + '\n';
        }
    }
    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var item in roomList)
        {
            Debug.Log("OnRoomListUpdate:" + item.Name);
        }
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        RefreshPlayers();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"OnJoinRandomFailed:" + message);
    }
    public void ExitOnline()
    {
        PhotonNetwork.Disconnect();
        Hide();
        _titleView.Show();
    }

    public void Show()
    {
        PhotonNetwork.ConnectUsingSettings();
        this._view.SetActive(true);
    }
    public void Hide()
    {
        this._view.SetActive(false);
    }
    void IInRoomCallbacks.OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom");
        RefreshPlayers();
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            photonView.RPC(nameof(MatchStart), RpcTarget.All, "MatchStart");
        }
        photonView.RPC(nameof(RpcSendMessage), RpcTarget.All, "OnPlayerEnteredRoom");
    }

    [PunRPC]
    private void RpcSendMessage(string message)
    {
        Debug.Log("RpcSendMessage:" + message);
    }

    [PunRPC]
    private void MatchStart(string message)
    {
        Debug.Log("MatchStart:" + message);
        _oseroView.GameStart();
        Hide();
    }
}
