using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Vector2 _humanInTowerRange;
    [SerializeField] private Human[] _humansTempales;
    [SerializeField] private Trampoline[] _trampolineTempales;
    private int _humanInTowerCount;
    public Vector3 _pointForTrampoline;
    private List<Human> _humanInTower;
    [SerializeField] private float _explosionForce;
    [SerializeField] private Vector3 _explosionPosition;
    [SerializeField] private float _radius;

    private void Start()
    {
        _humanInTower = new List<Human>();
        _humanInTowerCount = Convert.ToInt32(UnityEngine.Random.Range(_humanInTowerRange.x, _humanInTowerRange.y));
        SpawnTrampoline(_pointForTrampoline);
        SpawnHumans(_humanInTowerCount);
        
    }
    private void SpawnHumans(int humanCount)
    {
        Vector3 spawnPoint = transform.position;
        
        for (int i = 0; i < humanCount; i++)
        {
            Human spawnedHuman = _humansTempales[UnityEngine.Random.Range(0, _humansTempales.Length)];

            _humanInTower.Add(Instantiate(spawnedHuman, spawnPoint, new Quaternion(0,0,0,0),transform));
            _humanInTower[i].transform.localPosition = new Vector3(0, _humanInTower[i].transform.localPosition.y, 0);
            spawnPoint = _humanInTower[i].FixationPoint.position;
        }
        
    }
    public void SpawnTrampoline(Vector3 spawnPosition)
    {
        spawnPosition.y = _trampolineTempales[_humanInTowerCount - 1].transform.position.y;
        Instantiate(_trampolineTempales[_humanInTowerCount - 1], spawnPosition, Quaternion.identity, transform);
    }
    public List<Human> CollectHuman(Transform distanceChecker, float fixationMaxDistance)
    {
        for (int i = 0; i < _humanInTower.Count; i++)
        {
            float distanceBetweenPoints = CheckDistanceY(distanceChecker, _humanInTower[i].FixationPoint);
            if (distanceBetweenPoints < fixationMaxDistance)
            {
                List<Human> collectedHumans = _humanInTower.GetRange(0, i + 1);
                _humanInTower.RemoveRange(0, i + 1);
                return collectedHumans;
            }
        }
        return null;
    }
    public float CheckDistanceY(Transform distanceChecker, Transform humanFixationPoint)
    {
        Vector3 distanceCheckerY = new Vector3(0, distanceChecker.position.y, 0);
        Vector3 humanFixationPointY = new Vector3(0, humanFixationPoint.position.y, 0);
        return Vector3.Distance(distanceCheckerY, humanFixationPointY);
    }
    public void Break()
    {
        foreach (Human human in _humanInTower)
        {
            human.transform.parent = null;
            human.gameObject.AddComponent<Rigidbody>();
            Rigidbody rigidbody = human.gameObject.GetComponent<Rigidbody>();
            rigidbody.freezeRotation = true;
            human.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            human.gameObject.GetComponent<Animator>().enabled = false;
            _explosionPosition = new Vector3(UnityEngine.Random.Range(-1, 1), -1, UnityEngine.Random.Range(-1, 1));
            rigidbody.AddExplosionForce(_explosionForce, human.transform.position + _explosionPosition, _radius);

        }
        _humanInTower = new List<Human>();
    }
}

