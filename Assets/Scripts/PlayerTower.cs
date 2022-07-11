using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTower : MonoBehaviour
{
    [SerializeField] private Human _startHuman;
    [SerializeField] private Transform _distanceChecker;
    [SerializeField] private float _fixationMaxDistance;
    [SerializeField] private BoxCollider _checkCollider;
    private List<Human> _humans;

    public List<Human> Humans { get => _humans;}
    public Transform DistanceChecker { get => _distanceChecker; set => _distanceChecker = value; }
    public BoxCollider CheckCollider { get => _checkCollider; set => _checkCollider = value; }

    public event UnityAction<int> HumanAdded;
    private void Start()
    {
        _humans = new List<Human>();
        Vector3 spawnPoint = transform.position;
        _humans.Add(Instantiate(_startHuman, spawnPoint, Quaternion.identity, transform));
        if (_humans[0].gameObject.TryGetComponent(out Animator CharacterAnimator) == true)
        {
            CharacterAnimator.SetInteger("Motion", 7);
        }
        HumanAdded?.Invoke(_humans.Count);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Human human))
        {
            
            Tower collisionTower = human.GetComponentInParent<Tower>();

            if (collisionTower != null)
            {
                List<Human> collectedHumans = collisionTower.CollectHuman(_distanceChecker, _fixationMaxDistance);
                if (collectedHumans != null)
                {
                    for (int i = collectedHumans.Count - 1; i >= 0; i--)
                    {
                        Human insertHuman = collectedHumans[i];
                        InsertHuman(insertHuman);
                        DisplaceCheckers(insertHuman);
                    }

                }
                SetAnimationToHumans();
                collisionTower.Break();
            }
        }
    }
    public void SetAnimationToHumans()
    {
        HumanAdded?.Invoke(_humans.Count);
        int i = 0;
        foreach (var human in _humans)
        {
            if (i>0)
            {
                if (human.gameObject.TryGetComponent(out Animator animator) == true)
                {
                    animator.SetInteger("Motion", Random.Range(1, 6));
                }
            }
            else
            {
                if (human.gameObject.TryGetComponent(out Animator animator) == true)
                {
                    animator.SetInteger("Motion", 7);
                }
            }
            i++;
        }
    }
    private void InsertHuman(Human collectedHuman)
    {
        _humans.Insert(0, collectedHuman);
        SetHumanPosition(collectedHuman);
    }
    private void SetHumanPosition(Human human)
    {
        human.transform.parent = transform;
        human.transform.localPosition = new Vector3(0, human.transform.localPosition.y, 0);
        human.transform.localRotation = Quaternion.identity;
    }
    private void DisplaceCheckers(Human human)
    {
        float DisplaceScale = 1.3f;
        Vector3 distanceCheckerNewPosition = _distanceChecker.position;
        distanceCheckerNewPosition.y -= human.transform.localScale.y * DisplaceScale;
        _distanceChecker.position = distanceCheckerNewPosition;
        _checkCollider.center = _distanceChecker.localPosition;
    }
}
