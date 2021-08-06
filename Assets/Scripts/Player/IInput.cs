using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInput 
{
    float Horizontal {get; }
    float Vertical {get; }

    float MouseX {get; }
    float MouseY {get; }

    float Jump {get; }

    bool Interact {get; }
    bool TapShoot {get; }
    bool HoldShoot {get; }
    bool ExitShoot {get; }
    bool TapScope {get; }
    bool Scope {get; }
    bool ExitScope {get; }
    bool Relaod {get; }
    bool Drop {get; }
    bool Alpha1 {get; }
    bool Alpha2 {get; }
    bool Alpha3 {get; }

    Vector2 lookRotate{get; }

    InputMaster playerInputMaster {get; }

    public void ReadInput();
}
