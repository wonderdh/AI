class MapInfo
{
    public string[] id;
    public string[] checkedList;
    public string[] descriptionList;

    public MapInfo(string[] checkedList, string[] descriptionList)
    {
        this.checkedList = checkedList;
        this.descriptionList = descriptionList;

        this.id = new string[checkedList.Length];

        for (int i = 0; i < checkedList.Length; i++)
        {
            id[i] = i.ToString();
        }
    }
}