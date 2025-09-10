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
        _gameStartbtn.onClick.AddListener(() =>
        {
            _oseroView.GameStart();
            Hide();

        });

        _onlinebtn.onClick.AddListener(() =>
        {
            // PhotonNetwork.ConnectUsingSettings();
            // PhotonNetwork.Instantiate("NetworkedObject", Vector3.zero, Quaternion.identity);
            _onlineView.Show();
            Hide();
        });
    }

    // public void OnConnectedToMaster()
    // {
    //     // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
    //     PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    // }

    // public void OnJoinedRoom()
    // {
    //     // // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
    //     // var position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
    //     // PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
    // }
    public void Show()
    {
        this._view.SetActive(true);
    }
    public void Hide()
    {
        this._view.SetActive(false);
    }

}
