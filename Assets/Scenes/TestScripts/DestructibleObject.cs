using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IHittable
{
    DestructibleObjectPart baseObject;
    [SerializeField] DestructibleObjectPart[] ObjectStages;
    int currentPart = 0;
    public float healthPoints { get; set; }
    public HealthManager _healthManager { get; set; }
    public float nextThreshold;
    void Start()
    {
        ObjectStages = GetComponentsInChildren<DestructibleObjectPart>();
        baseObject = ObjectStages[0];
        nextThreshold = baseObject.breakingPointInPercents;
        baseObject.ToggleBreakPoint();
        _healthManager = GetComponent<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    [SerializeField] float currentPercent;
    public void ProcessHit(float damage)
    {
        _healthManager.ProcessHit(damage);
        currentPercent = (_healthManager.currentHealth / _healthManager.maxHealth) * 100;
        if (currentPercent <= nextThreshold && currentPart < ObjectStages.Length - 1)
        {
            NextDamagePoint();
        }
        if (_healthManager.IsDead)
        {

        }
    }

    void NextDamagePoint()
    {
        ObjectStages[currentPart].ToggleBreakPoint();
        ObjectStages[currentPart + 1].ToggleBreakPoint();
        nextThreshold = ObjectStages[currentPart + 1].breakingPointInPercents;
        currentPart++;

    }

}
