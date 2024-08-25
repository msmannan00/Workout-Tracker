using System;
using System.Collections.Generic;

public interface PageController
{
    void onInit(Dictionary<string, object> data, Action <object> callback);
}
