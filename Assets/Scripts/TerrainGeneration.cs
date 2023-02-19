using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

public class TerrainGeneration : MonoBehaviour {
    [SerializeField] SpriteShapeController shape;
    [SerializeField] private int terrainLength = 100;
    [SerializeField] private float terrainHeight = 10f;
    [SerializeField] private float heightOffest = 10f;

    [SerializeField] private int distanceBetweenPoints = 1;
    [Tooltip("Set to -1 for random seed.")]
    [SerializeField] int seed;

    private void Start() {
        shape.spline.Clear();
        if (seed != -1) {
            Random.InitState(seed);
        }

        GenerateTerrainPerlinAsDerivative();
    }

    void GenerateTerrainPerlinAsDerivative() {
        shape.spline.InsertPointAt(0, Vector3.down * terrainHeight);
        shape.spline.InsertPointAt(1, Vector3.zero);

        var initX = shape.spline.GetPosition(1).x;
        var prevY = shape.spline.GetPosition(1).y;
        float lastDropXPos = 0;
        for (int i = 0; i < terrainLength / distanceBetweenPoints; i++) {
            float xPos = initX + (i + 1) * distanceBetweenPoints;
            float noiseVal = Mathf.PerlinNoise(i * Random.Range(5f, 15f), 0);
            noiseVal = -Mathf.Clamp01(noiseVal);
            var dropHeight = Random.Range(0, terrainHeight);
            float yPos = noiseVal * dropHeight + prevY;

            Vector3 leftTan = Vector3.left * distanceBetweenPoints / 3;
            Vector3 rightTan = Vector3.right * distanceBetweenPoints / 3;
            if (dropHeight > 3 * terrainHeight / 4 && xPos - lastDropXPos > distanceBetweenPoints * 6) {
                var lastLeftTan = leftTan + Vector3.down * distanceBetweenPoints / 5;
                shape.spline.SetLeftTangent((i - 1) + 2, lastLeftTan);
                shape.spline.SetRightTangent((i - 1) + 2, lastLeftTan * -3);
                shape.spline.SetPosition((i - 1) + 2,
                    new Vector3(initX + i * distanceBetweenPoints, prevY + 0.5f * terrainHeight));
                yPos += dropHeight * noiseVal;
                lastDropXPos = xPos;
            }

            shape.spline.InsertPointAt(i + 2, new Vector3(xPos, yPos));
            shape.spline.SetTangentMode(i + 2, ShapeTangentMode.Continuous);

            shape.spline.SetLeftTangent(i + 2, leftTan);
            shape.spline.SetRightTangent(i + 2, rightTan);
            prevY = yPos;
        }

        var lastIdx = terrainLength / distanceBetweenPoints + 1;
        shape.spline.SetTangentMode(0, ShapeTangentMode.Linear);
        shape.spline.SetPosition(0, Vector3.down * Mathf.Abs(prevY));
        shape.spline.InsertPointAt(lastIdx + 1,
            Vector3.right * terrainLength + Vector3.down * (Mathf.Abs(prevY) + terrainHeight));
    }

    void GenerateTerrainUsingPerlin() {
        shape.spline.InsertPointAt(0, Vector3.down * terrainHeight);
        // shape.spline.InsertPointAt(1, Vector3.zero);
        // shape.spline.InsertPointAt(2, Vector3.right * terrainLength);
        // shape.spline.InsertPointAt(3, Vector3.right * terrainLength + Vector3.down * terrainHeight);

        var initPos = shape.spline.GetPosition(0);
        for (int i = 0; i <= (terrainLength / distanceBetweenPoints); i++) {
            float xPos = initPos.x + i * distanceBetweenPoints;
            float noiseVal = Mathf.PerlinNoise(i * Random.Range(5f, 15f), 0);
            noiseVal = Mathf.Clamp01(noiseVal);
            float yPos = noiseVal * terrainHeight + heightOffest;
            shape.spline.InsertPointAt(i + 1, new Vector3(xPos, yPos));
            shape.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            shape.spline.SetLeftTangent(i, Vector3.left * distanceBetweenPoints / 2);
            shape.spline.SetRightTangent(i, Vector3.right * distanceBetweenPoints / 2);
        }

        var lastIdx = terrainLength / distanceBetweenPoints + 1;
        // shape.spline.InsertPointAt(lastIdx + 1, Vector3.zero);
        shape.spline.SetTangentMode(0, ShapeTangentMode.Linear);
        shape.spline.InsertPointAt(lastIdx + 1, Vector3.right * terrainLength + Vector3.down * terrainHeight);
    }


    void Update() {
    }
}