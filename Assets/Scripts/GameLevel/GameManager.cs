using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject karePrefab;
    [SerializeField]
    private Transform karelerPaneli;

    [SerializeField]
    private Text soruText;

    [SerializeField]
    private Transform soruPaneli;

    [SerializeField]
    private Sprite[] kareSprites;

    [SerializeField]
    private GameObject sonucPaneli;

    private GameObject[] karelerDizisi = new GameObject[25];

    List<int> bolumDegerleriListesi = new List<int>();
    int bolunenSayi, bolenSayi;
    int kacinciSoru;
    int butonDegeri;
    int dogruSonuc;
    bool butonaBasilsinmi;
    int kalanHak;
    string sorununZorlukDerecesi;

    kalanHakManager kalanHakManager;
    puanManager puanManager;
    GameObject gecerliKare;

    [SerializeField]
    AudioSource audioSource;
    public AudioClip butonSesi;

    private void Awake()
    {
        kalanHak = 3;

        audioSource = GetComponent<AudioSource>();

        sonucPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;

        kalanHakManager = Object.FindObjectOfType<kalanHakManager>();
        kalanHakManager.KalanHaklariKontrolEt(kalanHak);

        puanManager = Object.FindObjectOfType<puanManager>();

    }

    void Start()
    {
        butonaBasilsinmi = false;
        soruPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;
        kareleriOlustur();

    }

    public void kareleriOlustur()
    {
        for (int i = 0; i < 25; i++)
        {
            GameObject kare = Instantiate(karePrefab, karelerPaneli);

            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprites[Random.Range(0, kareSprites.Length)];

            kare.transform.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi());
            karelerDizisi[i] = kare;
        }

        BolumDegerleriniTextYazdir();

        StartCoroutine(DoFadeRoutine());

        Invoke("SoruPaneliAc", 2f);
    }

    void ButonaBasildi()
    {
        if(butonaBasilsinmi==true)
        {
            audioSource.PlayOneShot(butonSesi);

            butonDegeri = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text);

            gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

            sonucuKontrolEt();
        }
    }

    void sonucuKontrolEt()
    {
        if(butonDegeri==dogruSonuc)
        {
            puanManager.puanArtir(sorununZorlukDerecesi);

            gecerliKare.transform.GetChild(1).GetComponent<Image>().enabled = true;
            gecerliKare.transform.GetChild(0).GetComponent<Text>().text = "";
            gecerliKare.transform.GetComponent<Button>().interactable = false;

            if(bolumDegerleriListesi.Count>0)
            {
                SoruPaneliAc();
            }
            else
            {
                oyunBitti();
            }
            bolumDegerleriListesi.Remove(kacinciSoru);
        }
        else
        {
            kalanHak--;
            kalanHakManager.KalanHaklariKontrolEt(kalanHak);
        }

        if(kalanHak<=0)
        {
            oyunBitti();
        }
    }

    void oyunBitti()
    {
        butonaBasilsinmi = false;
        sonucPaneli.GetComponent<RectTransform>().DOScale(1, 0.5f);
    }

    IEnumerator DoFadeRoutine()
    {
        foreach (var kare in karelerDizisi)
        {
            kare.GetComponent<CanvasGroup>().DOFade(1, 0.7f);
            yield return new WaitForSeconds(0.1f);
        } 
    }

    void BolumDegerleriniTextYazdir()
    {
        foreach (var kare in karelerDizisi)
        {
            int randomSayi = Random.Range(1, 13);
            bolumDegerleriListesi.Add(randomSayi);
            kare.transform.GetChild(0).GetComponent<Text>().text = randomSayi.ToString();
        }
    }

    void SoruPaneliAc()
    {
        soruyuSor();
        butonaBasilsinmi = true;
        soruPaneli.GetComponent<RectTransform>().DOScale(1, 0.5f);
    }

    void soruyuSor()
    {
        bolenSayi = Random.Range(2, 11);
        kacinciSoru = Random.Range(0, bolumDegerleriListesi.Count);

        dogruSonuc = bolumDegerleriListesi[kacinciSoru];

        bolunenSayi = bolenSayi * dogruSonuc;

        if(bolunenSayi<=40)
        {
            sorununZorlukDerecesi = "kolay";
        }
        else if(bolunenSayi > 40 && bolunenSayi <= 80)
        {
            sorununZorlukDerecesi = "orta";
        }
        else
        {
            sorununZorlukDerecesi = "zor";
        }

        soruText.text = bolunenSayi.ToString() + " : " + bolenSayi.ToString();
    }
}
