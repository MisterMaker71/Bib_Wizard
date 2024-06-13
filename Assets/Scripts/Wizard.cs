using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Wizard : MonoBehaviour
{
    public static Wizard wizard;
    public Transform visualModel;
    public float mana = 100;
    public float health = 100;
    public List<MovePoint> layerChange = new List<MovePoint>();
    public Tilemap map;
    public Tilemap HightMap;
    public TileBase Ramp;
    public float z = 0;
    Vector2 movementVector;
    Animator animator;
    public GameObject fierball;
    public Transform wand;
    //public Transform ffff;
    public TileBase tileBelowPlayer;
    Vector2 lastMovedir;
    PointManager pointManager;
    float blink;
    public PlayerStats PlayerStats;
    public float xp = 0;
    public int level = 0;
    [Header("Skill Points")]
    [Space(10)]
    public int skillPoints = 0;
    int usedskillPoints = 0;
    public int UsedskillPoints { get { return usedskillPoints; } }
    [Space(5)]
    public int PointsInManaRegen = 0;
    public int PointsInMana = 0;
    public int PointsInHealth = 0;
    public int PointsInHealthRegen = 0;
    public int PointsInMovementSpeed = 0;
    public int PointsInSpellSpeed = 0;
    public int PointsInDamage = 0;
    void Start()
    {
        wizard = this;
        animator = GetComponent<Animator>();
        pointManager = FindObjectOfType<PointManager>();
    }
    void Update()
    {
        if (Time.timeScale > 0.1f)
        {

            usedskillPoints = PointsInManaRegen + PointsInMana + PointsInHealth + PointsInHealthRegen + PointsInMovementSpeed + PointsInSpellSpeed + PointsInDamage;

            if (wizard == null)
                wizard = this;
            animator.SetFloat("CastingSpeed", PlayerStats.CastingSpeed * GetRedusedValue(PointsInSpellSpeed, 2));

            if (mana < PlayerStats.MaxMana * GetRedusedValue(PointsInMana, 2))
                mana += Time.deltaTime * PlayerStats.ManaRegen * GetRedusedValue(PointsInManaRegen, 3);
            else
                mana = PlayerStats.MaxMana * GetRedusedValue(PointsInMana, 2);

            if (health < PlayerStats.MaxHealth * GetRedusedValue(PointsInHealth, 2))
                health += Time.deltaTime * PlayerStats.HealthRegen * GetRedusedValue(PointsInHealthRegen, 3);
            else
                health = PlayerStats.MaxHealth * GetRedusedValue(PointsInHealth, 2);

            pointManager.helth.value = health;
            pointManager.mana.value = mana;
            pointManager.helth.maxValue = PlayerStats.MaxHealth * GetRedusedValue(PointsInHealth, 2);
            pointManager.mana.maxValue = PlayerStats.MaxMana * GetRedusedValue(PointsInMana, 2);


            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (Camera.main.orthographicSize > 3)
                    Camera.main.orthographicSize--;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (Camera.main.orthographicSize < 15)
                    Camera.main.orthographicSize++;
            }

            GetComponentInChildren<SpriteRenderer>().sortingOrder = (int)(Mathf.Ceil(z + 1));

            //float wihte = (map.GetComponent<TilemapRenderer>().sortingOrder + 4 - z) / 4;
            //map.color = new Color(wihte, wihte, wihte);
            //if (map.GetComponent<TilemapRenderer>().sortingOrder == Mathf.RoundToInt(z))
            //{
            tileBelowPlayer = GetZTile(new Vector3(visualModel.position.x, visualModel.position.y - 1.5f, visualModel.position.z), map);



            if (HightMap.GetTile(HightMap.WorldToCell(transform.position)) != null)
            {
                //print(HightMap.GetTile(HightMap.WorldToCell(transform.position)).name);
                float hight = Convert.ToInt32(HightMap.GetTile(HightMap.WorldToCell(transform.position)).name);
                z = hight * 0.8f;
            }



            //foreach (MovePoint pos in layerChange)
            //{
            //    if (pos.position == map.WorldToCell(transform.position) && pos.layer == z)
            //        if (pos.up)
            //            z++;
            //        else
            //            z--;
            //}
            //}
            /*z =*/
            //print(visualModel.position);

            visualModel.position = Vector3.MoveTowards(visualModel.position, transform.position + Vector3.up * (z + 1.5f), Time.deltaTime * 10);

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            //transform.position += (Vector3.right * horizontal + Vector3.up * vertical) * Time.deltaTime * MovementSpeed;
            movementVector = ((Vector3.right * horizontal + Vector3.up * vertical)).normalized;
            transform.position += new Vector3(movementVector.x, movementVector.y, 0) * Time.deltaTime * PlayerStats.MovementSpeed;

            //print(((Vector3.right * horizontal + Vector3.up * vertical) * Time.deltaTime * MovementSpeed).normalized);

            Vector3 look = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (look.x < transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);


            //if (movementVector != Vector2.zero)
            //{
            //    lastMovedir = movementVector;
            //}
            //if(lastMovedir.x == 1 || lastMovedir.x == -1)
            //transform.localScale = new Vector3(lastMovedir.x, transform.localScale.y, transform.localScale.z);


            blink += Time.deltaTime;
            if (animator != null)
            {
                animator.SetBool("move", movementVector != Vector2.zero);
                if (blink > 1.3f && blink < 2f)
                    animator.SetBool("Blink", true);
                if (blink > 2)
                {
                    blink = UnityEngine.Random.Range(-1.0f, 0.0f);
                    animator.SetBool("Blink", false);
                }

                if (Input.GetMouseButton(0))
                //if (Input.GetKey(KeyCode.Space))
                {
                    animator.SetBool("atack", true);
                }
                else
                {
                    animator.SetBool("atack", false);
                }
            }


            if (!(Vector3.Distance(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0), visualModel.position) < 0.2f))// && movementVector == Vector2.zero
            {
                //Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
                Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(visualModel.position.x, visualModel.position.y, -10),
                    Time.deltaTime * Vector3.Distance(visualModel.position, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0)) * 2);
            }
        }
    }
    public enum PointsTypes { PointsInManaRegen, PointsInMana, PointsInHealth, PointsInHealthRegen, PointsInMovementSpeed, PointsInSpellSpeed, PointsInDamage }
    public void AsingnPoint(PointsTypes type)
    {
        switch (type)
        {
            case PointsTypes.PointsInManaRegen:
                if(UsedskillPoints < skillPoints)
                    PointsInManaRegen++;
                break;
            case PointsTypes.PointsInMana:
                if (UsedskillPoints < skillPoints)
                    PointsInMana++;
                break;
            case PointsTypes.PointsInHealth:
                if (UsedskillPoints < skillPoints)
                    PointsInHealth++;
                break;
            case PointsTypes.PointsInHealthRegen:
                if (UsedskillPoints < skillPoints)
                    PointsInHealthRegen++;
                break;
            case PointsTypes.PointsInMovementSpeed:
                if (UsedskillPoints < skillPoints)
                    PointsInMovementSpeed++;
                break;
            case PointsTypes.PointsInSpellSpeed:
                if (UsedskillPoints < skillPoints)
                    PointsInSpellSpeed++;
                break;
            case PointsTypes.PointsInDamage:
                if (UsedskillPoints < skillPoints)
                    PointsInDamage++;
                break;
        }
    }
    public void RevokePoint(PointsTypes type)
    {
        switch (type)
        {
            case PointsTypes.PointsInManaRegen:
                if (UsedskillPoints < skillPoints)
                    PointsInManaRegen++;
                break;
            case PointsTypes.PointsInMana:
                if (UsedskillPoints < skillPoints)
                    PointsInMana++;
                break;
            case PointsTypes.PointsInHealth:
                if (UsedskillPoints < skillPoints)
                    PointsInHealth++;
                break;
            case PointsTypes.PointsInHealthRegen:
                if (UsedskillPoints < skillPoints)
                    PointsInHealthRegen++;
                break;
            case PointsTypes.PointsInMovementSpeed:
                if (UsedskillPoints < skillPoints)
                    PointsInMovementSpeed++;
                break;
            case PointsTypes.PointsInSpellSpeed:
                if (UsedskillPoints < skillPoints)
                    PointsInSpellSpeed++;
                break;
            case PointsTypes.PointsInDamage:
                if (UsedskillPoints < skillPoints)
                    PointsInDamage++;
                break;
        }
    }
    public float GetRedusedValue(float value, float divider)
    {
        if (value >= 1)
            return (value + 2) / divider;
        else
            return 1;
    }
    public static void AddXp(float value)
    {
        //wizard.xp += value * (wizard.xp / 10);
        wizard.xp += value;
    }
    public TileBase GetZTile(Vector3 position, Tilemap tilemap)
    {
            Vector3 wp = position;

            for (int z = 0; z < 20; z++)
            {
                wp.z = z / 2;
                TileBase tile = tilemap.GetTile(tilemap.WorldToCell(wp));
                if (tile != null)
                    return tile;
        }
        return null;
    }
    public void UseSpell()
    {
        float m = 0;
        switch (fierball.GetComponent<Projectile>().effect)
        {
            case ProjectileEffect.damage:
                m = 10;
                break;
            case ProjectileEffect.fire:
                m = 15;
                break;
            case ProjectileEffect.explosion:
                m = 25;
                break;
            case ProjectileEffect.stun:
                m = 20;
                break;
        }
        if (Input.GetMouseButton(0) && mana > m)
        {
            mana -= m;
            Vector3 look = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject g = Instantiate(fierball, new Vector3(wand.position.x, wand.position.y, -3), Quaternion.identity);
            g.transform.LookAt(new Vector3(look.x, look.y, 0));
            g.transform.Rotate(new Vector3(0, 90, 180));
            Destroy(g, 10);

            //GameObject g = Instantiate(fierball, new Vector3(wand.position.x, wand.position.y, -3), Quaternion.identity);
            //g.transform.right = lastMovedir;
            //Destroy(g, 10);}
        }
    }
        [System.Serializable]
    public class MovePoint
    {
        public bool up;
        public Vector3Int position;
        public int layer;
        public MovePoint(Vector3Int Position, int Layer, bool Up)
        {
            position = Position;
            layer = Layer;
            up = Up;
        }
    }
    void TerainSetup()
    {
        print(map + " " + map.cellBounds);
        for (int x = 0; x < map.cellBounds.size.x; x++)
        {
            for (int y = 0; y < map.cellBounds.size.y; y++)
            {
                if (map != null)
                {
                    TileBase tile = map.GetTile(new Vector3Int(map.cellBounds.position.x + x, 0, map.cellBounds.position.y));
                    print(tile);
                    if (tile != null)
                    {
                        if (tile.name == "Tile Up" || tile.name == "Tile Down")
                        {
                            layerChange.Add(new MovePoint(new Vector3Int(map.cellBounds.position.x + x, 0, map.cellBounds.position.y + y), map.GetComponent<TilemapRenderer>().sortingOrder, tile.name == "Tile Up"));
                            map.SetTile(new Vector3Int(map.cellBounds.position.x + x, 0, map.cellBounds.position.y + y), Ramp);
                        }
                        //map.RefreshTile(new Vector3Int(x, y, 0));
                    }
                    else
                    {
                        map.SetTile(new Vector3Int(map.cellBounds.position.x + x, 0, map.cellBounds.position.y + y), Ramp);//
                    }
                }
            }
        }
    }
}
