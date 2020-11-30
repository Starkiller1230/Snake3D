using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private Food _foodPrefab;
    [SerializeField, Min(0)] private float _spawnWidth;
    [SerializeField, Min(0)] private float _spawnHeight;
    [SerializeField, Min(0)] private float _spawnDelay;
    [SerializeField, Min(1)] private int _maxFood;

    private float _lastTimeSpawn;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_spawnWidth, 1, _spawnHeight));
    }

    private void SpawnFood(int currentFood)
    {
        float _x = Random.Range(transform.position.x - (_spawnWidth / 2), transform.position.x + (_spawnWidth / 2));
        float _z = Random.Range(transform.position.z - (_spawnHeight / 2), transform.position.z + (_spawnHeight / 2));
        float _y = transform.position.y;
        Instantiate(_foodPrefab.gameObject, new Vector3(_x, _y, _z), _foodPrefab.transform.rotation);

        _lastTimeSpawn = Time.time + _spawnDelay;
    }

    private void FixedUpdate()
    {
        if (Food.FoodCount < _maxFood && _lastTimeSpawn <= Time.time)
            SpawnFood(Food.FoodCount);

    }

}
