using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BackgroundParticleSystem : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private ParticleSystem system;

    private void Awake()
    {
        system = GetComponent<ParticleSystem>();
        var shape = system.shape;
        shape.shapeType = ParticleSystemShapeType.Rectangle;

        float cameraWidth = cam.orthographicSize * cam.aspect * 2;
        int count = 3;
        shape.scale = new Vector3(cameraWidth * count, cam.orthographicSize * 2f, 1);

        shape.position = new Vector3(shape.scale.x - cameraWidth*(count-1), 0, 0);
    }
}
