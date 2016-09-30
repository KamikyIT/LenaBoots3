using UnityEngine;

namespace Assets.scenes.Lena
{
    public class LenaMoves : MonoBehaviour
    {
        public float Speed = 1f;

        public LayerMask UnmovableObjectsLayerMask;

        private Rigidbody2D rb;
        // Use this for initialization
        void Start ()
        {
            rb = GetComponent<Rigidbody2D>();

        }


        void Update()
        {
            
            

        }
        // Update is called once per frame
        void FixedUpdate ()
        {
            if (Input.GetKey(KeyCode.A))
            {
                dx = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dx = 1f;
            }
            else
            {
                dx = 0f;
            }

            if (Input.GetKey(KeyCode.W))
            {
                dy = 1f;
            }
            else
            if (Input.GetKey(KeyCode.S))
            {
                dy = -1f;
            }
            else
            {
                dy = 0f;
            }


            rb.velocity = new Vector2(dx, dy) * Speed;


            return;

            Moves();

        }

        public float dx;
        public float dy;

        private void Moves()
        {

            var pnow = (Vector2)transform.position;

            var ds = new Vector2(dx, dy);

            if (ds.magnitude > 1f)
            {
                ds.Normalize();
            }

            var s = ds * Speed;

            Debug.DrawLine(pnow, pnow + s, Color.red, 1f);

            #region Linecasts

            // По диагонали.
            var lc = Physics2D.Linecast(pnow, pnow + s, UnmovableObjectsLayerMask);

            if (lc.collider == null)
            {
                rb.velocity = s;
                //transform.Translate((Vector3) s , Space.World);
                return;
            }

            // По горизонтали.
            lc = Physics2D.Linecast(pnow, pnow + new Vector2(s.x, 0f), UnmovableObjectsLayerMask);
            if (lc.collider == null)
            {
                rb.velocity = new Vector2(0f, s.y);
                //transform.Translate(new Vector3(0f, s.y), Space.World);
                return;
            }

            // По вертикали.
            lc = Physics2D.Linecast(pnow, pnow + new Vector2(0f, s.y), UnmovableObjectsLayerMask);
            if (lc.collider == null)
            {
                rb.velocity = new Vector2(s.x, 0f);
                //transform.Translate(new Vector3(s.x, 0f) , Space.World);
                return;
            }

            rb.velocity = new Vector2();

            #endregion

        }
    }
}
