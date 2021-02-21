using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static bool GameOver = false;
    public static bool NextStage = false;
    public static bool moveHoleToNext=false;
    public static bool isSecondStage = false;

    [SerializeField] ParticleSystem winFx;

    public int collectablesInFirstStage;
    public int collectablesInSecondStage;

    public Camera cam;
    public GameObject Hole;
    public GameObject Door;
    public Image firstStageFill;
    public Image secondStageFill;
    public TMP_Text stage1Text;
    public TMP_Text stage2Text;

    public Transform FirstCollectablesParent;
    public Transform SecondCollectablesParent;

    public float speed;

    private Vector3 camTarget;
    private Vector3 holeTarget;
    private Vector3 holeTargetAtSecondStage;
    private int firstCol;
    private int secondCol;

    private void Start()
    {
        //Assigning the level numbers and resettting progress bars
        stage1Text.text = (SceneManager.GetActiveScene().buildIndex*2+1).ToString();
        stage2Text.text = (SceneManager.GetActiveScene().buildIndex*2 + 2).ToString();
        firstStageFill.fillAmount = 0;
        secondStageFill.fillAmount = 0;

        //Counting Collectable Objects in the Scene. (Can be group of objects or single object)
        for (int i = 0; i < FirstCollectablesParent.childCount; i++)
        {
            if (FirstCollectablesParent.GetChild(i).childCount == 0)
            {
                collectablesInFirstStage += 1;
            }else
                collectablesInFirstStage += FirstCollectablesParent.GetChild(i).childCount;
        }
        for (int i = 0; i < SecondCollectablesParent.childCount; i++)
        {
            if (SecondCollectablesParent.GetChild(i).childCount == 0)
            {
                collectablesInSecondStage += 1;
            }
            else
                collectablesInSecondStage += SecondCollectablesParent.GetChild(i).childCount;
        }

        firstCol = collectablesInFirstStage;
        secondCol = collectablesInSecondStage;
        isSecondStage = false;
        moveHoleToNext = false;      
        camTarget = new Vector3(0, 16.18f, 23f);
        holeTarget = new Vector3(0, 0, 0);
        holeTargetAtSecondStage = new Vector3(0, 0, 30);
    }

    //Detecting the falling objects and arranging gameplay accordingly.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.root.tag.Equals("Collectable1"))
        {
            collectablesInFirstStage--;
            firstStageFill.fillAmount = (float)(firstCol - collectablesInFirstStage) / (float)firstCol;
            PullActivateRb.Instance.RemoveFromPull(other.attachedRigidbody); //Remove the object from list before destroy
            Destroy(other.gameObject);
            if (collectablesInFirstStage == 0)
            {
                //NextStage
                GoNextStage();
            }
        }
        if (other.gameObject.transform.root.tag.Equals("Collectable2"))
        {
            collectablesInSecondStage--;
            secondStageFill.fillAmount = (float)(secondCol - collectablesInSecondStage) / (float)secondCol;

            PullActivateRb.Instance.RemoveFromPull(other.attachedRigidbody); //Remove the object from list before destroy
            Destroy(other.gameObject);
            if (collectablesInSecondStage == 0)
            {
                //Load Next Scene
               StartCoroutine("LoadNextLevel");
            }
        }
        if (other.gameObject.transform.root.tag.Equals("NonCollectable"))
        {
            //RestartLevel
            PullActivateRb.Instance.RemoveFromPull(other.attachedRigidbody); //Remove the object from list before destroy
            RestartLevel();
        }
    }

    //Playe winFx and Load Next Scene after 2 sec.
    private IEnumerator LoadNextLevel()
    {
        winFx.Play();
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoNextStage()
    {
        NextStage = true;
    }

    //Arranging the transition to the next stage and disable the player movement control during transition by making static booleans false.
    public void FixedUpdate()
    {
        if (NextStage == true)
        {
            Hole.transform.position = Vector3.MoveTowards(Hole.transform.position, holeTarget, speed * Time.deltaTime);
            Door.transform.position= Vector3.MoveTowards(Door.transform.position, new Vector3(0,-1.1f,10), speed * Time.deltaTime);
            Hole.GetComponent<CapsuleCollider>().enabled = false;
            
        }
        if (Hole.transform.position == holeTarget)
        {
            moveHoleToNext = true;
            NextStage = false;
        }
        if (moveHoleToNext == true)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, camTarget, speed * Time.deltaTime * 10);
            Hole.transform.position = Vector3.MoveTowards(Hole.transform.position, holeTargetAtSecondStage, speed * Time.deltaTime*10);
        }
        if (Hole.transform.position == holeTargetAtSecondStage && cam.transform.position==camTarget)
        {
            moveHoleToNext = false;
            isSecondStage = true;
            Hole.GetComponent<CapsuleCollider>().enabled = true ;
        }
    }
}
