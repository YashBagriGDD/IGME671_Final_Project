using UnityEngine;

namespace Player
{ 
    [RequireComponent(typeof(Rigidbody2D) )]
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Animator animator;
        [SerializeField] private float timeBetweenStep;
        [FMODUnity.EventRef] public string fmodEvent;

        private float timeSinceLastStep = 0f;

        public bool Active
        {
            get => _active;
            set
            {
                if(!value) ChangeVelocity(Vector2.zero);
                _active = value;
            }
        }

        private bool _active;
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _velocity;
        private static readonly int XSpeed = Animator.StringToHash("x_speed");
        private static readonly int YSpeed = Animator.StringToHash("y_speed");


        private void Awake()
        {
            Active = true;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Update()
        {
            timeSinceLastStep += Time.deltaTime;
            if (!Active) { return; }
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");

            if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
                if(timeSinceLastStep > timeBetweenStep) {
                    FMODUnity.RuntimeManager.PlayOneShot(fmodEvent, GetComponent<Transform>().position);
                    timeSinceLastStep = 0;
                }
            }
        
            ChangeVelocity(new Vector2(x, y) * speed);

           _spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
        }

        private void FixedUpdate()
        {
            _rigidbody2D.position += _velocity * Time.fixedDeltaTime;
        }

        private void ChangeVelocity(Vector2 newVelocity)
        {
            _velocity = newVelocity;
            var y = _velocity.y;
            var x = _velocity.x;
            animator.SetFloat(YSpeed, y);
            animator.SetFloat(XSpeed, x);
        }
    }
}
