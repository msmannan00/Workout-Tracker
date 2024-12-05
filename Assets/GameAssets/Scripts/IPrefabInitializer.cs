using System.Collections.Generic;
using System;

public interface IPrefabInitializer
{
    void InitPrefab(Action<List<object>> onFinish, List<object> data);
}
