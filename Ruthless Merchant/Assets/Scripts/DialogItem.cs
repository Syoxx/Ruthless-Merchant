namespace RuthlessMerchant
{
    public struct DialogItem
    {
        public int Id;
        public string Text;
        public string AudioFile;
        public int NextId;
        public DialogItem[] Answers;
        public ItemValue[] Rewards;
        public QuestItem[] NewQuests;
    }
}