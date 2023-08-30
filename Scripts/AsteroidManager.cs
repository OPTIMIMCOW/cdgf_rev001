using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] Asteroid asteroid;
    [SerializeField] int numberOfAsteroidsOnAxis = 10;
    [SerializeField] int spacing = 10;
    float offset = 0.8f;

    private void Start()
    {
        PlaceAsteroids();
    }

    private void PlaceAsteroids()
    {
        for (int x = 0; x < numberOfAsteroidsOnAxis; x++)
        {
            for (int y = 0; y < numberOfAsteroidsOnAxis; y++)
            {
                for (int z = 0; z < numberOfAsteroidsOnAxis; z++)
                {
                    InstantiateAsteroid(x, y, z);
                }
            }
        }
    }

    private void InstantiateAsteroid(int x, int y, int z)
    {
        Instantiate(asteroid, new Vector3((x * spacing + AsteroidOffset()), (y * spacing + AsteroidOffset()), (z * spacing+ AsteroidOffset())), Quaternion.identity, transform);
    }

    private float AsteroidOffset()
    {
        return Random.Range(-offset, offset);
    }
}
