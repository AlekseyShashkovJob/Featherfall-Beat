using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GameCore.Objects
{
    public class Tile : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _blackChicken;
        [SerializeField] private Sprite _grayChicken;
        [SerializeField] private Sprite _whiteChicken;
        [SerializeField] private Sprite[] _emptySprites;

        private TileType _type;
        private ObjectPool _pool;

        public void Initialize(TileType type, ObjectPool pool)
        {
            _type = type;
            _pool = pool;

            switch (_type)
            {
                case TileType.ChickenBlack:
                    _image.sprite = _blackChicken;
                    break;
                case TileType.ChickenGray:
                    _image.sprite = _grayChicken;
                    break;
                case TileType.ChickenWhite:
                    _image.sprite = _whiteChicken;
                    break;
                case TileType.Empty1:
                case TileType.Empty2:
                case TileType.Empty3:
                    _image.sprite = _emptySprites[(int)_type - (int)TileType.Empty1];
                    break;
            }
        }

        public void ReturnToPool()
        {
            _pool.ReturnObject(gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!GameManager.Instance.IsGameActive) return;

            switch (_type)
            {
                case TileType.ChickenWhite:
                    GameManager.Instance.FinishGame();
                    break;

                case TileType.ChickenBlack:
                case TileType.ChickenGray:
                    GameManager.Instance.AddPoints(1);
                    _type = TileType.ChickenWhite;
                    _image.sprite = _whiteChicken;
                    break;

                case TileType.Empty1:
                case TileType.Empty2:
                case TileType.Empty3:
                    break;
            }
        }
    }

    public enum TileType
    {
        ChickenBlack,
        ChickenGray,
        ChickenWhite,
        Empty1,
        Empty2,
        Empty3
    }
}