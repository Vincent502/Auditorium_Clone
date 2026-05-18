using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject m_particule;
    public float m_timeBetweenParticle;
    public float m_unitCircleMultiplier;
    private void Start()
    {
        _elapsedTime = 0f;
    }

    private void Update()
    {
        if (m_particule == null) return;
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= m_timeBetweenParticle)
        {
            _elapsedTime = 0f;
            GameObject particle = Instantiate(m_particule, Random.insideUnitCircle * m_unitCircleMultiplier, Quaternion.identity);
            Destroy(particle, 2f);
        }
    }

    private float _elapsedTime;

}
