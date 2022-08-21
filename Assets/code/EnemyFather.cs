using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using VariablesSaver = Unity.VisualScripting.VariablesSaver;

public class EnemyFather : MonoBehaviour
{
    // 怪物的血量
    public int life;

    // 怪物的伤害
    public int damage;
    private BoxCollider2D _boxCollider2D;

    protected Animator _animator;

    // 判断是否在受伤害
    protected bool isHurt;

    // 流血对象， 外部引入
    public GameObject blood;

    // ai相关
    // 移动
    public float speed;
    public float moveWaitTime;
    private float _waitTime;
    public Transform movePos;

    [HideInInspector] public Transform _leftDownPos;
    [HideInInspector] public Transform _rightUpPos;
    [HideInInspector] public string currentName;
    [HideInInspector] public float limtLeftX;
    [HideInInspector] public float limtRightX;
    [HideInInspector] public float limtY;
    

    // public GameObject ParentPos;
    // [HideInInspector]
    // // 水平偏移量
    // public Transform levelOffsetPos;


    // Start is called before the first frame update
    public void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();

        // Debug.Log(_leftDownPos.position + " -- " + _rightUpPos.position);


        // 敌人刚创建的时候随机一个目标位置
        movePos.position = MoveRandPos();
        // 移动完后原地等多久
        _waitTime = moveWaitTime;
    }

    // Update is called once per frame
    public void Update()
    {
        myDestroy();
        if (life > 0)
        {
            // 有生命的时候才能调用这些方法
            hurtAnimator();
            // 到达目标后的下一步动作行为
            reachAction();
        }
    }

    protected virtual void myDestroy()
    {
        if (life <= 0)
        {
            // 放个动画消失
            Destroy(gameObject);
        }
    }


    // 受伤动画
    protected virtual void hurtAnimator()
    {
    }

    // 怪物受伤
    public void BeHurt(int damage)
    {
        if (life > 0)
        {
            Instantiate(blood, transform.position + new Vector3(0, 1), Quaternion.identity);
            // 受伤动画
            isHurt = true;
            // 伤害计算
            life -= damage;
        }
    }


    public virtual void reachAction()
    {
        // 移动
        transform.position = Vector2.MoveTowards(transform.position, movePos.position, speed * Time.deltaTime);
        // 是否到达目标
        if (Vector3.Distance(movePos.position, transform.position) < 0.1f)
        {
            if (_waitTime <= Mathf.Epsilon)
            {
                // 重置数据
                _waitTime = moveWaitTime;
                movePos.position = MoveRandPos();
            }

            Run(false);
            _waitTime -= Time.deltaTime;
        }
        else
        {
            // 还没到
            Run(true);
            Flip(movePos.position.x < transform.position.x);
        }
    }


    // 随机生成移动去的位置
    protected virtual Vector3 MoveRandPos()
    {
        // var y = GameObject.FindWithTag("FirstFlow").transform.position.y;
        Vector3 randPos = new Vector3(Random.Range(_leftDownPos.position.x, _rightUpPos.position.x),
            _leftDownPos.position.y);
        // Debug.Log(position.x + " -- " + position1.x);

        return randPos;
    }


    private void Flip(bool position)
    {
        if (position)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            // transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            // 反转180°
            gameObject.GetComponent<SpriteRenderer>().flipX = true;

            // 这个反转后会变扁，很奇怪..
            // transform.rotation = quaternion.Euler(0, 180, 0);
        }
    }


    protected virtual void Run(bool isMove)
    {
        // 按下键盘时才会获取到值
        // Debug.Log(Input.GetAxis("Horizontal"));
        // -1 到 1 之间好像
        // var isMove = Input.GetAxis("Horizontal") != 0;

        _animator.SetBool("run", isMove);
    }
}