using System;
using System.Collections.Generic;

public interface ItemController
{
    void onInit(Dictionary<string, object> data, Action<object> callback);
}
