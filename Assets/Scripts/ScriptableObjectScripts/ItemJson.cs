[System.Serializable]
public class ItemJson
{
    public string Name;
    public int ID;
    public string Description;
    public int Value;
}

public class ItemJsonList
{
    public ItemJson[] items;
}