[System.Serializable]
public class ActionData<T>
{
    public T payload;
    public string actionName;
}
[System.Serializable]
public class PayloadData{
    public string sku { get; set; }
    public int quantity { get; set; }
}