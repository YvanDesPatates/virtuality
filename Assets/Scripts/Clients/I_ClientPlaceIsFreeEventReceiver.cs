public interface I_ClientPlaceIsFreeEventReceiver
{
    /// <summary>
    /// Called when the client place became free.
    /// </summary>
    void OnClientPlaceIsFree(ClientPlaceToTakeElixir clientPlace);
}