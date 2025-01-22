using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSingleton : Singleton<BaseSingleton>
{
    // attach this script to any gameobject that you want to persist between scenes
    // used mainly for Manager parented obj with manager children
}
