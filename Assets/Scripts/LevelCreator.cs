using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private Tower _towerTemplate;
    [SerializeField] private Obstacle _obstacle;
    [SerializeField] private int _humanTowerCount;
    
    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        float roadLength = _pathCreator.path.length;
        float distanceBetwennTower = roadLength / _humanTowerCount;

        float distanceTraveled = 0;
        Vector3 spawnPoint;

        for (int i = 0; i < _humanTowerCount - 1; i++)
        {
            distanceTraveled += distanceBetwennTower;
                spawnPoint = _pathCreator.path.GetPointAtDistance(distanceTraveled, EndOfPathInstruction.Stop);
            if (Random.Range(0, 2) == 0)
            {
                
                Vector3 pointForTrampoline = _pathCreator.path.GetPointAtDistance(distanceTraveled - 3f);
                _towerTemplate._pointForTrampoline = pointForTrampoline;
                Instantiate(_towerTemplate, spawnPoint, Quaternion.identity).transform.LookAt(_pathCreator.path.GetPointAtDistance(distanceTraveled - 1));
            }
            else
            {
                Instantiate(_obstacle, spawnPoint, Quaternion.identity).transform.LookAt(_pathCreator.path.GetPointAtDistance(distanceTraveled - 1));
                
            }
        }
        
    }
}
