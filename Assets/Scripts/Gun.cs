using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    //патроны
    [SerializeField]
    private float maxAmmo, damage, range, shootSpeed, life;
    private float currentAmmo;
    //эффект выстрела
    [SerializeField]
    private ParticleSystem shootEffect;
    //аудио
    [SerializeField]
    private AudioClip audioRecharge, audioFire;
    //Эффект попадания
    [SerializeField]
    private GameObject hitEffect;
    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private EnemyDamage enemyDamageRed, enemyDamageBlack, enemyDamageGreen;

    [SerializeField]
    private LayerMask layerMask;
    //отображение руки
    [SerializeField]
    private Image handImage;
    //тексты на панеле
    [SerializeField]
    private Text currentAmmoText, maxAmmoText, lifeText, dieText, scoreText;
    //противники
    [SerializeField]
    private GameObject cube1, cube2, cube3, bomb, finishPanel, charactersPanel;
    [SerializeField]
    private int lifeEnemyE, lifeEnemyN, lifeEnemyH, score;

    public Vector3 coordinate;

    public Vector3 center, centerBomb;
    // Start is called before the first frame update
    void Start()
    {
        handImage.gameObject.SetActive(false);
        currentAmmo = 30;
        maxAmmo = 10;
        life = 100;
        range = 100f;
        score = 0;
        lifeEnemyE = lifeEnemyN = lifeEnemyH = 100;
        finishPanel.gameObject.SetActive(false);
        charactersPanel.gameObject.SetActive(true);
        RandomGenerate();
    }

    public void RandomGenerate()
    {
        Vector3 pos1 = center + new Vector3(
            UnityEngine.Random.Range(-coordinate.x / 2, coordinate.x / 2),
            UnityEngine.Random.Range(-coordinate.y / 2, coordinate.y / 2),
            UnityEngine.Random.Range(-coordinate.z / 2, coordinate.z / 2));
        Instantiate(cube1, pos1, Quaternion.identity);

        Vector3 pos2 = center + new Vector3(
            UnityEngine.Random.Range(-coordinate.x / 2, coordinate.x / 2),
            UnityEngine.Random.Range(-coordinate.y / 2, coordinate.y / 2),
            UnityEngine.Random.Range(-coordinate.z / 2, coordinate.z / 2));
        Instantiate(cube2, pos2, Quaternion.identity);

        Vector3 pos3 = center + new Vector3(
            UnityEngine.Random.Range(-coordinate.x / 2, coordinate.x / 2),
            UnityEngine.Random.Range(-coordinate.y / 2, coordinate.y / 2),
            UnityEngine.Random.Range(-coordinate.z / 2, coordinate.z / 2));
        Instantiate(cube3, pos3, Quaternion.identity);

        for(int i=0; i<10; i++)
        {
            Vector3 pos4 = centerBomb + new Vector3(
                UnityEngine.Random.Range(-coordinate.x / 2, coordinate.x / 2),
                UnityEngine.Random.Range(-coordinate.y / 2, coordinate.y / 2),
                UnityEngine.Random.Range(-coordinate.z / 2, coordinate.z / 2));
            Instantiate(bomb, pos4, Quaternion.identity);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //нажата лкм
        if(Input.GetButtonDown("Fire1"))
        {
            //аудио при выстреле
            GetComponent<AudioSource>().PlayOneShot(audioFire);
            if (maxAmmo>0)
            {
                Shoot();
                currentAmmo--;
                currentAmmoText.text = currentAmmo.ToString();
                if(currentAmmo<=0 || Input.GetKeyDown(KeyCode.R))
                {
                    GetComponent<AudioSource>().PlayOneShot(audioRecharge);
                    maxAmmo--;
                    currentAmmo = 30;
                    currentAmmoText.text = currentAmmo.ToString();
                    maxAmmoText.text = maxAmmo.ToString();
                }
            }
        }

        //Для подбора патрона............
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit rayHit;
        // смотрим на патроны и показываем панель с рукой
        if (Physics.Raycast(ray, out rayHit, 2f, layerMask))
        {
            // если рука не отображается
            if (!handImage.gameObject.activeSelf && rayHit.transform.tag == "Ammo")
            {
                // показать картинку
                handImage.gameObject.SetActive(true);
            }
            // если нажата клавиша Е
            if (Input.GetKeyDown(KeyCode.E))
            {
                // если смотрим на патроны
                if (rayHit.transform.tag == "Ammo")
                {
                    maxAmmo += 1;
                    if (maxAmmo > 10)
                        maxAmmo = 10;
                    maxAmmoText.text = maxAmmo.ToString();
                    // уничтожить патроны
                    Destroy(rayHit.transform.gameObject);
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Отключаем панель
                handImage.gameObject.SetActive(false);
            }
        }
        else
        {
            //выключаем картинку
            handImage.gameObject.SetActive(false);
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(
            playerCamera.transform.position, //откуда луч выходит
            playerCamera.transform.forward, //в каком напрвлении
            out hit,//куда записать информацию о том, во что попал
            range))
        {
            // эффект выстрела
            shootEffect.Play();
            

            //эффект попадания
            GameObject effect = Instantiate(
                hitEffect, // что создать
                hit.point, //где создать
                Quaternion.LookRotation(hit.normal)); // как повернуть
            Destroy(effect, 1f);

            Debug.Log("hit name" + hit.collider.name);

            // попадание в объекты
            if(hit.collider.tag == "EnemyEasy")
            {
                //нанести урон
                lifeEnemyE -= 25;

                if (lifeEnemyE<=0)
                {
                    score += 5;
                    scoreText.text = score.ToString();
                    Destroy(hit.collider.gameObject,1f);
                }
            }
            if (hit.collider.tag == "EnemyNormal")
            {
                //нанести урон
                lifeEnemyN -= 20;

                if (lifeEnemyN <= 0)
                {
                    score += 10;
                    scoreText.text = score.ToString();
                    Destroy(hit.collider.gameObject,1f);
                }
            }
            if (hit.collider.tag == "EnemyHard")
            {
                //нанести урон
                lifeEnemyH -= 10;
                if (lifeEnemyH <= 0)
                {
                    score += 15;
                    scoreText.text = score.ToString();
                    Destroy(hit.collider.gameObject, 1f);
                }
            }
        }
    }

    public void Triggers(int damageEnemy)
    {
        Debug.Log("Triggered");
        if(life>0)
        {
            damage = damageEnemy;
            life -= damage;
            lifeText.text = life.ToString();
        }
        else
        {
            finishPanel.gameObject.SetActive(true);
            charactersPanel.gameObject.SetActive(false);
            dieText.text = "YOU LOSE!!!";
            Invoke("RestartGame", 5f);
        }
    }

    private void RestartGame()
    {
        //перезапускаем уровень 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    void OnDrawGizmosSelectes()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.localPosition + center, coordinate);
    }
}
