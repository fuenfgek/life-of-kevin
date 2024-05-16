namespace Sopra.Maps
{
    public sealed class Riddle
    {
        public int RiddleItem1 { get; }
        public string Keyword { get; }
        public string Type { get; }

        public Riddle(string keyword, int riddleItem1, string type)
        {
            RiddleItem1 = riddleItem1;
            Keyword = keyword;
            Type = type;
        }


    }
}
