using NaughtyAttributes;
using UnityEngine;

namespace Core.Managers.Animators
{
    /// <summary>
    /// Simple class for an Animator with two <see cref="bool"/> parameters:
    /// <list type="bullet">
    /// <item>Active - true if Active Animation is/was played</item>
    /// <item>NotActive - true if NotActive Animation is/was played</item>
    /// </list>
    /// </summary>
    public class SimpleAnimatorControler : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        [Required]
        private Animator _animator;

        public bool IsActive { get => active; }
        private bool active;
        #endregion

        #region Public Methods

        /// <summary>
        /// Sets Active of Animator to true, and NotActive to false
        /// </summary>
        [Button]
        public void Activate() => SetAnimator(Active: true, NotActive: false);
        /// <summary>
        /// Sets Active of Animator to false, and NotActive to true
        /// </summary>
        [Button]
        public void Deactivate() => SetAnimator(Active: false, NotActive: true);

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets Animator's two parameters to the given values
        /// </summary>
        /// <param name="Active">State of "Active" parameter</param>
        /// <param name="NotActive">State of "NotActive" parameter</param>
        private void SetAnimator(bool Active, bool NotActive)
        {
            if (_animator == null)
                return;
            active = Active;
            _animator.SetBool(nameof(Active), Active);
            _animator.SetBool(nameof(NotActive), NotActive);
        }

        #endregion
    }
}