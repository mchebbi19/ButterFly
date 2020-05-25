using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
public class Level : MonoBehaviour
{// pipe widh et height sont prix de l'inspecteur
    private const float PIPE_WIDH = 1.5f;
    
    //private const float PIPE_HEAD_WIDH = 1.7f;
   // private const float PIPE_HEAD_HEIGHT = 1.7f;
    private const float PIPE_DESTROY_X_POSITION = -20f;
 
    private const float PIPE_SPAWN_X_POSITION = 20f;
    private const float GROUND_DESTROY_X_POSITION = -40;
    //private const float GROUND_SPAWN_X_POSITION = 20f;
    private int pipePassedCount;
    private const float CAMERA_ORTHO_SIZE = 10f;
     
    private const float PIPE_MOVE_SPEED = 10f;

    public float VariableSpeed = 2f;

    private const float BIRD_X_POSITION = 0f;

    private  float pipeSpawnTime ;
    private  float pipeSpawnTimeMax ;    
    private  float gapSize ;
    private  int pipeSpawned ;
    private State state;
    private List<Pipe> pipeList;
   


    public enum Difficulty
    {
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4,
        Level5 = 5,
        Level6 = 6,
        Level7 = 7,
        Level8 = 8,
        Level9 = 9,
        Level10 = 10,
    }
    public enum State
    {
        WaitingToStart,
        Playing,
        Pause,
        BirdDead,
     }

    private static Level instance;

    private List<Transform> groudList;
    public static Level GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        Debug.Log("GameHandler.instance");
        instance = this;
        pipeList = new List<Pipe>();
        
        SpawnInitialGround();
        pipeSpawnTimeMax = 1.5f;
        gapSize = 4f;
        SetDifficulty(Difficulty.Level1);
        state = State.WaitingToStart;
    }

    private void Start()
    {
        
        Bird.GetInstance().Ondied += Bird_OnDied;
        Bird.GetInstance().OnStartPlaying += Bird_OnStartPlaying;
        Bird.GetInstance().OnPauseMode += Bird_OnPauseMode;
        
    }

    // Event bird died
    private void Bird_OnDied(object sender, System.EventArgs e)
    {
        //CMDebug.TextPopup("Dead! ", new Vector3(0.0f, 0.0f, 0.0f));
        state = State.BirdDead;
        
        //FunctionTimer.Create(() => {
        //    UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        //}, 5f);
    }

    // Event bird OnStartPlaying
    private void Bird_OnStartPlaying(object sender, System.EventArgs e)
    {
        
        state = State.Playing;

        GameObject winShow = GameObject.Find("TAPTOFLY");
        winShow.gameObject.SetActive(false);

    }


    // Event bird OnPauseMode
    private void Bird_OnPauseMode(object sender, System.EventArgs e)
    {
        
        state = State.Pause;
               
    }
    private void Update()
    {
        if (state == State.Playing)
        {
            HandlePipeMouvement();
            HandlePipeSpawning();
            HandleGround();
        }

    }


    private void HandlePipeMouvement()
    {
        //foreach (Pipe pipe in pipeList) {

        for (int i = 0; i < pipeList.Count; i++) {
            Pipe pipe = pipeList[i];
            bool isInTheRightOfbird = pipe.GetXposition() > BIRD_X_POSITION;
            pipe.Move();

            if (isInTheRightOfbird && pipe.GetXposition() <= BIRD_X_POSITION && pipe.IsBottom())
            {
                //pipe passed  bird
                pipePassedCount = pipePassedCount+ 1;
                SoundManager.PlaySound(SoundManager.Sound.Score);
            }
            if (pipe.GetXposition() <  PIPE_DESTROY_X_POSITION)
            {
                // destroy
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }
    }

    private void HandlePipeSpawning() {
        pipeSpawnTime -= Time.deltaTime;
        if (pipeSpawnTime < 0)
        {
            // Time to spawn anotrher Pipe
            pipeSpawnTime += pipeSpawnTimeMax;
            float HeightEdgeLimit = 2f;
            float minHeight = gapSize * .5f + HeightEdgeLimit;
            float TotalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight = TotalHeight - gapSize * .5f - HeightEdgeLimit;
            float Height = UnityEngine.Random.Range(minHeight, maxHeight);

            CreateGapPipes(Height, gapSize, PIPE_SPAWN_X_POSITION);
            pipeSpawned++;
            SetDifficulty(GetDifficulty());
        }


    }

    private void HandleGround()
    {
        foreach(Transform groundTransform in groudList)
        {
            groundTransform.position += new Vector3(-1f, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            if (groundTransform.position.x < GROUND_DESTROY_X_POSITION)
            {
                // Ground passed the left side, relocate on right side
                // Find right most X position
                float rightMostXPosition = 30f;
                for (int i=1; i < groudList.Count; i++)
                {
                    if (groudList[i].position.x > rightMostXPosition)
                    {
                        rightMostXPosition = groudList[i].position.x;
                    }
                }
                // Place Ground on the right most position
                float groundWidh = 30f;
                groundTransform.position = new Vector3(rightMostXPosition + groundWidh, groundTransform.position.y, groundTransform.position.z);
            }
        }
    }

    private void SpawnInitialGround()
    {
        groudList = new List<Transform>();
        Transform groundTransform;
        float groundY = -10.08f;
        float groundWidh = 30f;
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(0, groundY, 0),Quaternion.identity);
        groudList.Add(groundTransform);

        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(groundWidh, groundY, 0), Quaternion.identity);
        groudList.Add(groundTransform);

        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(groundWidh * 2f, groundY, 0), Quaternion.identity);
        groudList.Add(groundTransform);

    }

    private Difficulty GetDifficulty()
    {
        if (pipeSpawned >= 80) return Difficulty.Level10;
        if (pipeSpawned >= 70) return Difficulty.Level9;
        if (pipeSpawned >= 60) return Difficulty.Level8;
        if (pipeSpawned >= 50) return Difficulty.Level7;
        if (pipeSpawned >= 40) return Difficulty.Level6;
        if (pipeSpawned >= 30) return Difficulty.Level5;
        if (pipeSpawned >= 20) return Difficulty.Level4;
        if (pipeSpawned >= 10) return Difficulty.Level3;
        if (pipeSpawned >= 5) return Difficulty.Level2;
        return Difficulty.Level1;
    }

    private float GetVariableSpeed()
    {

        return VariableSpeed;
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        switch(difficulty)
        {
            case Difficulty.Level1:
                gapSize = 7f;
                pipeSpawnTimeMax = 1.5f;
                VariableSpeed = 0.8f;
                break;
            case Difficulty.Level2:
                gapSize = 6f;
                pipeSpawnTimeMax = 1.45f;
                VariableSpeed = 0.9f;
                break;
            case Difficulty.Level3:                
                gapSize = 5f;
                pipeSpawnTimeMax = 1.4f;
                VariableSpeed = 1.1f;
                break;
            case Difficulty.Level4:
                gapSize = 4f;
                pipeSpawnTimeMax = 1.3f;
                VariableSpeed = 1.1f;                
                break;
            case Difficulty.Level5:
                gapSize = 3.5f;                
                pipeSpawnTimeMax = 1.4f;
                VariableSpeed = 1.2f;
                break;
            case Difficulty.Level6:
                gapSize = 2.5f;                
                pipeSpawnTimeMax = 1.5f;
                VariableSpeed = 1.3f;
                break;
            case Difficulty.Level7:
                gapSize = 2.5f; pipeSpawnTimeMax = 1.5f;

                break;
            case Difficulty.Level8:
                gapSize = 2.5f; pipeSpawnTimeMax = 1.5f;
                break;
            case Difficulty.Level9:
                gapSize = 2.5f; pipeSpawnTimeMax = 1.5f;
                break;
            case Difficulty.Level10:
            gapSize = 2.5f; pipeSpawnTimeMax = 1.5f;
                break;
                

        }
    }

    private void CreatePipe(float height, float xPosition, bool bCreateBottom)
    {
        //// set up head
        //Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        //pipeHead.position = new Vector3(xPosition , (MAX_HEIGHT - height)  *.5f );

        //SpriteRenderer pipeHeadSpriteRender = pipeHead.GetComponent<SpriteRenderer>();
        //pipeHeadSpriteRender.size = new Vector2(PIPE_HEAD_WIDH, 1.0f);      
        //pipeHead.localScale = new Vector3(1.0f, -1.0f, 1.0f);

        // set up Body

        float pipeBodyYposition;

        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        if (bCreateBottom) {
            pipeBodyYposition = -CAMERA_ORTHO_SIZE;
        }
        else
        {
            pipeBodyYposition = +CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1,-1,1);
        }
        pipeBody.position = new Vector3(xPosition, pipeBodyYposition);
      

        SpriteRenderer pipeBodySpriteRender = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRender.size = new Vector2(PIPE_WIDH, height);


        // set up Boxcolodier
        BoxCollider2D pipeBodyBoxClollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxClollider.size = new Vector2(PIPE_WIDH, height);
        pipeBodyBoxClollider.offset = new Vector2(0, height * .5f);

        Pipe pipe = new Pipe(pipeBody, bCreateBottom);
        pipeList.Add(pipe);
    }


    private void CreateGapPipes(float gapY, float gapsize, float xPosition)   {
        CreatePipe(gapY - gapsize * .5f, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2 - gapY - gapsize * .5f, xPosition, false);
    }

    public int GetPipeSpawned() {
         return pipeSpawned;
    }
    public int GetPipePassedCount()
    {
        return pipePassedCount;
    }
    
    /* class*/
    private class Pipe
    {
        private Transform pipeBodyTransform;
        private bool IsBotom;
        public Pipe(Transform pipeBodyTransform, bool IsBotom)
        {
            this.pipeBodyTransform = pipeBodyTransform;
            this.IsBotom = IsBotom;
        }

        public void Move()
        {
            pipeBodyTransform.position += new Vector3(-1f, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime * Level.GetInstance().GetVariableSpeed();
        }

        // retourne position du pipe pour cleaning (qui sort de l'écran)
        public float GetXposition()
        {
            return pipeBodyTransform.position.x;
        }

        public void DestroySelf()
        {
            Destroy(pipeBodyTransform.gameObject);
        }

        public bool IsBottom()
        {
            return IsBotom;
        }
    }

}
