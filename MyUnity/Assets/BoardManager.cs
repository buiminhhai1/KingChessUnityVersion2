using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PromotionChess
{
    public bool isPromotion { set; get; }
}

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; } // hien thi Ban co hien tai
    private bool[,] allowedMoves { set; get; }  // Chua cac nuoc di co the di chuyen duoc cua quan co

    public Chessman[,] Chessmans { set; get; }  // Mang cac quan co tren ban co
    private Chessman selectedChessman;          // Luu quan co duoc chon tren ban co

    #region Cac so lieu dinh nghia cua ban co
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    #endregion


    // Quan co duoc chon tren ban co
    private int selectionX = -1;
    private int selectionY = -1;
    
    
    public List<GameObject> chessmanPrefabs;    // Chua mo hinh cac quan co
    private List<GameObject> activeChessman;    // Cac mo hinh con ton tai tren co

    private Material previousMat;               // Chua kieu mo hinh quan co truoc khi duoc chon
    public Material selectedMat;                // Kieu mo hinh quan co duoc chon

    public int[] EnPassantMove { set; get; }

    // Xác định các hướng khác nhau mà điều khiển hoặc bố trí có thể có.
    private Quaternion orientation = Quaternion.Euler(0, 180, 0); 

    public bool isWhiteTurn = true; // Mac dinh cho quan trang di truoc
    public bool isPromotion = false; 

    private void Start()
    {
        //isMyPromotion.isPromotion = false;
        Instance = this;        // Hien thi ban co hien tai
        SpawnAllChessmans();    // Bo tri quan co moi tren ban co
    }
    
    private void Update()
    {
        UpdateSelection();
        DrawChessboard();   

        if(Input.GetMouseButtonDown(0))
        {
            if(selectionX >= 0 && selectionY >= 0)
            {
                if(selectedChessman == null)
                {
                    // Select the chessman
                    SelectChessman(selectionX, selectionY);
                }
                else
                {
                    // Move the chessman
                    MoveChessman(selectionX, selectionY);
                }
            }
        }
    }   

    // Chon quan co tai vi tri x, y tren ban co
    private void SelectChessman(int x, int y)
    {
        if (Chessmans[x, y] == null)    // Neu vi tri tren ban co khong co quan co
            return;

        if (Chessmans[x, y].isWhite != isWhiteTurn) // Neu nhu quan co duoc chon la quan trang nhung khong phai luot cua quan trang thi return
            return;
        
        bool hasAtleastOneMove = false; 
        allowedMoves = Chessmans[x, y].PossibleMove(); // Tim so nuoc di co the cua quan co [x,y] tren ban co
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (allowedMoves[i, j]) // Neu nhu co ton tai nuoc di 
                    hasAtleastOneMove = true;

        if (!hasAtleastOneMove) // Nếu như khong có nước đi
            return;

        selectedChessman = Chessmans[x, y]; // Lưu quân cờ được chon
        // Thao tác với kiểu mô hình được chọn
        previousMat = selectedChessman.GetComponent<MeshRenderer>().material;
        selectedMat.mainTexture = previousMat.mainTexture;
        selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;
        // Hiển thị đường đi có thể di chuyển của quân cờ
        BoardHighligts.Instance.HighlightAllowedMoves(allowedMoves);
    }

    // Di chuyển quân cờ tới vị trí có thể di chuyển
    private void MoveChessman(int x, int y)
    {
        if(allowedMoves[x,y])   // Nếu như có nước đi ở vị trí x , y
        {
            Chessman c = Chessmans[x, y];   // Lưu quân cờ ở vị trí x, y

            // Nếu như tồn tại quân cờ và quân cờ này không phải quân cờ trắng 
            if(c!= null && c.isWhite != isWhiteTurn)
            {
                // Ăn quân cờ
                // Nếu quân cờ kia là quân Vua Đen 
                if(c.GetType() == typeof(King))
                {
                    // Tạo ra MessageBox 
                    // End the game 
                    EndGame();
                    return;
                }

                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            #region Quân tốt
            if (x == EnPassantMove[0] && y == EnPassantMove[1])
            {
                if(isWhiteTurn)
                    c = Chessmans[x, y + 1];
                else
                    c = Chessmans[x, y - 1];
                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            EnPassantMove[0] = -1;
            EnPassantMove[1] = -1;
            
            // Nếu như quân cờ được chonj là quân tốt 
            if(selectedChessman.GetType() == typeof(Pawn))
            {
                // Nếu quân tốt trắng sang đến cuối để phong 
                if (y == 0)
                {
                    var tempChessman = selectedChessman;
                    PromotionChessman.Instance.CurrentX = x;
                    PromotionChessman.Instance.CurrentY = y;
                    
                    isPromotion = true;

                    //activeChessman.Remove(selectedChessman.gameObject);
                    //Destroy(selectedChessman.gameObject);
                    
                    if (Input.GetMouseButtonDown(1))
                    {
                        selectedChessman = Chessmans[x, y];
                    }
                    activeChessman.Remove(tempChessman.gameObject);
                    Destroy(tempChessman.gameObject);
                    
                        

                }
                else if (y == 7) // Nếu như quân tốt đen sang đến cuối để phong
                {
                    var tempChessman = selectedChessman;
                    PromotionChessman.Instance.CurrentX = x;
                    PromotionChessman.Instance.CurrentY = y;
                    
                    // Chọn tước vị để phong

                    isPromotion = true;
                    if (Input.GetMouseButtonDown(1))
                    {
                        // Hủy quân tốt

                        // Sắc phong quân tốt
                        //selectedChessman = Chessmans[x, y];
                        selectedChessman = Chessmans[x, y];
                    }

                    activeChessman.Remove(tempChessman.gameObject);
                    Destroy(tempChessman.gameObject);
                }

                // Ăn quân tốt địch nếu như nó di chuyển 2 nút ban đầu
                if (selectedChessman.CurrentY == 1 && y == 3) // Trường hợp của quân tốt đen 
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y - 1;
                }
                else if(selectedChessman.CurrentY == 6 && y == 4)  
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y + 1;
                }
            }
            #endregion

            // Vị trí mà vị trí của quân cờ trước khi di chuyển sẽ bị xóa bỏ
            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;

            isWhiteTurn = !isWhiteTurn; // Chuyển lượt cho quân đối thủ
        }

        selectedChessman.GetComponent<MeshRenderer>().material = previousMat;   // Phục hồi màu cho quân cờ được chọn

        BoardHighligts.Instance.Hidehighlights();
        selectedChessman = null;
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit, 25.0f,LayerMask.GetMask("ChessPlane")))
        {
            //Debug.Log(hit.point);
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }
    
    public void SpawnChessman(int index, int x, int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], GetTileCenter(x,y), orientation) as GameObject;
        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
    }

    private void SpawnAllChessmans()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];
        EnPassantMove = new int[2] { -1, -1 };

        // Spawn the white team!
        // Bishops 
        SpawnChessman(0, 2, 0);
        SpawnChessman(0, 5, 0);
        // King
        SpawnChessman(1, 3, 0);
        //Knights
        SpawnChessman(2, 1, 0);
        SpawnChessman(2, 6, 0);
        // Pawns 
        for(int i = 0; i < 8; i ++)
        {
            SpawnChessman(3, i, 1);
        }
        //Queen
        SpawnChessman(4, 4, 0);
        // Rooks 
        SpawnChessman(5, 0, 0);
        SpawnChessman(5, 7, 0);


        // Spawn Black Team!
        // Bishops
        SpawnChessman(6, 2, 7);
        SpawnChessman(6, 5, 7);
        // King
        SpawnChessman(7, 3, 7);
        //Knights
        SpawnChessman(8, 1, 7);
        SpawnChessman(8, 6, 7);
        //Pawns 
        for(int i = 0; i < 8; i++)
        {
            SpawnChessman(9, i, 6);
        }
        // Queen
        SpawnChessman(10, 4, 7);
        // Rooks
        SpawnChessman(11, 0, 7);
        SpawnChessman(11, 7, 7);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    private void DrawChessboard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for(int i =  0; i < 9; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);

            for(int j = 0; j < 9; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }

            // Draw the selection
            if(selectionX >=0 && selectionY >=0 )
            {
                Debug.DrawLine(
                    Vector3.forward * selectionY + Vector3.right * selectionX,
                    Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

                Debug.DrawLine(
                    Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                    Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
            }
        }
    }

    private void EndGame()
    {
        if (isWhiteTurn)
            Debug.Log("White team wins");
        else
            Debug.Log("Black team wins");

        foreach (GameObject go in activeChessman)
            Destroy(go);

        isWhiteTurn = true;
        BoardHighligts.Instance.Hidehighlights();
        SpawnAllChessmans();
    }
}
