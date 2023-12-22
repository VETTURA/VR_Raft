using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RockGenerator : MonoBehaviour
{
    private System.Random rnd = new();

    private const float MIN_Z_DISTANCE = -400.0f;
    private const float MAX_Z_DISTANCE = -1900.0f;
    private const float MIN_X_DISTANCE = -1000.0f;
    private const float MAX_X_DISTANCE = 1000.0f;

    List<GameObject> rocksPrefab;
    List<GameObject> rocksObject = new List<GameObject>();
    List<Position> rockPositions = new List<Position>();

    void Start()
    {
        GetPrefabs();
        GetPosition();
        SpawnRock();
    }

    private void GetPrefabs()
    {
        rocksPrefab = new List<GameObject> 
        {
            (GameObject)Resources.Load("Prefabs/Level/Rocks/Rock_1", typeof(GameObject)),
            (GameObject)Resources.Load("Prefabs/Level/Rocks/Rock_2", typeof(GameObject)),
            (GameObject)Resources.Load("Prefabs/Level/Rocks/Rock_3", typeof(GameObject))
        };
    }

    private void GetPosition()
    {
        int countParallels = rnd.Next(5, 15);
        float zStep = (MAX_Z_DISTANCE - MIN_Z_DISTANCE) / countParallels;

        int countMeridians = rnd.Next(50, 100);
        float xStep = (MAX_X_DISTANCE - MIN_X_DISTANCE) / countMeridians;

        for(int i = 0; i < countParallels; i++)
        {
            for(int j = 0; j < countMeridians; j++)
            {
                float probability = (float)rnd.NextDouble();

                if(probability > 0.6)
                {
                    float zDeviation = rnd.Next(Math.Abs((int)zStep) * -100, Math.Abs((int)zStep) * 100) / 100;
                    float xDeviation = rnd.Next(Math.Abs((int)xStep) * -100, Math.Abs((int)xStep) * 100) / 100;

                    float zPosition = i * zStep + zDeviation + MIN_Z_DISTANCE;
                    float xPosition = j * xStep + xDeviation + MIN_X_DISTANCE;

                    float yDeviation = rnd.Next(-500, 0) / 100;

                    float rotation = rnd.Next(100, 36000) / 100;

                    float scale = rnd.Next(150, 500) / 100;

                    rockPositions.Add(new Position(xPosition, yDeviation, zPosition, rotation, scale));
                }
            }
        }
    }

    private void SpawnRock()
    {
        foreach(var rock in rockPositions) 
        {
            int element = rnd.Next(0, rocksPrefab.Count + 1);

            GameObject gameObject = Instantiate(
                rocksPrefab[element], 
                new Vector3(rock.xPosition, rock.yPosition, rock.zPosition), 
                Quaternion.identity);

            gameObject.transform.Rotate(0, rock.rotate, 0);
            gameObject.transform.localScale = new Vector3(rock.scale, rock.scale, rock.scale);

            rocksObject.Add(gameObject);
        }
    }
}

class Position
{
    public float xPosition { get; set; }
    public float yPosition { get; set; }
    public float zPosition { get; set; }
    public float rotate { get; set; }
    public float scale { get; set; }

    public Position(float xPosition, float yPosition, float zPosition, float rotate, float scale)
    {
        this.xPosition = xPosition;
        this.zPosition = zPosition;
        this.yPosition = yPosition;
        this.rotate = rotate;
        this.scale = scale;
    }
}
