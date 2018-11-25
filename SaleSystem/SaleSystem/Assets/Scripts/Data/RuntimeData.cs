using System.Collections.Generic;

namespace Sale
{
    public class Room
    {
        public int Index;
        public string Name;
        public Queue<RoomRecord> Records = new Queue<RoomRecord>();

        public Room(int index)
        {
            Index = index;
        }

        public void AddRecord(RoomRecord roomRecord)
        {
            Records.Enqueue(roomRecord);
        }
    }

}