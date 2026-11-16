using System.Collections; //Özel zamanlama iþlemleri için kullanýlýr. Coroutine/ IEnumerator
using UnityEngine; // : Monobehaviour (script'i sürükleyip Inspector'a component olarak takýlabilmek için; Avake(), Start() gibi fonksiyonlarý da içerir)
using UnityEngine.EventSystems; //Týklama, sürükleme gibi kullanýcý etkileþimleri için olan sistem. Raycast týklamasýnýn yakalanabilmesi için zorunlu.
// Baþýnda I olan yapýlar class deðil interface. C# da tek class dan kalýtým alýnýr ama istediðin kadar interface uygulanabilir.
public class SimpleTarget : MonoBehaviour, IPointerClickHandler // Sýnýfa týklanma özelliði ekler. Kameradaki Raycaster sahneye ýþýn atýp collider'a çarptýðýnda EventSystem objede olup olmadýðýna bakar.
{
    // [Header], Inspector paneline kategori baþlýðý atar.
    [Header("Sprites")]
    public Sprite saucedSprite;

    [Header("Timings")]
    //[Tooltip], Inspector'da deðiþken üzerine fare imleci ile gelindiðinde açýlan ipucu kutucuðudur.
    [Tooltip("Týklanmazsa ekranda kaç saniye kalýp kaybolsun")]
    public float lifetime;

    [Tooltip("Target kaç saniye sonra kaybolsun")]
    public float delayBeforeDestroy = 0.4f;

    [Header("Score")]
    public int points; // Her baþarýlý týklamadda verilecek puan

    //private yapýnca inspector panelinde gözükmezler.
    private SpriteRenderer spriteRenderer;
    private Collider2D targetCollider;
    private bool isClicked = false;
    private Coroutine autoDestroyCoroutine;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        autoDestroyCoroutine = StartCoroutine(AutoDestroyRoutine()); //Ýçindeki zamana göre zamanlayýcý olarak baþlar.
    }

    IEnumerator AutoDestroyRoutine()//IEnumerator, bu fonksiyonun adým adým yürütüleceðini ve duraklatýlabileceðini belirtir.
    {
        yield return new WaitForSeconds(lifetime); //Lifetime boyunca orda beklemesi ve diðer targetlerin gelmesini engellememek için

        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData) //Eventsystem aracýlýðý ile gönderilen veri paketidir. Farenin hangi tuþu ile týklandýðý, týklamýnýn ekrandaki koordinatý gibi
    {
        if (isClicked) return; // Çift týklamanýn engellenmesi için
        isClicked = true;

        
        if (autoDestroyCoroutine != null)
        {
            StopCoroutine(autoDestroyCoroutine);// Otomatik silinme geri sayýmýný durdur
        }

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(points);
        }

        StartCoroutine(ApplySauceAndDestroy()); //Efekt için zamanlayýcý
    }

    IEnumerator ApplySauceAndDestroy() 
    {
        if (targetCollider != null)
        {
            targetCollider.enabled = false; //Targetteki box collider devre dýþý kalýr böylece efekt girdiðinde tekrar týklanamaz hale gelir.
        }

        if (saucedSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = saucedSprite;
        }

        yield return new WaitForSeconds(delayBeforeDestroy);

        Destroy(gameObject);
    }
}