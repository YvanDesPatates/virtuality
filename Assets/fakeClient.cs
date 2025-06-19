using System;

public class fakeClient : ElixirIsReadySubscriber
{
    public ClientPlaceToTakeElixir clientPlace;

    private void Start()
    {
        clientPlace.Subscribe(this);
    }

    public override void OnElixirIsReady()
    {
        clientPlace.TakeElixir(IngredientType.Elixir1);
        Destroy(gameObject);
    }

    public override void OnElixirIsNotReady()
    {
        //
    }
}
