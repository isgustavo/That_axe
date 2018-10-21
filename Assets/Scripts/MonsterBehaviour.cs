using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class MonsterBehaviour : MonoBehaviour, ISpawnebleObject
{
    protected NavMeshAgent agent;
    protected Animator animator;
    protected Transform player;

    protected Renderer rend;

    [SerializeField]
    private GameObject monsterHud;

    public void OnCollisionEnterEvent()
    {
        agent.isStopped = true;
        animator.SetTrigger("GetHit");
    }

    public void OnCollisionExitEvent()
    {
        monsterHud.SetActive(false);
        StartCoroutine(DissolverCoroutine()); 
    }

    protected IEnumerator DissolverCoroutine()
    {
        float sliceAmount = 0;
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
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        rend = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    protected void OnEnable()
    {

        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                player = obj.transform;
            }
        }

        monsterHud.SetActive(true);
        rend.material.SetFloat("_SliceAmount", 0);

        agent.SetDestination(player.position);

        //animator.SetTrigger("Walk");
    }

    private void Start()
    {
        //Debug.Log("START");
       // agent.SetDestination(player.position);

        //animator.SetTrigger("Walk");
    }

}
