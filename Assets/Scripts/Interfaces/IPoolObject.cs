﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManagerEvets{
    public delegate void Events(IPoolObject _gameObject);
}

public enum State
{
    InPool,
    InUse,
    Destroying,
}

public interface IPoolObject {
    GameObject ownerObject { get; set; }
    GameObject gameObject { get; }
    State CurrentState
    {
        get;
        set;
    }

    void PoolInit();

    event PoolManagerEvets.Events OnObjectSpawn;
    event PoolManagerEvets.Events OnObjectDestroy;
}
