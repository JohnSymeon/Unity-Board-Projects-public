using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board_Generator : MonoBehaviour
{

    public GameObject hex_tile_prefab;

    public int size;

    public Tile[,] board;

    public bool[,] connections_matrix;

    // Start is called before the first frame update
    void Start()
    {
        board = new Tile[size,size];
        float j_offset=0f;
        float i_offset = 0f;
        for(int i=0;i<size;i++)
        {
            for(int j=0;j<size;j++)
            {
                GameObject o = Instantiate(hex_tile_prefab,new Vector3(j-j_offset,i - i_offset,0),transform.rotation, gameObject.transform);
                o.transform.parent = gameObject.transform;
                board[i,j] = new Tile(i*size+j,o);
            }
            j_offset += 0.5f;
            i_offset += 0.11f;
        }

        connections_matrix = new bool[size*size, size*size];
        Initialise_CM();

        for(int j=0;j<size*size;j++)
        {
            if(connections_matrix[16,j])
                Debug.Log(j);
        }

        test_BoardtoWorld();
        Debug.Log(connections_matrix);
        transform.position = new Vector3(3.86f,1.77f,0f);
        transform.localScale = new Vector3(0.7f,0.7f,0.7f);
        
    }

    TextMesh[,] arr;
    public void test_BoardtoWorld()
    {
        if(arr==null)
        {
            arr = new TextMesh[size,size];
            
        }
            
        for(int i=0;i<size;i++)
        {
            for(int j=0;j<size;j++)
            {
                Color color;
                string text;

                text = board[i,j].id.ToString();
                color =Color.white;
                /*if(board[i,j].ID== Cell_status.Neutral)
                {
                    text = "N";
                    color =Color.white;
                }
                    
                else if(board[i,j].status== Cell_status.Player)
                {
                    color = Color.red;
                    text = "R";
                }
                else
                {
                    color = Color.yellow;
                    text = "Y";
                }*/
                    
                if(arr[i,j]==null)
                    arr[i,j] = CreateWorldText(text,color, TextAnchor.MiddleCenter, TextAlignment.Center,board[i,j].go.transform.position,100,gameObject.transform);
                else
                {
                    arr[i,j].text = text;
                    arr[i,j].color = color;
                }
                    
            }
        }
       
    }

    public TextMesh CreateWorldText( string text, Color color, TextAnchor textAnchor, TextAlignment textAlignment,Vector3  localPosition , int sortingOrder,Transform parent = null, int fontSize = 10)
    {
        GameObject gameObject = new GameObject("world_text",typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent,true);
        transform.localPosition = localPosition;
        transform.eulerAngles = new Vector3(-90,180,90);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text= text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;

    }

    void Initialise_CM()
    {
        // upper left = 0,0
        for(int j=0;j<size;j++)
        {
            for(int i=0;i<size;i++)
            {
                //upper left corner//LOWER LEFT
                if( i==0 && j==0)
                {
                    connections_matrix[board[i,j].id, i*size+j+1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j] = true;
                }
                else if(i==size-1 && j==size-1)//lower right corner//UPPER RIGHT
                {
                    connections_matrix[board[i,j].id, i*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i-1)*size+j] = true;
                }
                else if(i==0 && j == size-1)//upper right corner
                {
                    connections_matrix[board[i,j].id, i*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j] = true;
                }
                else if(i==size-1 && j ==0)//lower left corner
                {
                    connections_matrix[board[i,j].id, (i-1)*size+j] = true;
                    connections_matrix[board[i,j].id, (i-1)*size+j+1] = true;
                    connections_matrix[board[i,j].id, i*size+j+1] = true;
                }
                else if(j==0)//left column
                {
                    connections_matrix[board[i,j].id, (i-1)*size+j+1] = true;
                    connections_matrix[board[i,j].id, i*size+j+1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j] = true;
                    connections_matrix[board[i,j].id, (i-1)*size+j] = true;
                }
                else if(j==size-1)//right column
                {
                    connections_matrix[board[i,j].id, (i-1)*size+j] = true;
                    connections_matrix[board[i,j].id, (i)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j] = true;
                }
                else if(i==0)//upper row
                {
                    connections_matrix[board[i,j].id, (i)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i)*size+j+1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j] = true;
                }
                else if(i==size-1)//lower row
                {
                    connections_matrix[board[i,j].id, (i-1)*size+j] = true;
                    connections_matrix[board[i,j].id, (i-1)*size+j+1] = true;
                    connections_matrix[board[i,j].id, (i)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i)*size+j+1] = true;
                }
                else if(i>0 && j>0 && (i<size-1) && (j<size-1))//rest
                {
                    connections_matrix[board[i,j].id, (i)*size+j+1] = true;
                    connections_matrix[board[i,j].id, (i)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j] = true;
                    connections_matrix[board[i,j].id, (i+1)*size+j-1] = true;
                    connections_matrix[board[i,j].id, (i-1)*size+j] = true;
                    connections_matrix[board[i,j].id, (i-1)*size+j+1] = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        test_BoardtoWorld();
    }
}