using UnityEngine;

namespace Autditorium
{
    public class SpawnerParticle : MonoBehaviour
    {
        #region Public Variable

        public GameObject m_particule;
        public float m_timeBetweenParticle;
        public float m_unitCircleMultiplier;
        public float m_timeToDestroyParticle;

        #endregion


        #region Api Unity

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
                Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * m_unitCircleMultiplier;
                GameObject particle = Instantiate(m_particule, spawnPos, Quaternion.identity);

                Destroy(particle, m_timeToDestroyParticle);
            }
        }

        #endregion


        #region Private and Protected

        private float _elapsedTime;

        #endregion
    }
}
