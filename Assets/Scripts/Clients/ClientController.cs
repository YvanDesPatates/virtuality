using UnityEngine;

public class ClientController : ElixirIsReadySubscriber
{
    public float speed = 1.5f;
    
    private ClientPlaceToTakeElixir placeToTakeElixir;
    private Transform[] pathTargets;
    private Transform[] arrivalPathTargets;
    private Transform[] departurePathTargets;
    private int currentTargetIndex = 0;
    private Animator animator;
    
    private bool _isAtTheBar = false;
    private bool _hasLeavedTheBar = false;
    private bool _elixirIsReady = false;

    public void SetArrivalPathTargets(Transform[] targets)
    {
        arrivalPathTargets = targets;
        pathTargets = arrivalPathTargets;
        currentTargetIndex = 0;
    }

    public void SetDeparturePathTargets(Transform[] targets)
    {
        departurePathTargets = targets;
    }

    public void SetPlaceToTakeElixir(ClientPlaceToTakeElixir placeToTakeElixir)
    {
        this.placeToTakeElixir = placeToTakeElixir;
        placeToTakeElixir.Subscribe(this);
    }
    
    public override void OnElixirIsReady()
    {
        _elixirIsReady = true;
    }

    public override void OnElixirIsNotReady()
    {
        //
    }    
    
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Walk");
        }
    }
    void Update()
    {
        if (_isAtTheBar && _elixirIsReady)
        {
            TakeElixir();
        }
        
        if (pathTargets.Length == 0 || _isAtTheBar) return;
        

        Transform target = pathTargets[currentTargetIndex];

        // Position cible sur le plan XZ (même hauteur que l'objet)
        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;

        // Déplacement fluide sur le plan horizontal
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Direction sans tenir compte de la hauteur
        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // Ignore la différence de hauteur
        direction = direction.normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // Vérifie si le client est arrivé (en 3D, mais la distance reste précise)
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            currentTargetIndex++;
            if (currentTargetIndex >= pathTargets.Length)
            {
                if (animator != null) animator.SetTrigger("IdleBreak");

                if (_hasLeavedTheBar)
                {
                    Destroy(gameObject);
                }
                else
                {
                    _hasLeavedTheBar = true;
                    _isAtTheBar = true;
                }
            }
        }
    }

    private void TakeElixir()
    {
        placeToTakeElixir.TakeElixir(IngredientType.Elixir1);
        pathTargets = departurePathTargets;
        currentTargetIndex = 0;
        _isAtTheBar = false;
        if (animator is not null) animator.SetTrigger("Walk");
    }
}
