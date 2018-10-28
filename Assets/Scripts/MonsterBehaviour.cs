using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent (typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class MonsterBehaviour : MonoBehaviour, ISpawnebleObject
{
    protected NavMeshAgent agent;
    protected Animator animator;
    protected Transform player;

    protected Renderer rend;
    protected AudioSource audioSource;

    protected bool isAttack = false;

    [SerializeField]
    private GameObject monsterHud;
    [SerializeField]
    private UnityEvent OnStartWalkingEvent;
    [SerializeField]
    private UnityEvent OnMonsterHitted;
    [SerializeField]
    private UnityEvent OnDissolveEvent;
    [SerializeField]
    private UnityEvent OnAttackEvent;
    [SerializeField]
    private UnityEvent OnEndGameConditionEvent;

    public void OnCollisionEnterEvent()
    {
        if (!isAttack)
        {
            agent.isStopped = true;
            audioSource.Stop();
            animator.SetTrigger("GetHit");
        }
    }

    public void OnCollisionExitEvent()
    {
        if (!isAttack)
        {
            monsterHud.SetActive(false);
            OnMonsterHitted.Invoke();
            StartCoroutine(DissolverCoroutine());
        }
    }

    protected IEnumerator DissolverCoroutine()
    {
        float sliceAmount = 0;
        OnDissolveEvent.Invoke();
        while (sliceAmount <= 1)
        {
            sliceAmount += Time.deltaTime;
            rend.material.SetFloat("_SliceAmount", sliceAmount);
            yield return new WaitForEndOfFrame();
        }

        gameObject.SetActive(false);
    }

    public bool IsActiveInHierarchy()
    {
        return gameObject.activeInHierarchy;
    }

    public void SetStartPosition(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);

        agent.SetDestination(player.position);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    protected void OnEnable()
    {
        if (player == null)
        {
            GameObject obj = Camera.main.gameObject;
            if (obj == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                player = obj.transform;
            }
        }

        isAttack = false;
        monsterHud.SetActive(true);
        rend.material.SetFloat("_SliceAmount", 0);

        OnStartWalkingEvent.Invoke();
    }

    protected void Update()
    {
        if (!agent.pathPending && !isAttack)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    isAttack = true;
                    animator.SetTrigger("Attack");
                    OnAttackEvent.Invoke();
                    StartCoroutine(LazyEndGameCoroutine());
                }
            }
        }
    }

    protected IEnumerator LazyEndGameCoroutine()
    {
        yield return new WaitForSeconds(1f);
        OnEndGameConditionEvent.Invoke();
    }

    public void DestroyMonster()
    {
        monsterHud.SetActive(false);
        StartCoroutine(DissolverCoroutine());
    }
}
