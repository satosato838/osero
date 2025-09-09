using UnityEngine;

public class HogeHogeCCC
{
    // 依存注入の対象として宣言する
    private HogeHogeAAA _hogeHogeAAA = null;

    // 宣言しない
    [Inject]
    private HogeHogeBBB _hogeHogeBBB = null;

    [Inject]
    private Player _player = null;

    public void Log(string str)
    {
        if (_hogeHogeAAA == null)
        {
            Debug.Log($"{str} _hogeHogeAAA; は nullです。");
        }
        else
        {
            Debug.Log($"{str} _hogeHogeAAA は DIに成功しました。");
        }

        if (_hogeHogeBBB == null)
        {
            Debug.Log($"{str} _hogeHogeBBB は nullです。");
        }
        else
        {
            Debug.Log($"{str} _hogeHogeBBB は DIに成功しました。");
        }

        if (_player != null)
        {
            _player.SetName("TARO");
            Debug.Log($"{_player.Name}");
        }
    }
}