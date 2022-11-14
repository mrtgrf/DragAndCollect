using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameObject[] Toplar;
    public GameObject FirePoint;
    [SerializeField] private float TopGucu;
    int AktifTopIndex;
    public Animator _topatar;
    public ParticleSystem Efekt;
    

    [SerializeField] private int HedefTopSayisi;
    [SerializeField] private int MevcutTopSayisi;
    int GirenTopsayisi;
    public Slider LevelSlider;
    public TextMeshProUGUI KalanTopSayisi_Text;

    public GameObject[] Paneller;
    public TextMeshProUGUI YildizSayisi;
    public TextMeshProUGUI Kazandin_LevelSayisi;
    public TextMeshProUGUI Kaybettin_LevelSayisi;

    public Renderer KovaSeffaf;
    float KovaBaslangicDegeri;
    float KovaStepdeger;

    bool effectPlaying = false;
    float effectDeltaTime = 0.0f;
    float effectMaxTime = 1.0f;

    void Start()
    {
        

        LevelSlider.maxValue = HedefTopSayisi;
        KovaBaslangicDegeri = .5f;

        KovaStepdeger = .25f / HedefTopSayisi;
        

        LevelSlider.maxValue = HedefTopSayisi;

        //KovaSeffaf.material.SetTextureScale("_MainTex", new Vector2(1f, .30f));
    }


    public void ParcEfekt(Vector3 Pozisyon) 
    {
        Efekt.transform.position = Pozisyon;
        Efekt.gameObject.SetActive(true);
        Efekt.Play();
        effectPlaying = true;
    }
    public void StopEffect()
    {
        effectDeltaTime += Time.deltaTime;

        if(effectDeltaTime >= effectMaxTime)
        {
            effectDeltaTime = 0;
            effectPlaying = false;
            Efekt.Stop();
        }
    }

    public void TopGirdi()
    {
        GirenTopsayisi++;
        LevelSlider.value = GirenTopsayisi;
        MevcutTopSayisi--;

        KovaBaslangicDegeri -= KovaStepdeger;

        KovaSeffaf.material.SetTextureScale("_MainTex", new Vector2(1f, KovaBaslangicDegeri));
        if (GirenTopsayisi == HedefTopSayisi)
        {
            PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("Yildiz", PlayerPrefs.GetInt("Yildiz") + 15);
            YildizSayisi.text = PlayerPrefs.GetInt("Yildiz").ToString();
            Kazandin_LevelSayisi.text = "Level : "  +  SceneManager.GetActiveScene().name;

            Paneller[1].SetActive(true);
        }


    }

    public void TopGirmedi()
    {

    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _topatar.Play("TopAtar");


            Toplar[AktifTopIndex].transform.SetPositionAndRotation(FirePoint.transform.position, FirePoint.transform.rotation);
            Toplar[AktifTopIndex].SetActive(true);
            Toplar[AktifTopIndex].GetComponent<Rigidbody>().AddForce(Toplar[AktifTopIndex].transform.TransformDirection(90, 90, 0)
                        * TopGucu, ForceMode.Force);

            if (Toplar.Length - 1 == AktifTopIndex)
            {
                AktifTopIndex = 0;
            }
            else
                AktifTopIndex++;
        }

        if (effectPlaying)
        {
            StopEffect();
        }
    }



    public void OyunuDurdur()
    {
        Paneller[0].SetActive(true);
        Time.timeScale = 0; 
    }
    
    public void PanellericinButonislemi(string islem)
    {
        switch (islem)
        {
            case "Devamet":
                Time.timeScale = 1;
                Paneller[0].SetActive(false);
                break;
            case "Cikis":
                Application.Quit();
                break;
            case "Ayarlar":
                break;
            case "Tekrar":
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case "Next":
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;



        }

    }

}
