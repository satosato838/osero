using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class OseroView : MonoBehaviourPunCallbacks
{
    public const int BoardSize = 8;
    [SerializeField] private OseroCellView OseroCellPrefab;
    [SerializeField] private GameObject _Skip;
    [SerializeField] private Transform OseroCellParent;
    [SerializeField] private TextMeshProUGUI _currentPlayerName;
    [SerializeField] private OseroGameScoreView _oseroGameScoreView;
    [SerializeField] private OseroGameResult _oseroGameResultView;
    [SerializeField] private GameObject _view;
    private Osero _osero;
    private Osero.PlayerTurn MyTurn;
    private bool IsMyTurn => MyTurn == _osero.CurrentTurnDiskColor;
    private Color ClearColor => new Color(1, 1, 1, 0);
    private bool IsShowSkip => _Skip.activeSelf;
    private List<List<OseroCellView>> _AllCells = new List<List<OseroCellView>>();
    void Start()
    {
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
        _osero = new Osero(CreatePlayers(), () => ShowSkipEffect());
        _oseroGameResultView.Hide();
        MyTurn = _osero.GetIdPlayerTurn(GetMyId());
        SkipEffectHide();
        Refresh();
        SoundManager.Instance.PlaySE(SESoundData.SE.gamestart);
        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.main);
    }

    private int GetMyId()
    {
        return PhotonNetwork.CurrentRoom.Players.First(p => p.Value.NickName.Equals(PhotonNetwork.LocalPlayer.NickName)).Value.ActorNumber;
    }

    private List<GamePlayer> CreatePlayers()
    {
        List<GamePlayer> players = new List<GamePlayer>();

        foreach (var item in PhotonNetwork.CurrentRoom.Players)
        {
            GamePlayer player = new GamePlayer(item.Value.ActorNumber, item.Value.NickName, GetPlayerTurn(item.Key));
            players.Add(player);
        }
        return players;
    }

    private Osero.PlayerTurn GetPlayerTurn(int turnId)
    {
        return turnId == 1 ? Osero.PlayerTurn.Black : Osero.PlayerTurn.White;
    }
    public void PlaceDisk((int, int) pos)
    {
        if (!IsMyTurn || IsShowSkip) return;
        _osero.PlaceDisk(pos);
        Refresh();
        photonView.RPC(nameof(OpponentPlaceDisk), RpcTarget.All, new int[] { pos.Item1, pos.Item2 });
        SoundManager.Instance.PlaySE(SESoundData.SE.diskpush);
    }

    [PunRPC]
    private void OpponentPlaceDisk(int[] messages)
    {
        _osero.PlaceDisk((messages[0], messages[1]));
        Refresh();
        SoundManager.Instance.PlaySE(SESoundData.SE.diskpush);
    }

    public void Refresh()
    {
        _currentPlayerName.text = "Turn " + _osero.CurrentGamePlayer.Name;
        var disks = _osero.BoardDisks;
        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                if (IsMyTurn)
                {
                    if (disks[y][x].IsDot)
                    {
                        _AllCells[y][x].SetDot(_osero.CurrentTurnDiskColor == Osero.PlayerTurn.Black ? Color.black : Color.white);
                    }
                    else
                    {
                        _AllCells[y][x].SetDot(ClearColor);
                    }
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
        SoundManager.Instance.PlaySE(SESoundData.SE.skip);
        Invoke(nameof(SkipEffectHide), 1.0f);
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
