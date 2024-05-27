using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




public class GameManegerScript : MonoBehaviour
{


    //�ǉ�
    public GameObject PlayerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;

    public GameObject clearText;

    int[,] map;
    GameObject[,] field;//�Q�[���Ǘ��p�̔z��


    void Start()
    {
        //GameObject instance = Instantiate(
        //    PlayerPrefab,
        //    new Vector3(0, 0, 0),
        //    Quaternion.identity
        //
        //    );

        map = new int[,]
        {
          {0,0,0,0,0 },
          {0,3,1,3,0 },
          {0,0,2,0,0 },
          {0,2,3,2,0 },
          {0,0,0,0,0 },
        };

        field = new GameObject
            [
            map.GetLength(0),
            map.GetLength(1)
            ];

        string debugTXT = "";

        //�ύX�B��dfor���œ�d�z��̏����o��
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(
                        PlayerPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                        );
                }

                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity);
                }


                if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(
                        goalPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0.1f),
                        Quaternion.identity);
                }
                //debugTXT += map[x, y].ToString() + ",";

            }
            //debugTXT += "\n";//���s

        }



        Debug.Log(debugTXT);

        //PrintArray();
    }

    //private void PrintArray()
    //{
    //    string debugtext = "";
    //    for (int i = 0; i < map.Length; i++)
    //    {
    //        debugtext += map[i].ToString() + ",";
    //    }
    //    Debug.Log(debugtext);
    //}


    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] == null) { continue; }
                if (field[y, x].tag == "Player") { return new Vector2Int(x, y); }
            }
        }
        return new Vector2Int(-1, -1);
    }

    //null�`�F�b�N�����Ă���^�O�̃`�F�b�N���s��

    //Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber(
                playerIndex,
                playerIndex + new Vector2Int(1, 0));
            //PrintArray();

            //�����N���A���Ă�����
            if (IsCleard())
            {
                //�Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber(
                playerIndex,
                playerIndex + new Vector2Int(-1, 0));
            //PrintArray();

            //�����N���A���Ă�����
            if (IsCleard())
            {
                //�Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber(
                playerIndex,
                playerIndex + new Vector2Int(0, -1));
            //PrintArray();

            //�����N���A���Ă�����
            if (IsCleard())
            {
                //�Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber(
                playerIndex,
                playerIndex + new Vector2Int(0, 1));
            //PrintArray();

            //�����N���A���Ă�����
            if (IsCleard())
            {
                //�Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
                clearText.SetActive(true);
            }
        }
    }

    bool IsCleard()
    {
        //Vector2Int�̉ϒ��z��̍쐬
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                //�i�[�ꏊ���ۂ��𔻒f
                if (map[y, x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        //�v�f����goals.Count�Ŏ擾
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                //��ł������Ȃ�������������B��
                return false;
            }
        }
        //�������B���łȂ���Ώ����B��
        return true;
    }


    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)
    {


        //�c�������̔z��O�Q�Ƃ����Ă��Ȃ���
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }

        //Box�^�O�������Ă�����ċN����
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(moveTo, moveTo + velocity);
            if (!success) { return false; }
        }

        //GameObject�̍��W(position)���ғ������Ă���C���f�b�N�X�����ւ�

        // field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        Vector3 moveToPosition = new Vector3(
            moveTo.x, map.GetLength(0) - moveTo.y, 0);
        field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);

        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];

        field[moveFrom.y, moveFrom.x] = null;

        return true;

    }



}



//int[] map = { 0, 0, 0, 2, 0, 1, 0, 2, 0, 0, 0 };

// Start is called before the first frame update
//void Start()
//{
//
//   // PrintArray();
//}


//
//    MoveNumber(1, PlayerIndex, PlayerIndex + 1);
//}
//
//
//if (Input.GetKeyDown(KeyCode.LeftArrow))
//{
//    int PlayerIndex = GetPlayerIndex();
//
//    PrintArray();
//    MoveNumber(1, PlayerIndex, PlayerIndex - 1);
//
//}
//}
