using LANMatching;
using UnityEngine;
using UnityEngine.UI;

public class OnlineView : MonoBehaviour
{
    [SerializeField] private OseroTitleView _titleView;
    [SerializeField] private Button _createRoomBtn;
    [SerializeField] private Button _searchRoomBtn;
    [SerializeField] private Button _exitOnlineBtn;
    [SerializeField] private GameObject _view;

    void Start()
    {
        Hide();
        _createRoomBtn.onClick.AddListener(() => { CreateRoom(); });
        _searchRoomBtn.onClick.AddListener(() => { SearchRoom(); });
        _exitOnlineBtn.onClick.AddListener(() => { ExitOnline(); });
    }

    public void CreateRoom()
    {
        byte limitUser = 2; // <- 最大参加人数
        int serverPort = 8888; // <- 実際のサーバー接続時に待っているポート番号
        var roomInfo = new RoomInfo("初心者歓迎ルーム", serverPort, limitUser);
        // Hostルームの設定をします
        LANRoomManager.Instance.hostRoomInfo = roomInfo;
        // 募集を開始します
        LANRoomManager.Instance.StartHostThread();
    }
    public void SearchRoom()
    {
        LANRoomManager.Instance.OnFindNewRoom = (hostRoomInfo) =>
        {
            Debug.Log("新規ルームが見つかりました：" + hostRoomInfo.roomInfo.name);
        };
        LANRoomManager.Instance.OnChangeRoom = (hostRoomInfo) =>
        {
            Debug.Log("情報が変更されました:" + hostRoomInfo.roomInfo.name);
        };
        LANRoomManager.Instance.OnLoseRoom = (hostRoomInfo) =>
        {
            Debug.Log("ルームが閉じました:" + hostRoomInfo.roomInfo.name);
        };
        LANRoomManager.Instance.StartClientThread();
    }

    public void ExitOnline()
    {
        LANRoomManager.Instance.OnFindNewRoom = null;
        LANRoomManager.Instance.OnChangeRoom = null;
        LANRoomManager.Instance.OnLoseRoom = null;
        LANRoomManager.Instance.Stop();
        Hide();
        _titleView.Show();
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
