using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ClientController : ElixirIsReadySubscriber
{
    public float speed = 1.5f;
    [FormerlySerializedAs("bubblePositionExeptTarget")] public Transform bubblePositionExceptY;
    public GameObject bubblePrefab;
    
    private ClientPlaceToTakeElixir placeToTakeElixir;
    private Transform[] pathTargets;
    private Transform[] arrivalPathTargets;
    private Transform[] departurePathTargets;
    private int currentTargetIndex = 0;
    private Animator animator;

    private IngredientType _elixirToAskFor;
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
        _elixirToAskFor = RandomElixirPicker.GetRandomElixirIndex();
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
                    HasReachedTheBar();
                }
            }
        }
    }

    private void TakeElixir()
    {
        placeToTakeElixir.TakeElixir(_elixirToAskFor);
        pathTargets = departurePathTargets;
        currentTargetIndex = 0;
        _isAtTheBar = false;
        if (animator is not null) animator.SetTrigger("Walk");
        Destroy(bubblePrefab);
    }

    private void HasReachedTheBar()
    {
        _hasLeavedTheBar = true;
        _isAtTheBar = true;
        StartCoroutine(SpawnBubble(3f));
    }

    private IEnumerator SpawnBubble(float delayInSeconds)
    {
        bubblePrefab = Instantiate(bubblePrefab, Vector3.zero, Quaternion.identity);
        BubblePutElixirImage bubble = bubblePrefab.GetComponent<BubblePutElixirImage>();
        bubble.SetElixirImage(_elixirToAskFor);
        bubblePrefab.SetActive(false);
        var yPosition = transform.position.y + GetComponent<Collider>().bounds.size.y + bubblePrefab.transform.localScale.y/5;
        var position = new Vector3(bubblePositionExceptY.position.x, yPosition, bubblePositionExceptY.position.z);
        bubblePrefab.transform.position = position;
        bubblePrefab.transform.SetParent(bubblePositionExceptY);
        yield return new WaitForSeconds(delayInSeconds);
        bubblePrefab.SetActive(true);
    }
}
    