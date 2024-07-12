using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinMovementPlanet : MonoBehaviour
{
  // Geschwindigkeit der Bewegung
  public float speed = 1.0f;

  // Skalierung des Perlin Noise
  public float noiseScale = 1.0f;

  // Startpositionen
  private Vector3 startPos;

  void Start()
  {
      // Speichere die Startposition
      startPos = transform.position;
  }

  void Update()
  {
      // Berechne die neuen Positionen basierend auf Perlin Noise
      float x = startPos.x + Mathf.PerlinNoise(Time.time * speed, 0.0f) * noiseScale;
      float y = startPos.y + Mathf.PerlinNoise(0.0f, Time.time * speed) * noiseScale;
      float z = startPos.z + Mathf.PerlinNoise(Time.time * speed, Time.time * speed) * noiseScale;

      // Setze die neue Position
      transform.position = new Vector3(x, y, z);
  }
}
