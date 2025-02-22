using UnityEngine;

namespace Practice3
{
    public class Movement : MonoBehaviour
    {
        public CharacterController characterController;
        public Animator animator;

        public float speed = 12f;
        public float gravity = -9.8f;
        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        public float jumpHeight;

        Vector3 velocity;
        bool isGrounded;

        private void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if(x == 0 && z == 0)
            {
                animator.SetBool("Moving", false);
            }
            else
            {
                animator.SetBool("Moving", true);
            }

            Vector3 move = transform.right * x + transform.forward * z;
            characterController.Move(move * speed * Time.deltaTime);

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if(isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            else
            {
                //Здесь в методичке была ошибка, должно быть умножение
                velocity.y += gravity * Time.deltaTime;
                characterController.Move((velocity * Time.deltaTime) / 2f);
            }

            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                //В методичке снова ошибк
                //Не нужно брать никакие корни и прочее, у нас и так jumpForce
                //Лучше переименовать в jumpHeight, тогда физически будет верно
                velocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
        }
    }
}
