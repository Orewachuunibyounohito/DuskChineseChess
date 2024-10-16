using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChineseChess.Chesses;
using ChineseChess.SO;
using ChineseChess.Tools;
using ChuuniExtension;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChineseChess
{
    public class CheckerBoardView
    {
        private const int UV_INDEX = 1;
        private static readonly GameObject chessPrefab;
        private static readonly Transform previewChessPrefab;
        private static readonly Transform previewRampagePrefab_x;
        private static readonly Transform previewRampagePrefab_y;
        private static readonly Transform collection;
        private static readonly Dictionary<ChessType, Dictionary<ChessCamp, Material>> materialContents;

        private Dictionary<Chess, Transform> chessTransforms = new Dictionary<Chess, Transform>();
        private List<GameObject> previewChesses;
        private Vector3 yOffset = new Vector3(0, 1.21f, 0);

        static CheckerBoardView(){
            var prefabsSettings = Resources.Load<PrefabsSettings>(ResourcesPath.PREFABS_SETTINGS);
            var materialSettings = Resources.Load<MaterialsSettings>(ResourcesPath.MATERIALS_SETTINGS);
            chessPrefab = prefabsSettings.Chess;
            previewChessPrefab = prefabsSettings.PreviewChess.transform;
            previewRampagePrefab_x = prefabsSettings.PreviewRampage_x.transform;
            previewRampagePrefab_y = prefabsSettings.PreviewRampage_y.transform;
            Transform ObjectGruop = RootGroup.Object;
            collection = new GameObject("ChessCollection").transform;
            collection.parent = ObjectGruop;
            materialContents = new();
            foreach(var content in materialSettings.chessMaterials){
                materialContents.Add(content.chess, new(){
                    { ChessCamp.Red, content.red },
                    { ChessCamp.Black, content.black }
                });
            }
        }

        public void Add(CheckerBoard model, Chess chess, Vector2Int mappedPosition){
                var chessTrans = Object.Instantiate(chessPrefab, yOffset, chessPrefab.transform.rotation, collection).transform;
                var meshRenderer = chessTrans.GetComponent<MeshRenderer>();
                meshRenderer.material = materialContents[chess.Type][chess.Camp];
                chessTrans.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                if(chess.Camp == ChessCamp.Black){
                    chessTrans.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                }
                var item = chessTrans.gameObject.AddComponent<CheckerBoardItem>();
                item.SetBoard(model).SetBoardView(this).SetPosition(mappedPosition);
                chess.Moved += item.OnMoved;
                chess.MovedWithCommand += item.OnMoved;
                chess.ChessDefeated += DestroyChessView;
                chessTransforms.Add(chess, chessTrans);
                UpdateChessView(chess, mappedPosition);
        }

        public void UpdateChessView(Chess chess, Vector2Int newPosition){
            chessTransforms[chess].position = ChessPositionMappingTool.PositionToView(newPosition);
        }
        public void UpdateChessViewWithCommand(Chess chess, Vector2Int newPosition){
            MoveMode mode = MoveMode.Walk;
            ChessMoving moving = chessTransforms[chess].GetComponent<ChessMoving>();
            moving.SetDestination(ChessPositionMappingTool.PositionToView(newPosition))
                  .SetDuration(1f)
                  .StartMoving(mode.ChessMoveMode(chess.Type));
        }

        public void DestroyChessView(Chess chess){
            chessTransforms[chess].gameObject.AddComponent<AddForceByCollided>();
            ChessSystem.CoroutineExecutor.StartCoroutine(DestroyWhenCommandFinishedTask(chess));
        }

        private IEnumerator DestroyWhenCommandFinishedTask(Chess chess){
            while(!ChessCommandInvoker.Instance.CurrentCommand.IsFinished){
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(1f);
            Object.Destroy(chessTransforms[chess].gameObject);
            chessTransforms.Remove(chess);
        }

        public void ShowReachable(CheckerBoard model, Chess chess){
            previewChesses = new();
            var reachables = chess.FindOutReachable(model);
            var item = chessTransforms[chess].GetComponent<CheckerBoardItem>();
            foreach(var content in reachables){
                var previewPosition = ChessPositionMappingTool.PositionMapping(chess.NextPosition(content.Direction, content.Steps), chess.Camp);
                GameObject previewObject = Object.Instantiate(previewChessPrefab,
                                                              ChessPositionMappingTool.PositionToView(previewPosition),
                                                              previewChessPrefab.rotation,
                                                              collection)
                                                 .gameObject;
                var movementCommandContent = new MovementCommandContent(chess, content);
                var movementCommands = new List<ICommand>{ CommandFactory.GenerateMovement(movementCommandContent, item) };
                item.AddObjectCommands(previewObject, movementCommands);
                previewChesses.Add(previewObject);
            }
        }
        public void ShowRampageDirection(CheckerBoard model, Chess chess){
            previewChesses = new();
            var item = chessTransforms[chess].GetComponent<CheckerBoardItem>();
            var rampageDirection = chess.FindOutRampageDirection(model, item);
            foreach(var content in rampageDirection){
                var previewPosition = ChessPositionMappingTool.PositionMapping(chess.NextPosition(content.Direction, 1), chess.Camp);
                GameObject previewObject = InstantiatePreviewWithDirection(item, previewPosition, content.Direction);
                item.AddObjectCommands(previewObject, new List<ICommand> { content });
                previewChesses.Add(previewObject);
            }
        }

        private static GameObject InstantiatePreviewWithDirection(CheckerBoardItem item, Vector2Int previewPosition, DirectionType direction){
            Transform prefab = previewRampagePrefab_y;
            var angle = 0f;
            switch(direction){
                case DirectionType.上: break;
                case DirectionType.下: angle = 180; break;
                case DirectionType.左: angle = 270; prefab = previewRampagePrefab_x; break;
                case DirectionType.右: angle =  90; prefab = previewRampagePrefab_x; break;
                default:
                    throw new System.Exception("Uncognization Direction.");
            }
            var previewObject = Object.Instantiate(prefab,
                                                   ChessPositionMappingTool.PositionToView(previewPosition),
                                                   prefab.rotation,
                                                   item.transform)
                                      .gameObject;
            if (item.Chess.Camp == ChessCamp.Black){ angle += 180; }
            previewObject.transform.Rotate(0, angle, 0);
            return previewObject;
        }

        public void HideReachable(){
            foreach(var preview in previewChesses){
                Object.Destroy(preview);
            }
            previewChesses.Clear();
        }

        public void IntoPlayingState(){
            foreach(var chessTrans in chessTransforms.Values){
                chessTrans.GetComponent<Rigidbody>().isKinematic = true;
                chessTrans.GetComponent<Collider>().isTrigger = true;
            }
        }
        public float GetChessY() => chessTransforms.First().Value.position.y;
    }
}
