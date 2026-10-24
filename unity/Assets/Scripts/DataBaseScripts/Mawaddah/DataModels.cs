using System;
using System.Collections.Generic;

[Serializable]
public class FetchResponse
{
    public bool success;
    public List<ChildData> children;
    public string message;
}

[Serializable]
public class ChildData
{
    public int ChildID;
    public string Name;
    public string Avatar;
}
