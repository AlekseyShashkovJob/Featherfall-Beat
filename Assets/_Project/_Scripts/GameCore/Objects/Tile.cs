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
        private float _tileHeight;

        public void Initialize(TileType type, ObjectPool pool, float tileHeight)
        {
            _type = type;
            _pool = pool;
            _tileHeight = tileHeight;

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
                    int index = (int)_type - (int)TileType.Empty1;
                    _image.sprite = _emptySprites[index];
                    break;
            }
        }

        public void MoveDown(float speed)
        {
            transform.localPosition += Vector3.down * speed * Time.deltaTime;

            var parentRect = transform.parent as RectTransform;
            float containerHeight = parentRect != null ? parentRect.rect.height : Screen.height;
            float destroyThreshold = -containerHeight / 2 - _tileHeight;

            if (transform.localPosition.y < destroyThreshold)
            {
                _pool.ReturnObject(gameObject);
            }
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