using System.Collections.Generic;
using Sale;

public class SaleConstDefine
{
    public const int DefaultRecordHour = 12;
    public const int DefaultRoomPrice = 300;
    private static List<string> _playTypes;

    public static List<string> PayTypes
    {
        get
        {
            if (_playTypes == null)
            {
                _playTypes = new List<string>((int) EPayType.Max);
                for (int i = 0; i < (int) EPayType.Max; i++)
                {
                    _playTypes.Add(((EPayType) i).ToString());
                }
            }

            return _playTypes;
        }
    }
}