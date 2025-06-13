using UnityEngine;

public abstract class ElixirIsReadySubscriber : MonoBehaviour
{
    /// <summary>
    /// Called when the elixir is ready.
    /// </summary>
    public abstract void OnElixirIsReady();

    /// <summary>
    /// Called when the elixir is not ready anymore.
    /// </summary>
    public abstract void OnElixirIsNotReady();
}