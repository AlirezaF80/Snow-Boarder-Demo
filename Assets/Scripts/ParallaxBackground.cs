using UnityEngine;

public class ParallaxBackground : MonoBehaviour {
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private bool initPosToCamera = true;
    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;
    private float _textureUnitSizeX;
    private float _textureUnitSizeY;

    void Start() {
        _cameraTransform = Camera.main.transform;
        _lastCameraPosition = _cameraTransform.position;
        if (initPosToCamera) {
            transform.position =
                new Vector3(_cameraTransform.position.x, _cameraTransform.position.y, transform.position.z);
        }

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        _textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        _textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    void LateUpdate() {
        var deltaMovement = _cameraTransform.position - _lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x,
            deltaMovement.y * parallaxEffectMultiplier.y);
        _lastCameraPosition = _cameraTransform.position;

        Vector3 newTransformPos = transform.position;
        if (Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureUnitSizeX) {
            float offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureUnitSizeX;
            newTransformPos = new Vector3(_lastCameraPosition.x + offsetPositionX, newTransformPos.y);
        }

        if (Mathf.Abs(_cameraTransform.position.y - transform.position.y) >= _textureUnitSizeY) {
            float offsetPositionY = (_cameraTransform.position.y - transform.position.y) % _textureUnitSizeY;
            newTransformPos = new Vector3(newTransformPos.x, _lastCameraPosition.y + offsetPositionY);
        }

        transform.position = newTransformPos;
    }
}