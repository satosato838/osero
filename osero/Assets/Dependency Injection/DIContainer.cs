using System;
using System.Collections.Generic;
using System.Reflection;

public class DIContainer
{
    public enum Lifetime
    {
        Singleton,  // 常に同じインスタンスを返す
        Transient   // 新しいインスタンスを生成して返す
    }

    private class Value
    {
        public Lifetime Lifetime { get; set; }
        public object Instance { get; set; }
        public Value(Lifetime lifetime, object instance)
        {
            Lifetime = lifetime;
            Instance = instance;
        }
    }

    private readonly Dictionary<Type, Value> _container = new Dictionary<Type, Value>();

    private readonly static DIContainer _instance = new DIContainer();
    public static DIContainer Instance => _instance;

    // T型からインスタンスの生成と登録を行う
    public void Register<T>(Lifetime lifetime) where T : class, new()
    {
        var type = typeof(T);
        if (_container.TryGetValue(type, out Value value))
        {
            if (value.Lifetime == Lifetime.Singleton)
                return;

            var ins = new T();
            value.Lifetime = lifetime;
            value.Instance = ins;
        }
        else
        {
            var ins = new T();
            _container.Add(type, new Value(lifetime, ins));
        }
    }

    // 既に生成されたインスタンスの登録を行う
    public void Register<T>(T ins, Lifetime lifetime)
    {
        var type = typeof(T);
        if (_container.TryGetValue(type, out Value value))
        {
            if (value.Lifetime == Lifetime.Singleton)
                return;

            value.Lifetime = lifetime;
            value.Instance = ins;
        }
        else
        {
            _container.Add(type, new Value(lifetime, ins));
        }
    }

    public void Unregister<T>() where T : class
    {
        _container.Remove(typeof(T));
    }

    public void UnregisterAll()
    {
        _container.Clear();
    }

    public T Resolve<T>() where T : class
    {
        if (_container.TryGetValue(typeof(T), out Value value))
        {
            return (T)value.Instance;
        }
        return default;
    }

    // 依存性注入
    public void Inject(object target)
    {
        // targetオブジェクトの全フィールドを取得
        var fields = target
            .GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var field in fields)
        {
            // [Inject]属性が付与されたフィールドのみ対象とする
            var injectAttribute = field.GetCustomAttributes(typeof(InjectAttribute), true);
            if (injectAttribute.Length <= 0)
                continue;

            if (!_container.TryGetValue(field.FieldType, out Value value))
                continue;

            // フィールドに依存注入
            field.SetValue(target, value.Instance);
        }
    }
}
