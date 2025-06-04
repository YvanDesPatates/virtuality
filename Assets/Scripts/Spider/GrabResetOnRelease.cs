using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GrabResetOnRelease : XRGrabInteractable
{
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        transform.rotation = Quaternion.identity; // Reset rotation
    }
}

