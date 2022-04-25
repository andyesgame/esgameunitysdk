using System;
[Serializable]
public class ESResponse<T>
{
    public int code;
    public string message;
    public T data;
}
