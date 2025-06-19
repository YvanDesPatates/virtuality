using System.Collections.Generic;

public class ClientPlaceIsFreeSingleton
{
    private static ClientPlacesIsFree _instance;
    
    /// <summary>
    /// Subscribe to the event that is triggered when a client place is free, WARNING: only one receiver can be registered at a time.
    /// </summary>
    public static void Subscribe(I_ClientPlaceIsFreeEventReceiver receiver)
    {
        GetInstance().SubscribeTo(receiver);
    }

    public static void OnClientPlaceIsFree(ClientPlaceToTakeElixir clientPlace)
    {
        GetInstance().OnNewClientPlaceIsFree(clientPlace);
    }

    private static ClientPlacesIsFree GetInstance()
    {
        if (_instance is null)
        {
            _instance = new ClientPlacesIsFree();
        }

        return _instance;
    }

    private class ClientPlacesIsFree
    {
        private I_ClientPlaceIsFreeEventReceiver _receiver;
        /// <summary>
        /// this is used only at the beginning of the game to know which client places are free.
        /// </summary>
        private readonly List<ClientPlaceToTakeElixir> _freeClientPlaces = new();
    
        /// <summary>
        /// Subscribe to the event that is triggered when a client place is free, WARNING: only one receiver can be registered at a time.
        /// </summary>
        public void SubscribeTo(I_ClientPlaceIsFreeEventReceiver receiver)
        {
            _receiver = receiver;
            if (_freeClientPlaces.Count > 0)
            {
                foreach (var clientPlace in _freeClientPlaces)
                {
                    _receiver.OnClientPlaceIsFree(clientPlace);
                }
                _freeClientPlaces.Clear(); // Clear the list after notifying the receiver
            }
        }

        public void OnNewClientPlaceIsFree(ClientPlaceToTakeElixir clientPlace)
        {
            if (_receiver is null)
            {
                _freeClientPlaces.Add(clientPlace);
                return;
            }
            _receiver.OnClientPlaceIsFree(clientPlace);
        }
    }
}