using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(HealthManager))]
public class TestDummy : MonoBehaviour, IHittable
{
    public float healthPoints { get; set; }
    public HealthManager _healthManager { get; set; }

    private void Awake()
    {
        _healthManager = GetComponent<HealthManager>();
    }
    public void ProcessHit(float damage)
    {
        _healthManager.ProcessHit(damage);
    }

    // Start is called before the first frame update
    IEnumerator ObserveHit()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        GetComponent<MeshRenderer>().material.color = Color.white;
    }

    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
