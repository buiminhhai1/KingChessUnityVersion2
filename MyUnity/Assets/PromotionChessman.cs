using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromotionChessman : MonoBehaviour
{

    public static PromotionChessman Instance { get; set; }

    public bool isDestroy = false;

    public GameObject promotionPrefab;
    //private List<GameObject> promotion;

    public int CurrentX { set; get; }
    public int CurrentY { set; get; }

    private void Start()
    {
        
        Instance = this;
        promotionPrefab.SetActive(false);
       // promotion = new List<GameObject>();
    }

    private void Update()
    {
        if(BoardManager.Instance.isPromotion)
        {
            promotionPrefab.SetActive(true);
        }
    }
    public void Queen()
    {
        //Chessman c = BoardManager.Instance.Chessmans[CurrentX, CurrentY];
        if (BoardManager.Instance.isWhiteTurn) // Lượt của quân đen
        {
            // tại vì khi chạy vào dòng code này thì isWhiteTurn đã chuyển sang lượt của quân đen
           
            BoardManager.Instance.SpawnChessman(4, CurrentX, CurrentY);
        }
        else
        {
            BoardManager.Instance.SpawnChessman(10, CurrentX, CurrentY);
        }

        BoardManager.Instance.isPromotion = false;
        promotionPrefab.SetActive(false);
        Debug.Log("Promotion Queen");
    }

    public void Rook()
    {
        if (BoardManager.Instance.isWhiteTurn) // Lượt của quân đen
        {
            // tại vì khi chạy vào dòng code này thì isWhiteTurn đã chuyển sang lượt của quân đen
            BoardManager.Instance.SpawnChessman(5, CurrentX, CurrentY);
        }
        else
        {
            BoardManager.Instance.SpawnChessman(11, CurrentX, CurrentY);
        }

        BoardManager.Instance.isPromotion = false;
        //BoardManager.Instance.SpawnChessman()
        promotionPrefab.SetActive(false);
        Debug.Log("Promotion Rook");
    }

    public void Knight()
    {
        if (BoardManager.Instance.isWhiteTurn) // Lượt của quân đen
        {
            // tại vì khi chạy vào dòng code này thì isWhiteTurn đã chuyển sang lượt của quân đen
            BoardManager.Instance.SpawnChessman(2, CurrentX, CurrentY);
        }
        else
        {
            BoardManager.Instance.SpawnChessman(8, CurrentX, CurrentY);
        }

        BoardManager.Instance.isPromotion = false;
        //BoardManager.Instance.SpawnChessman()
        promotionPrefab.SetActive(false);
        Debug.Log("Promotion Knight");
    }

    public void Bishop()
    {
        if (BoardManager.Instance.isWhiteTurn) // Lượt của quân đen
        {
            // tại vì khi chạy vào dòng code này thì isWhiteTurn đã chuyển sang lượt của quân đen
            BoardManager.Instance.SpawnChessman(0, CurrentX, CurrentY);
        }
        else
        {
            BoardManager.Instance.SpawnChessman(6, CurrentX, CurrentY);
        }
        BoardManager.Instance.isWhiteTurn = !BoardManager.Instance.isWhiteTurn;
        //BoardManager.Instance.SpawnChessman()
        promotionPrefab.SetActive(false);
        BoardManager.Instance.isPromotion = false;
        Debug.Log("Promotion Bishop");
    }

    //public void Cancel()
    //{
    //    BoardManager.Instance.isWhiteTurn = !BoardManager.Instance.isWhiteTurn;
    //    BoardManager.Instance.isPromotion = false;
    //    //BoardManager.Instance.SpawnChessman()
    //    promotionPrefab.SetActive(false);
    //    Debug.Log("Promotion Bishop");
    //}





    //void Update()
    //{

    //    //if (BoardManager.)
    //        if (Input.GetKeyDown(KeyCode.Escape))
    //        {
    //            if (GameIsPaused)
    //            {

    //                Resume();
    //            }
    //            else
    //            {
    //                Pause();
    //            }
    //        }
    //}
    //public int PromotionNumber = 0;

    //public void SetPromotion()
    //{
    //    Debug.Log("QueenPromotion");
    //    // if(Input.GetMouseButtonDown(btnQPromotion))
    //}
    //public int QueenPromotion()
    //{
    //    Debug.Log("QueenPromotion");
    //    return 1;
    //}

    //public int RookPromotion()
    //{
    //    Debug.Log("Rook Promotion");
    //    return 2;
    //}

    //public int KnightPromotion()
    //{
    //    Debug.Log("Knight Promotion");
    //    return 3;
    //}

    //public int BishopPromotion()
    //{
    //    Debug.Log("Bishop Promotion");
    //    return 4;
    //}

    //public int Cancel()
    //{
    //    Debug.Log("Cancel promotion");
    //    return 0;
    //}
    //public void Resume()
    //{
    //    PromotionUI.SetActive(false);
    //    //Time.timeScale = 1f;
    //    GameIsPaused = false;
    //}

    //void Pause()
    //{
    //    PromotionUI.SetActive(true);
    //    //Time.timeScale = 0f;
    //    GameIsPaused = true;
    //}

    //public void LoadMenu()
    //{
    //    Debug.Log("Loading menu...");

    //}

    //public void QuitGame()
    //{
    //    Debug.Log("Quitting Game...");
    //}
}

// Update is called once per frame
