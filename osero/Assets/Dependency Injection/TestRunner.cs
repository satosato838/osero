using UnityEngine;

public class TestRunner : MonoBehaviour
{
    private void Start()
    {
        // 登録
        DIContainer.Instance.Register<HogeHogeAAA>(DIContainer.Lifetime.Singleton);
        DIContainer.Instance.Register<HogeHogeBBB>(DIContainer.Lifetime.Singleton);
        DIContainer.Instance.Register<Player>(DIContainer.Lifetime.Singleton);

        var ccc = new HogeHogeCCC();

        ccc.Log("1 >>>");
        Debug.Log("依存注入する");
        DIContainer.Instance.Inject(ccc);
        ccc.Log("2 >>>");
        DIContainer.Instance.Unregister<Player>();


    }
}

