using UnityEngine;

public class FloatingOrigin : MonoBehaviour
{
    public float threshold = 1000.0f;
    public float physicsThreshold = 10000.0f;
    public float defaultSleepThreshold = 0.14f;
    private ParticleSystem.Particle[] parts = null;

    private void LateUpdate()
    {
        Vector3 cameraPosition = gameObject.transform.position;
        if (cameraPosition.magnitude > threshold)
        {
            Object[] objects = FindObjectsOfType<Transform>();
            foreach (Object o in objects)
            {
                Transform t = (Transform)o;
                if (t.parent == null)
                {
                    t.position -= cameraPosition;
                }
            }
            objects = FindObjectsOfType<ParticleSystem>();
            foreach (Object o in objects)
            {
                ParticleSystem sys = (ParticleSystem)o;
                if (sys.main.simulationSpace != ParticleSystemSimulationSpace.World)
                {
                    continue;
                }

                int particlesNeeded = sys.main.maxParticles;
                if (particlesNeeded <= 0)
                {
                    continue;
                }

                bool wasPaused = sys.isPaused;
                bool wasPlaying = sys.isPlaying;
                if (!wasPaused)
                {
                    sys.Pause();
                }

                if (parts == null || parts.Length < particlesNeeded)
                {
                    parts = new ParticleSystem.Particle[particlesNeeded];
                }
                int num = sys.GetParticles(parts);
                for (int i = 0; i < num; i++)
                {
                    parts[i].position -= cameraPosition;
                }
                sys.SetParticles(parts, num);
                if (wasPlaying)
                {
                    sys.Play();
                }
            }
            objects = FindObjectsOfType<TrailRenderer>();
            foreach (Object o in objects)
            {
                TrailRenderer trail = (TrailRenderer)o;
                trail.Clear();
            }
            objects = FindObjectsOfType(typeof(LineRenderer));
            foreach (Object o in objects)
            {
                LineRenderer line = (LineRenderer)o;
                Vector3[] positions = new Vector3[line.positionCount];
                line.GetPositions(positions);
                for (int i = 0; i < positions.Length; ++i)
                {
                    positions[i] -= cameraPosition;
                }

                line.SetPositions(positions);
            }
            if (physicsThreshold > 0f)
            {
                float physicsThreshold2 = physicsThreshold * physicsThreshold;
                objects = FindObjectsOfType(typeof(Rigidbody));
                foreach (Object o in objects)
                {
                    Rigidbody r = (Rigidbody)o;
                    if (r.gameObject.transform.position.sqrMagnitude > physicsThreshold2)
                    {
                        r.sleepThreshold = float.MaxValue;
                    }
                    else
                    {
                        r.sleepThreshold = defaultSleepThreshold;
                    }
                }
            }
            // print ("Moved origin");
        }
    }
}