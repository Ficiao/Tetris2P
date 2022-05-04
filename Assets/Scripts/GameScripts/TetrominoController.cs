using UnityEngine;

namespace GameScene
{
    public delegate void GameEnd(string playerName);

    public class TetrominoController : MonoBehaviour
    {
        [SerializeField]
        private string leftInput;
        [SerializeField]
        private string rightInput;
        [SerializeField]
        private string downInput;
        [SerializeField]
        private string dropInput;
        [SerializeField]
        private string holdInput;
        [SerializeField]
        private string rotateLeftInput;
        [SerializeField]
        private string rotateRightInput;
        [SerializeField]
        private TetrisQueue tetrominoQueue;
        [SerializeField]
        public TetrisGrid grid;
        public int id;

        public static event GameEnd GameEnd;

        public string playerName;      
        private bool allowedToPutOnHold;
        public GameObject tetromino;
        private Transform tetrominoTransform;
        private Tetromino tetrominoStats;
        private int movedLefState;
        private int movedRighState;
        private float moveLeftTimepoint;
        private float moveRightTimepoint;
        private float moveDownTimepoint;

        private void Start()
        {
            allowedToPutOnHold = true;
            movedLefState = 0;
            movedRighState = 0;
        }

        public void SetControllerTetromino(GameObject created)
        {
            tetromino = created;
            tetrominoTransform = tetromino.transform;
            tetrominoStats = tetromino.GetComponent<Tetromino>();
        }

        public void DropOnTick()
        {
            bool pass = true;

            if (tetrominoTransform.localPosition.y <= tetrominoStats.down)
            {
                tetrominoStats.LockTetromino();
                GameManager.Instance.SumPiece(this.playerName);
                SpawnNew();
                allowedToPutOnHold = true;
                pass = false;
            }
            else if (pass)
            {
                foreach (Transform childTile in tetrominoTransform)
                {
                    int x = (int)Mathf.Floor(childTile.position.x - transform.position.x);
                    int y = (int)Mathf.Floor(childTile.position.y - transform.position.y);

                    if (grid.CheckIfFieldEmpty(y - 1, x) == false)
                    {
                        tetrominoStats.LockTetromino();
                        GameManager.Instance.SumPiece(this.playerName);
                        allowedToPutOnHold = true;
                        SpawnNew();
                        pass = false;
                        break;
                    }
                }
            }
            if (pass)
            {
                tetrominoTransform.Translate(new Vector3(0, -1, 0), Space.World);
            }
        }

        void Update()
        {
            if (Input.GetKey(leftInput))
            {
                MoveLeft();
            }
            else
            {
                movedLefState = 0;
            }

            if (Input.GetKey(rightInput))
            {
                MoveRight();
            }
            else
            {
                movedRighState = 0;
            }

            if (Input.GetKey(downInput))
            {
                bool pass = true;

                if (tetrominoTransform.localPosition.y <= tetrominoStats.down)
                {
                    tetrominoStats.LockTetromino();
                    GameManager.Instance.SumPiece(this.playerName);
                    SpawnNew();
                    allowedToPutOnHold = true;
                    pass = false;
                }
                else if (pass)
                {
                    foreach (Transform childTile in tetrominoTransform)
                    {
                        int x = (int)Mathf.Floor(childTile.position.x - transform.position.x);
                        int y = (int)Mathf.Floor(childTile.position.y - transform.position.y);

                        if (grid.CheckIfFieldEmpty(y - 1, x) == false)
                        {
                            tetrominoStats.LockTetromino();
                            GameManager.Instance.SumPiece(this.playerName);
                            allowedToPutOnHold = true;
                            SpawnNew();
                            pass = false;
                            break;
                        }
                    }
                }
                if (pass && Time.time - moveDownTimepoint >= 0.08f)
                {


                    moveDownTimepoint = Time.time;

                    tetrominoTransform.Translate(new Vector3(0, -1, 0), Space.World);
                }

            }

            if (Input.GetKeyDown(dropInput))
            {
                int minY = int.MaxValue;

                foreach (Transform childTile in tetrominoTransform)
                {
                    int x = (int)Mathf.Floor(childTile.position.x - transform.position.x);
                    int y = (int)Mathf.Floor(childTile.position.y - transform.position.y);
                    int privY = (int)Mathf.Floor(childTile.position.y - transform.position.y) - grid.GetMaxAvailableHeight(y, x);
                    if (privY < minY)
                    {
                        minY = privY;
                    }

                }

                Vector2 position = tetrominoTransform.position;
                position.y = position.y - minY;
                tetrominoTransform.position = position;
                tetrominoStats.LockTetromino();
                allowedToPutOnHold = true;
                GameManager.Instance.SumPiece(this.playerName);
                SpawnNew();
            }

            if (Input.GetKeyDown(holdInput))
            {
                if (allowedToPutOnHold)
                {
                    tetrominoQueue.HoldTetromino();
                    allowedToPutOnHold = false;
                }
            }

            if (Input.GetKeyDown(rotateLeftInput))
            {
                if (string.Equals(tetrominoStats.tetName, "cube") == false)
                {
                    Rotate(90);
                    tetrominoStats.UpdateGhost();
                }
            }

            if (Input.GetKeyDown(rotateRightInput))
            {
                if (string.Equals(tetrominoStats.tetName, "cube") == false)
                {
                    Rotate(-90);
                    tetrominoStats.UpdateGhost();
                }
            }

        }

        private void MoveRight()
        {
            if (tetrominoTransform.localPosition.x >= grid.width - tetrominoStats.right)
            {
                movedRighState = 0;
                return;
            }

            if (movedRighState == 0 || (movedRighState == 1 && Time.time - moveRightTimepoint >= 0.23f)
                || (movedRighState == 2 && Time.time - moveRightTimepoint >= 0.013f))
            {
                if (movedRighState == 0)
                {
                    movedRighState = 1;
                }
                else
                {
                    movedRighState = 2;
                }

                moveRightTimepoint = Time.time;

                bool pass = true;
                foreach (Transform childTile in tetrominoTransform)
                {
                    int x = (int)Mathf.Floor(childTile.position.x - transform.position.x);
                    int y = (int)Mathf.Floor(childTile.position.y - transform.position.y);

                    if (grid.CheckIfFieldEmpty(y, x + 1) == false)
                    {
                        pass = false;
                    }
                }

                if (pass)
                {
                    tetrominoTransform.Translate(new Vector3(1, 0, 0), Space.World);
                    tetrominoStats.UpdateGhost();
                }
                else
                {
                    movedRighState = 0;
                }
            }
        }

        private void MoveLeft()
        {
            if (tetrominoTransform.localPosition.x <= tetrominoStats.left)
            {
                movedLefState = 0;
                return;
            }

            if (movedLefState == 0 || (movedLefState == 1 && Time.time - moveLeftTimepoint >= 0.23f)
                || (movedLefState == 2 && Time.time - moveLeftTimepoint >= 0.013f))
            {
                if (movedLefState == 0)
                {
                    movedLefState = 1;
                }
                else
                {
                    movedLefState = 2;
                }

                moveLeftTimepoint = Time.time;

                bool pass = true;
                foreach (Transform childTile in tetrominoTransform)
                {
                    int x = (int)Mathf.Floor(childTile.position.x - transform.position.x);
                    int y = (int)Mathf.Floor(childTile.position.y - transform.position.y);

                    if (grid.CheckIfFieldEmpty(y, x - 1) == false)
                    {
                        pass = false;
                    }
                }

                if (pass)
                {
                    tetrominoTransform.Translate(new Vector3(-1, 0, 0), Space.World);
                    tetrominoStats.UpdateGhost();
                }

                else
                {
                    movedLefState = 0;
                }
            }
            
        }

        private void Rotate(float degrees)
        {
            RotateAroundPivot(degrees);

            bool rotated = true;
            if (GridChecker.CheckRotationAvailabilityWitDisplacements(tetrominoTransform, tetrominoStats, grid, transform.position) == false)
            {
                RotateAroundPivot(-degrees);
                rotated = false;
            }

            int minY = int.MaxValue, minX = int.MaxValue, maxY = int.MinValue, maxX = int.MinValue;

            foreach (Transform childTile in tetrominoTransform)
            {
                if (rotated)
                {
                    childTile.transform.Rotate(0, 0, -degrees);
                }
                int x = (int)Mathf.Floor(childTile.position.x - transform.position.x);
                if (minX > x) minX = x;
                if (maxX < x) maxX = x;

                int y = (int)Mathf.Floor(childTile.position.y - transform.position.y);
                if (minY > y) minY = y;
                if (maxY < y) maxY = y;
            }

            tetrominoStats.up = maxY - (int)tetrominoTransform.localPosition.y + 1;
            tetrominoStats.down = (int)tetrominoTransform.localPosition.y - minY;
            tetrominoStats.left = (int)tetrominoTransform.localPosition.x - minX;
            tetrominoStats.right = maxX - (int)tetrominoTransform.localPosition.x + 1;
        }

        private void RotateAroundPivot(float degrees)
        {
            if (string.Equals(tetrominoStats.tetName, "long") == false)
            {
                Vector3 position = tetrominoTransform.position;
                position.x -= 0.5f;
                position.y -= 0.5f;
                tetrominoTransform.position = position;
                foreach (Transform childTile in tetrominoTransform)
                {
                    position = childTile.position;
                    position.x += 0.5f;
                    position.y += 0.5f;
                    childTile.position = position;
                }

            }

            tetrominoTransform.Rotate(0, 0, degrees);

            if (string.Equals(tetrominoStats.tetName, "long") == false)
            {
                Vector3 position = tetrominoTransform.position;
                position.x += 0.5f;
                position.y += 0.5f;
                tetrominoTransform.position = position;
                foreach (Transform childTile in tetrominoTransform)
                {
                    position = childTile.position;
                    position.x -= 0.5f;
                    position.y -= 0.5f;
                    childTile.position = position;
                }

            }
        }

        public void SpawnNew()
        {
            tetrominoQueue.NextTetromino();

            foreach (Transform childTile in tetrominoTransform)
            {
                int x = (int)Mathf.Floor(childTile.position.x - transform.position.x);
                int y = (int)Mathf.Floor(childTile.position.y - transform.position.y);
                if (grid.CheckIfFieldEmpty(y, x) == false)
                {
                    GameEnd?.Invoke(this.playerName);
                    foreach (Transform childTile2 in tetrominoTransform)
                    {
                        Destroy(childTile2.gameObject);
                    }
                    break;
                }
            }
        }

        public void CheckLinesMovedAbovePiece()
        {
            GridChecker.CheckLinesMovedAbovePiece(tetrominoTransform, grid, transform.position);
        }

    }
}
