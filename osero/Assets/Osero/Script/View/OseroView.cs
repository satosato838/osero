using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.UI;

public class OseroView : MonoBehaviourPunCallbacks
{
    public const int BoardSize = 8;
    [SerializeField] private OseroCellView OseroCellPrefab;
    [SerializeField] private Button _Skipbtn;
    [SerializeField] private GameObject _Skip;
    [SerializeField] private Transform OseroCellParent;
    [SerializeField] private OseroGameScoreView _oseroGameScoreView;
    [SerializeField] private OseroGameResult _oseroGameResultView;
    [SerializeField] private GameObject _view;
    private Osero _osero;
    private Osero.PlayerTurn MyTurn;
    private Color ClearColor => new Color(1, 1, 1, 0);
    private bool IsShowSkip => _Skip.activeSelf;
    private List<List<OseroCellView>> _AllCells = new List<List<OseroCellView>>();
    void Start()
    {
        _Skipbtn.onClick.AddListener(() =>
        {
            _osero.Skip();
            Refresh();
        });
#if UNITY_EDITOR
        //debug only
        _Skipbtn.gameObject.SetActive(false);
#endif
        for (int y = 0; y < BoardSize; y++)
        {
            var cells = new List<OseroCellView>();
            for (int x = 0; x < BoardSize; x++)
            {
                var cell = Instantiate(OseroCellPrefab, OseroCellParent);
                cell.name = $"Cell({y},{x})";
                cell.Init((y, x), (yx) =>
                {
                    PlaceDisk(yx);
                });
                cells.Add(cell);
            }
            _AllCells.Add(cells);
        }
        Hide();
    }

    public void GameStart()
    {
        Show();
        _osero = new Osero(() => ShowSkipEffect());
        _oseroGameResultView.Hide();
        MyTurn = PhotonNetwork.CurrentRoom.GetPlayer(1).IsMasterClient ? Osero.PlayerTurn.Black : Osero.PlayerTurn.White;
        SkipEffectHide();
        Refresh();
    }
    private Osero.PlayerTurn GetPlayerTurn(int turnId)
    {
        return turnId == 1 ? Osero.PlayerTurn.Black : Osero.PlayerTurn.White;
    }
    public void PlaceDisk((int, int) pos)
    {
        if (GetPlayerTurn(PhotonNetwork.CurrentRoom.GetTurn()) != MyTurn) return;
        if (IsShowSkip) return;
        _osero.PlaceDisk(pos);
        Refresh();
        photonView.RPC(nameof(OpponentPlaceDisk), RpcTarget.All, new int[] { pos.Item1, pos.Item2 });
        PhotonNetwork.CurrentRoom.SetTurn(PhotonNetwork.CurrentRoom.GetTurn() == 1 ? 2 : 1);
    }

    [PunRPC]
    private void OpponentPlaceDisk(int[] messages)
    {
        _osero.PlaceDisk((messages[0], messages[1]));
        Refresh();
    }

    public void Refresh()
    {
        var disks = _osero.BoardDisks;
        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                if (disks[y][x].IsDot)
                {
                    _AllCells[y][x].SetDot(_osero.CurrentTurnDiskColor == Osero.PlayerTurn.Black ? Color.black : Color.white);
                }
                else
                {
                    _AllCells[y][x].SetDot(ClearColor);
                }
                _AllCells[y][x].SetDisk(disks[y][x].DiskState switch
                {
                    Osero.DiskState.None => ClearColor,
                    Osero.DiskState.Black => Color.black,
                    Osero.DiskState.White => Color.white,
                    _ => ClearColor
                });
            }
            PhotonNetwork.CurrentRoom.SetTurn(_osero.CurrentTurnDiskColor == Osero.PlayerTurn.Black ? 1 : 2);
        }
        _oseroGameScoreView.Refresh(_osero.GetWhiteDiskCount, _osero.GetBlackDiskCount);
        if (_osero.IsGameEnd())
        {
            _oseroGameResultView.Show(_osero.GameResult, _osero.GameResult switch
            {
                Osero.Result.None => ClearColor,
                Osero.Result.Draw => ClearColor,
                Osero.Result.BlackWin => Color.black,
                Osero.Result.WhiteWin => Color.white,
            });
        }
    }
    private void ShowSkipEffect()
    {
        _Skip.gameObject.SetActive(true);
        Invoke(nameof(SkipEffectHide), 2.0f);
    }
    private void SkipEffectHide()
    {
        _Skip.gameObject.SetActive(false);
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
