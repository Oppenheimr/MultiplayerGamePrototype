using Photon.Pun;
using UI.InGame;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 5f; // Hareket hızı
        private Vector3 _targetPosition; // Hedef pozisyon
        private bool _isMoving; // Hareket durumu

        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private PhotonView _view;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _view = GetComponent<PhotonView>();
            
            if (!_view.IsMine)
                enabled = false;
        }

        private void Update()
        {
            if (!_view.IsMine)
                return;
            // Vector3 motion = _navMeshAgent.desiredVelocity.normalized;
            // _animator.SetFloat("Speed", motion.magnitude);
            
            // NavMeshAgent'ın hızını Animator'a ilet
            float speed = _navMeshAgent.velocity.magnitude;
            _animator.SetFloat("Speed", speed);
            
            // Fare sol tuşuna basıldığında
            if (Input.GetMouseButtonDown(0))
            {
                // Fare pozisyonunu dünya koordinatlarına çevir
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // NavMesh Agent'ı tıklanan pozisyona yönlendir
                    _navMeshAgent.SetDestination(hit.point);
                }
            }
        }

        public void OnAttack()
        {
            if (!_view.IsMine)
                return;
            
            WoodCounter.Instance.OnWoodChanged?.Invoke(5);
        }
    }
}