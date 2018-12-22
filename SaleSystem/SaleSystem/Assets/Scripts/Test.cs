using System;
using MyTools;
using Sale;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        var date = DateTime.Now;
        var days = date.GetDays();
        LogHelper.Info("months:{0}", days);
        var data = DateTimeHelper.GetDateTime(days);
        LogHelper.Info("date:{0}", data);
        LogHelper.Info("data:{0}", DateTimeHelper.GetDateTime(new DateTime(2018, 12, 22, 12, 0, 0).GetDays()) == data);
    }
}