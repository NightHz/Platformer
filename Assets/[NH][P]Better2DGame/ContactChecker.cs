using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContactChecker : MonoBehaviour
{
    public GameObject player;

    [Header("Parameter")]
    [Range(0, 0.5f)] public float leaveTimeThreshold = 0.12f;
    float leaveTime = 0;

    [Header("Event")]
    public UnityEvent OnContact = new UnityEvent();

    public bool IsContacted { get; private set; } = false;

    Collider2D trigger;
    private void Awake()
    {
        trigger = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.gameObject != player)
        {
            if (!IsContacted && leaveTime >= leaveTimeThreshold)
                // 真接触
                OnContact.Invoke();
            IsContacted = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;

        // 检查是否还有正在接触的碰撞箱
        List<Collider2D> colliders = new List<Collider2D>();
        trigger.GetContacts(colliders);
        foreach (Collider2D collider in colliders)
        {
            if (!collider.isTrigger && collider.gameObject != player)
            {
                // 有
                return;
            }
        }
        // 没有
        IsContacted = false;
        leaveTime = 0;
    }
    private void FixedUpdate()
    {
        leaveTime += Time.fixedDeltaTime;
    }
}
