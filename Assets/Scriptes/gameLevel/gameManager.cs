using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
   [SerializeField]
   private GameObject karePrefab;
   [SerializeField]
   private Transform karelerPaneli;
   [SerializeField]
   private Text soruText;


   private GameObject[] karelerDizisi = new GameObject[25];

   [SerializeField]
   private Transform soruPaneli;

   [SerializeField]
   private Sprite[] kareSprites;

   List<int> bolumDegerleriListesi = new List<int>();
   
   int bolenSayi, bolunenSayi, kacinciSoru;
   int dogruSonuc;
   
   int butonDegeri;

   bool butonaBasilsinMi;

   int kalanHak;

   string sorununZorlukDerecesi;

   KalanHakManager KalanHakManager;

   PuanManager puanManager;

   GameObject gecerliKare;

   [SerializeField]
   private GameObject sonucPaneli;

   private void Awake()
   {
       kalanHak = 3;

       sonucPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;

       KalanHakManager = Object.FindObjectOfType<KalanHakManager>();

       puanManager = Object.FindObjectOfType<PuanManager>();

       KalanHakManager.KalanHaklariKontrolEt(kalanHak);
   }



    void Start()
    {
        butonaBasilsinMi = false;
        
        karaleriOlustur();

        soruPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void karaleriOlustur()
    {
        for(int i=0;i<25;i++)
        {
            GameObject kare = Instantiate(karePrefab, karelerPaneli);
            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprites[Random.Range(0, kareSprites.Length)];
            kare.transform.GetComponent<Button>().onClick.AddListener(()=> ButonaBasildi());
            karelerDizisi[i] = kare;
        
        }

        BolumDegerleriniTexteYazdir();

        StartCoroutine(DoFadeRoutine());

        Invoke("SoruPaneliAc",2f);
    }

    void ButonaBasildi()
    {
        if(butonaBasilsinMi)
        {
            butonDegeri = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text);

            gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        
            SonucuKontrolEt();
        }

        
    }

    void SonucuKontrolEt()
    {
        if(butonDegeri==dogruSonuc)
        {
            gecerliKare.transform.GetChild(1).GetComponent<Image>().enabled = true;
            gecerliKare.transform.GetChild(0).GetComponent<Text>().text = "";
            gecerliKare.transform.GetComponent<Button>().interactable = false;

            puanManager.PuaniArtir(sorununZorlukDerecesi);

            bolumDegerleriListesi.RemoveAt(kacinciSoru);
            
            if(bolumDegerleriListesi.Count>0)
            {
                SoruPaneliAc();
            } else
            {
                OyunBitti();
            }

            
        } else
        {
            kalanHak--;
            KalanHakManager.KalanHaklariKontrolEt(kalanHak);
        }

        if(kalanHak<=0)
        {
            OyunBitti();
        }
    }

    void OyunBitti()
    {
        butonaBasilsinMi = false;
        
        sonucPaneli.GetComponent<RectTransform>().DOScale(1,0.3f).SetEase(Ease.OutBack);

    }

    IEnumerator DoFadeRoutine()
    {
        foreach(var kare in karelerDizisi)
        {
            kare.GetComponent<CanvasGroup>().DOFade(1,0.2f);

            yield return new WaitForSeconds(0.07f);
        }
    }

    void BolumDegerleriniTexteYazdir()
    {
        foreach(var kare in karelerDizisi)
        {
            int rastegeleDeger = Random.Range(2,13);

            bolumDegerleriListesi.Add(rastegeleDeger);

            kare.transform.GetChild(0).GetComponent<Text>().text = rastegeleDeger.ToString();
        }
    }

    void SoruPaneliAc()
    {
        SoruyuSor();

        butonaBasilsinMi = true;

        soruPaneli.GetComponent<RectTransform>().DOScale(1,0.3f).SetEase(Ease.OutBack);
    }

    void SoruyuSor()
    {
        bolenSayi = Random.Range(2,11);

        kacinciSoru = Random.Range(0,bolumDegerleriListesi.Count);

        dogruSonuc = bolumDegerleriListesi[kacinciSoru];
        
        bolunenSayi = bolenSayi * dogruSonuc;

        if(bolunenSayi<=40)
        {
            sorununZorlukDerecesi = "kolay";
        } else if(bolunenSayi>40 && bolunenSayi<=80)
        {
            sorununZorlukDerecesi = "orta";
        } else 
        {
            sorununZorlukDerecesi = "zor";
        }

        soruText.text = bolunenSayi.ToString() + " : " + bolenSayi.ToString();
    }

    
}
