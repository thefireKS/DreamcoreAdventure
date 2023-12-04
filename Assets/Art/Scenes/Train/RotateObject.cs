using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 5f; // Скорость вращения
    public float rotationAmplitude = 45f; // Амплитуда вращения

    private Vector3 targetRotation;

    void Start()
    {
        // Генерация случайного начального положения
        targetRotation = new Vector3(Random.Range(-rotationAmplitude, rotationAmplitude),
                                     Random.Range(-rotationAmplitude, rotationAmplitude),
                                     Random.Range(-rotationAmplitude, rotationAmplitude));
    }

    void Update()
    {
        // Плавное приближение к целевому вращению
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), rotationSpeed * Time.deltaTime);

        // Если достигли целевого вращения, генерируем новое случайное положение
        if (Quaternion.Angle(transform.rotation, Quaternion.Euler(targetRotation)) < 0.1f)
        {
            targetRotation = new Vector3(Random.Range(-rotationAmplitude, rotationAmplitude),
                                         Random.Range(-rotationAmplitude, rotationAmplitude),
                                         Random.Range(-rotationAmplitude, rotationAmplitude));
        }
    }
}