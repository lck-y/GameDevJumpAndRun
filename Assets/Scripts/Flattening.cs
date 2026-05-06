using DG.Tweening;
using UnityEngine;

public class Flatten : MonoBehaviour
{
    [SerializeField]private AudioSource deathSound;
    private bool flattened = false;

    public void Animate()
    {
        if (flattened) return;
        flattened = true;

        var Enemy = GetComponent<EnemyMovement>();
        if (Enemy != null) {Enemy.enabled = false;}
        if (deathSound != null){deathSound.Play();}
        
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScaleY(0.0f, 2.0f));
        sequence.Join(transform.DOMoveY(transform.position.y - transform.localScale.y / 10f, 2.0f));
        sequence.OnComplete(() =>
        {
            if (this != null && gameObject != null)
                Destroy(gameObject);
        });
    }
}