using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class move : MonoBehaviour
{
    [SerializeField]
    List<GameObject> gameobjGroup = new List<GameObject>();
    [SerializeField]

    Vector3[] gameobjGroupBefor;
    public GameObject tailGameobject;
    public GameObject apple;
    int wallLayerMask;
    int tailLayerMask;
    int appleLayerMask;
    public bool canmove;
    // Start is called before the first frame update
    void Start()
    {
        gameobjGroupBefor = new Vector3[100];
        appleLayerMask = LayerMask.GetMask("Apple");
        wallLayerMask = LayerMask.GetMask("Wall");
        tailLayerMask = LayerMask.GetMask("Tail");
        Debug.Log(wallLayerMask);
        canmove = true;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (!gameobjGroup.Contains(this.transform.GetChild(i).gameObject))
            {
                gameobjGroup.Add(this.transform.GetChild(i).gameObject);
               
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Apple")
        {
            Destroy(collision.gameObject);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.transform.name);
    }
    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(
    new Vector3(gameobjGroup[0].transform.position.x, gameobjGroup[0].transform.position.y + 0.5f, gameobjGroup[0].transform.position.z),
    new Vector3(gameobjGroup[0].transform.position.x, gameobjGroup[0].transform.position.y + 0.5f + 1f, gameobjGroup[0].transform.position.z),
    Color.red
);
        Debug.DrawLine(
new Vector3(gameobjGroup[0].transform.position.x, gameobjGroup[0].transform.position.y - 0.5f, gameobjGroup[0].transform.position.z),
new Vector3(gameobjGroup[0].transform.position.x, gameobjGroup[0].transform.position.y - 0.5f - 1f, gameobjGroup[0].transform.position.z),
Color.green
);
        Debug.DrawLine(
new Vector3(gameobjGroup[0].transform.position.x + 0.5f, gameobjGroup[0].transform.position.y, gameobjGroup[0].transform.position.z),
new Vector3(gameobjGroup[0].transform.position.x + 0.5f + 1f, gameobjGroup[0].transform.position.y, gameobjGroup[0].transform.position.z),
Color.yellow
);
        Debug.DrawLine(
new Vector3(gameobjGroup[0].transform.position.x - 0.5f, gameobjGroup[0].transform.position.y, gameobjGroup[0].transform.position.z),
new Vector3(gameobjGroup[0].transform.position.x - 0.5f - 1f, gameobjGroup[0].transform.position.y, gameobjGroup[0].transform.position.z),
Color.blue
);

      



        if(checkDown(gameobjGroup[gameobjGroup.Count-1]))
        {
            for (int i = 1; i <= gameobjGroup.Count - 1; i++)
            {
               
                    gameobjGroup[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                
            }
            
                RaycastHit2D hitleft = Physics2D.Raycast(gameobjGroup[0].transform.position, Vector2.left, tailLayerMask);
                RaycastHit2D hitright = Physics2D.Raycast(gameobjGroup[0].transform.position, Vector2.right, tailLayerMask);
                if (hitleft.collider != null)
                {
                    gameobjGroup[0].transform.position = new Vector3(gameobjGroup[0].transform.position.x, hitleft.transform.position.y, gameobjGroup[0].transform.position.z);

            }

            if (hitright.collider != null)
                {
                    gameobjGroup[0].transform.position = new Vector3(gameobjGroup[0].transform.position.x, hitright.transform.position.y, gameobjGroup[0].transform.position.z);

            }

            return;
        }

        if (checkWall(1))
        {
/*            gameobjGroup[gameobjGroup.Count - 1].GetComponent<Rigidbody2D>().mass = 1000000000;
*/
        }
        else
        {
            gameobjGroup[gameobjGroup.Count - 1].GetComponent<Rigidbody2D>().mass = 1;

        }

    }
    public void addnewTail(Vector3 v)
    {
        GameObject initGameojbect = Instantiate(tailGameobject, new Vector3(100000, 10000, 1000), Quaternion.identity, this.gameObject.transform);
        gameobjGroup[gameobjGroup.Count - 1].AddComponent<FixedJoint2D>();
        gameobjGroup[gameobjGroup.Count - 1].GetComponent<FixedJoint2D>().connectedBody = initGameojbect.GetComponent<Rigidbody2D>();
        gameobjGroup.Add(initGameojbect);
        gameobjGroup[gameobjGroup.Count - 1].GetComponent<Rigidbody2D>().mass = gameobjGroup[gameobjGroup.Count-2].GetComponent<Rigidbody2D>().mass+ 10000;
        setnewPostion();

    }
    public void LateUpdate()
    {
        
        if(gameobjGroup[0].transform.position.y<-6)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene(currentSceneName);
        }
        if (Input.GetKeyDown(KeyCode.W) && !checkWall(0)&&canmove)
        {
            setDynamic(false);

            Debug.Log("hhel");
            if (checkApple(0))
            {
                Destroy(apple);

                addnewTail(new Vector3(0, 1, 0));
            }
            for (int i = 0; i <= gameobjGroup.Count - 1; i++)
            {
                if (gameobjGroup[i] != null)
                {
                    gameobjGroupBefor[i] = (gameobjGroup[i].transform.position);

                }

            }
            gameobjGroup[0].transform.position+=(new Vector3(0,1, 0));
            canmove = false;
            StartCoroutine(resetmove());
            setnewPostion();

        }
        if (Input.GetKeyDown(KeyCode.S) && !checkWall(1) && canmove)
        {
            setDynamic(false);

            if (checkApple(1))
            {
                Destroy(apple);

                addnewTail(new Vector3(0, -1, 0));
            }
            for (int i = 0; i <= gameobjGroup.Count - 1; i++)
            {
                if (gameobjGroup[i] != null)
                {
                    gameobjGroupBefor[i] = (gameobjGroup[i].transform.position);

                }

            }
            gameobjGroup[0].transform.position += (new Vector3(0, -1, 0));

            canmove = false;
            StartCoroutine(resetmove());
            setnewPostion();

        }
        if (Input.GetKeyDown(KeyCode.D) && !checkWall(2)&& !checkRight(gameobjGroup[0]) && canmove)
        {

            setDynamic(false);

            if (checkApple(2))
            {
                Destroy(apple);

                addnewTail(new Vector3(1, 0, 0));

            }
            for (int i = 0; i <= gameobjGroup.Count - 1; i++)
            {
                if(gameobjGroup[i]!=null)
                {
                    gameobjGroupBefor[i] = (gameobjGroup[i].transform.position);

                }

            }
            gameobjGroup[0].transform.position += (new Vector3(1, 0, 0));
            canmove = false;
            StartCoroutine(resetmove());
            setnewPostion();

        }
        if (Input.GetKeyDown(KeyCode.A) && !checkWall(3)&&!CheckLeft(gameobjGroup[0]) && canmove)
        {
            setDynamic(false);
            if (checkApple(3))
            {
                Destroy(apple);

                addnewTail(new Vector3(-1, 0, 0));

            }
            for (int i = 0; i <= gameobjGroup.Count - 1; i++)
            {
                if (gameobjGroup[i] != null)
                {
                    gameobjGroupBefor[i] = (gameobjGroup[i].transform.position);

                }

            }
            gameobjGroup[0].transform.position += (new Vector3(-1, 0, 0));

            canmove = false;

            StartCoroutine(resetmove());
            setnewPostion();

        }


    }
    public void setnewPostion()
    {                    
        for (int i = gameobjGroup.Count - 1; i > 0; i--)
        {
            if (gameobjGroup[i] != null&&gameobjGroup[i-1]!=null)
            {
                gameobjGroup[i].transform.localPosition = gameobjGroupBefor[i - 1];

            }
        }
    }
    public void setDynamic(bool i)
    {
        if (i)
        {
            foreach(GameObject obj in gameobjGroup)
            {
                obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }
        else
        {

            foreach (GameObject obj in gameobjGroup)
            {
                obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
        }
    }
    public bool checkWall(int direction) {
        switch (direction)
        {
            case 0:
                RaycastHit2D hitUp = Physics2D.Raycast(new Vector3(gameobjGroup[0].transform.position.x, gameobjGroup[0].transform.position.y + 0.5f, gameobjGroup[0].transform.position.z), Vector2.up, 0.980000f, wallLayerMask);
                return hitUp;
            case 1:
                RaycastHit2D hitDown = Physics2D.Raycast(new Vector3(gameobjGroup[0].transform.position.x, gameobjGroup[0].transform.position.y - 0.5f, gameobjGroup[0].transform.position.z), Vector2.down, 0.980000f, wallLayerMask);

                return hitDown;


            case 2:
                RaycastHit2D hitRight = Physics2D.Raycast(new Vector3(gameobjGroup[0].transform.position.x + 0.5f, gameobjGroup[0].transform.position.y, gameobjGroup[0].transform.position.z), Vector2.right, 0.980000f, wallLayerMask);
                return hitRight;

            case 3:
                RaycastHit2D hitLeft = Physics2D.Raycast(new Vector3(gameobjGroup[0].transform.position.x - 0.5f, gameobjGroup[0].transform.position.y, gameobjGroup[0].transform.position.z), Vector2.left, 0.980000f, wallLayerMask);
                return hitLeft;

        }
        return true;
    }
    IEnumerator resetmove()
    {
        yield return new WaitForSeconds(0.5f);
        setDynamic(true);
        yield return new WaitForSeconds(0.5f);
        canmove = true;

    }
    public bool CheckLeft(GameObject a)
    {
        Debug.Log(a);
        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector3(a.transform.position.x - 0.5f, a.transform.position.y,a.transform.position.z), Vector2.left, 1f, tailLayerMask);
        // 如果射线碰到了障碍物，返回 true；否则返回 false
        return hitLeft.collider!=null;
    }
    public bool checkRight(GameObject a)
    {

        RaycastHit2D hitRight = Physics2D.Raycast(new Vector3(a.transform.position.x + 0.5f, a.transform.position.y, a.transform.position.z), Vector2.right, 1f, tailLayerMask);

        // 如果射线碰到了障碍物，返回 true；否则返回 false
        return hitRight.collider != null;
    }
  
    public bool checkDown(GameObject a)
    {
        RaycastHit2D hitDown = Physics2D.Raycast(a.transform.position, Vector2.down, wallLayerMask);
        return hitDown.collider!=null;
    }
    public bool checkApple(int direction)
    {
        switch (direction)
        {
            case 0:
                RaycastHit2D hitUp = Physics2D.Raycast(new Vector3(gameobjGroup[0].transform.position.x, gameobjGroup[0].transform.position.y + 0.5f, gameobjGroup[0].transform.position.z), Vector2.up, 1f, appleLayerMask);
                return hitUp;
            case 1:
                RaycastHit2D hitDown = Physics2D.Raycast(new Vector3(gameobjGroup[0].transform.position.x, gameobjGroup[0].transform.position.y - 0.5f, gameobjGroup[0].transform.position.z), Vector2.down, 1f, appleLayerMask);

                return hitDown;


            case 2:
                RaycastHit2D hitRight = Physics2D.Raycast(new Vector3(gameobjGroup[0].transform.position.x + 0.5f, gameobjGroup[0].transform.position.y, gameobjGroup[0].transform.position.z), Vector2.right, 1f, appleLayerMask);
                return hitRight;

            case 3:
                RaycastHit2D hitLeft = Physics2D.Raycast(new Vector3(gameobjGroup[0].transform.position.x - 0.5f, gameobjGroup[0].transform.position.y, gameobjGroup[0].transform.position.z), Vector2.left, 1f, appleLayerMask);
                return hitLeft;

        }
        return true;
    }
}
