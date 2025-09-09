
public class Player
{
    public string Name { get; private set; }
    public Player()
    {
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void Play()
    {
        UnityEngine.Debug.Log("Player : " + Name);
    }
}