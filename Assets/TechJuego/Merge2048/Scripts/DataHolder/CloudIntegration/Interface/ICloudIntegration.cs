using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICloudIntegration 
{
    void Login(Action OnComplete);
}
