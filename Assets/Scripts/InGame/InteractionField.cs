using UnityEngine;

namespace InGame
{
    public class InteractionField : MonoBehaviour 
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Animator animator))
                return;
            
            animator.SetTrigger("Attack");
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Animator animator))
                return;
            
            animator.SetTrigger("Attack");
        }
    }
}