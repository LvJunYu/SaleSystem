namespace MyTools
{
    public static class EMessengerType
    {
        private static int _nextId;
        public static readonly int ShowDialog = _nextId++;
        public static readonly int OnRoomChanged = _nextId++;
        public static readonly int OnRoomRecordChanged = _nextId++;
        public static readonly int OnPayInfoChanged = _nextId++;
    }
}