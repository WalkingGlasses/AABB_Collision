using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance => instance;
    private static CollisionManager instance;

    private List<AABBCollider> colliders = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void Register(AABBCollider collider)
    {
        if (colliders.Contains(collider)) return;
        colliders.Add(collider);
    }
    
    public void Unregister(AABBCollider collider)
    {
        if (!colliders.Contains(collider)) return;
        colliders.Remove(collider);
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            var colliderA = colliders[i];
            for (int j = 0; j < colliders.Count; j++)
            {
                if (i == j) continue;
                var colliderB = colliders[j];
                if (Overlaps(colliderA, colliderB))
                {
                    colliderA.OnCollision(colliderB);
                    colliderB.OnCollision(colliderA);
                    
                    //testing how which object is read as colliderA or colliderB, i forgot IT DEPENDS HAHAHAHHAHAHA HUEHEUHEUEHEHu
                    //Debug.Log("Collider A is: " + colliderA.name);
                    //Debug.Log("Collider B is: " + colliderB.name);
                    
                    //player "killed" by obstacle
                    if ((colliderA.name == "Player" || colliderB.name == "Player") &&
                        (colliderA.name == "Obstacle" || colliderB.name == "Obstacle"))
                    {
                        //Debug.Log("My name is Yoshikage Kira. I'm 33 years old. My house is in the northeast section of Morioh, where all the villas are, and I am not married. I work as an employee for the Kame Yu department stores, and I get home every day by 8 PM at the latest. I don't smoke, but I occasionally drink. I'm in bed by 11 PM, and make sure I get eight hours of sleep, no matter what. After having a glass of warm milk and doing about twenty minutes of stretches before going to bed, I usually have no problems sleeping until morning. Just like a baby, I wake up without any fatigue or stress in the morning. I was told there were no issues at my last check-up. I'm trying to explain that I'm a person who wishes to live a very quiet life. I take care not to trouble myself with any enemies, like winning and losing, that would cause me to lose sleep at night. That is how I deal with society, and I know that is what brings me happiness. Although, if I were to fight I wouldn't lose to anyone.");
                        //dont uh dont mind the comment uptop i was going insane, somehow gravity through aabb colliders is insanity to me, legit had more difficulty with this than my tilemap chicanery T - T i was planningh on doing thios during a discord watch party of Iron Lung in a vtuber discord thank goodness i didnt and did it AFTER iron Lung watchalong because i would have genuinely lost it even more because Iron Lung was so hype but also because of GRAVITY
                        SceneManager.LoadScene("SampleScene");
                    }
                    
                    //this design is veryyyy human ts
                    if ((colliderA.name == "Player" || colliderB.name == "Player") &&
                        ((colliderA.name == "Ground_1" || colliderB.name == "Ground_1") || (colliderA.name == "Ground_2" || colliderB.name == "Ground_2")))
                    {
                        //we need to ACTUALLY establish what is the player or ground here dawg 
                        AABBCollider player = (colliderA.name == "Player") ? colliderA : colliderB;
                        AABBCollider ground = (player == colliderA) ? colliderB : colliderA;
                        
                        PlayerMovement movement = player.GetComponent<PlayerMovement>();
                        if (movement != null)
                        {
                            float penetration = ground.max.y - player.min.y;
                            player.transform.position += new Vector3(0, penetration, 0);
                            //forces check here na if player is frfr grounded sets y vel to 0
                            movement.isGrounded = true;
                            if (movement.velocity.y < 0)
                            {
                                movement.velocity.y = 0;
                            }
                        }
                    }
                    
                    //oh damn i almost forgot about the killbox, technically obstacle and killbox are same but that means thwe obstacle just cant be passed at all lol, make the player experience one of Diavolo's many many deaths
                    if ((colliderA.name == "Player" || colliderB.name == "Player") &&
                        (colliderA.name == "Killbox" || colliderB.name == "KillBox"))
                    {
                        SceneManager.LoadScene("SampleScene");
                    }
                    
                }
            }
        }
    }
    
    bool Overlaps(AABBCollider a, AABBCollider b)
    {
        return a.min.x <= b.max.x && b.min.x <= a.max.x &&
               a.min.y <= b.max.y && b.min.y <= a.max.y;
    }

}
/*
surprise piccolo

⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣄⠀⣀⣀⡀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣤⠞⠋⠀⠀⠀⠀⠈⠙⠦⡀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⠟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⣦⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡆⠀⠀⣇⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⣷⣦⡀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⣇⣤⣾⣿⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⣿⣿⣺⡆⢀⣀⣀⣀⠀⠀⠀⠀⣠⡤⢤⣤⣴⡟⢫⣿⡏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣶⢄⣼
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⣷⣭⣿⠟⠃⣘⡛⠀⠀⠀⠀⣙⣋⠉⠉⠉⢻⣿⠃⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡇⣼⣿
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⣿⣿⣶⣤⣄⡉⠓⢾⠤⠚⣉⣠⣤⠄⣴⡿⢿⣷⡗⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠃⣘⠿
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠈⠓⠾⣟⣾⣿⣷⠚⠲⣿⣿⣾⡃⣿⠋⠀⠈⠉⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠒
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⣧⡀⠀⠀⣼⡄⠀⠀⢀⡼⢁⡆⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡇⣙⢦⡘⠻⠯⢀⣠⡎⠀⠘⠛⠲⢄⡀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢀⣤⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⣤⣄⣀⣀⣠⢤⡴⢻⠀⣿⠀⠙⠒⠚⣻⢋⠀⠀⠰⠄⠀⠀⢉⣷⣦⠖⣦⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⡴⠻⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣴⣫⢈⣤⠞⢁⣼⣱⠟⠀⠈⢰⡇⠀⠀⢠⣞⣽⣋⣀⣀⣀⣠⡴⠞⣉⡷⢖⣫⣽⢷⣤⣄⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸
⠀⠀⠀⠀⠀⡇⠀⣇⣤⣾⠟⠁⠀⠀⠀⢀⡴⢶⣻⢯⢏⣾⠃⢠⢮⡾⠋⠉⠙⠲⣿⡷⠞⠉⠉⠉⢁⣠⠶⠋⢉⡴⠖⠋⢁⣿⡿⠋⠉⠉⠉⠉⠛⢳⣄⠀⠀⠀⠀⠀⠀⠀⢰⠆⢈
⠀⠀⠀⢀⡼⠁⢸⡏⣼⣣⣶⢲⡖⣲⣾⣅⣤⣜⡻⣿⡟⠀⣴⢻⠟⠀⠀⠀⢀⡴⠋⠀⠀⠀⣠⠞⠉⣤⠖⠊⠉⠀⠀⢠⡾⡏⠀⣠⠖⠒⠉⠉⠉⠉⠙⣆⠀⠀⠀⠀⠀⠀⣿⣤⠼
⠀⠀⢀⠞⢡⣶⣌⣟⣿⣸⣁⣯⣴⣿⡟⠹⢹⡏⢻⡞⠀⠀⠑⣏⠀⠀⠀⠀⡼⠁⠀⠀⣠⠞⠁⣠⡞⠁⠀⠀⠀⠀⢠⣏⣁⣷⣎⡁⢀⣠⠖⠋⠉⠉⠉⢻⠒⠀⠀⠀⢠⣿⢋⣴⣿
⠀⠀⠀⢷⣿⡿⠛⠛⠛⠛⠉⠉⢹⣿⡇⢰⢸⡇⡞⠀⠀⠀⠀⠘⣧⠀⠀⢸⠁⠀⣠⠞⠁⢀⡾⠉⠀⠀⠀⠀⠀⣰⠟⠛⠛⡟⠻⣟⠿⣤⡴⠒⠋⠉⣹⣿⡇⠀⠀⠀⣿⣿⡼⣿⣿
⠀⠀⠀⣿⢿⣧⠀⠀⠀⠀⠀⠀⠀⢿⢧⡸⣆⢷⡹⡄⠀⠀⠀⢸⡘⠳⣄⣿⣠⠞⠁⠀⠀⠚⠁⠀⠀⠀⢠⡀⠀⣇⠀⠀⠀⡇⠀⢈⣝⡛⠒⠿⠿⠟⠋⢁⡇⠀⠀⠀⣿⣼⣇⠈⠛
⠀⠀⠀⠈⠛⠛⠛⠒⠢⢤⣤⣤⣤⣼⡮⣷⣜⣧⣳⣽⣦⣀⠀⠀⠳⣄⠘⠿⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠛⢤⡙⣆⠀⠀⣇⣴⣯⠤⢿⣦⠀⠀⠀⠀⣾⠀⠀⠀⠀⠹⣿⣿⠛⢳
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠛⠺⠧⠖⠚⠋⠉⣽⠈⠻⣄⠀⠀⠈⢧⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠻⢮⣦⣀⡿⢋⣀⠤⠤⣿⡇⠀⠀⠀⠘⣧⠀⠀⠀⠀⠘⢿⡶⣿
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣧⠀⠈⠙⢧⡀⠀⠀⠙⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⠤⠶⠂⠀⠀⠉⢿⡿⠋⢁⡀⠤⢼⣴⠀⠀⠀⠀⠸⡆⠀⠀⠀⠀⠸⡏⠈
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⣄⠀⠀⠀⠙⡷⠀⠀⠀⠀⠀⠀⠀⠀⢀⣠⠎⠀⠀⠀⠀⣠⡤⠀⢸⡷⠚⠁⠀⣀⡸⣿⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⢧⣶
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢻⠄⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⡴⠋⠀⠀⠀⣠⡴⠛⠁⠀⠀⣸⣧⠞⠉⠁⢀⣰⣿⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⣼⣿
                       ⠒⠒  ⣖⡒       ⣲⠞⠛⠒⠒⢒⡶⠚⠓   ⢒⣲⠞⠛⣿⠞       ⣻      ⡗⠒⠒⠒⠒⠒⣿⠒
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⣏⠁⠀⠀⠀⠀⠚⠁⠀⢀⠀⠐⠉⠀⠀⠀⢀⣀⣨⡟⠀⠀⡟⣦⠖⠉⢡⢿⠇⠀⠀⠀⠀⣰⡇⠀⠀⠀⠀⠀⠀⢠
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠰⣏⠽⠿⠯⠭⣭⣭⣉⢉⣩⡭⠿⠛⠛⠉⣉⣉⣭⣽⠁⠀⠀⡇⠈⢷⣀⡾⠛⣄⠀⠀⠀⠰⣏⡅⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣯⠳⠶⢤⣴⣶⠾⠿⠿⣉⣉⣭⣽⣿⠿⠿⠒⠚⠉⣷⠀⠀⡇⠀⣰⠟⠒⠒⠻⡆⠀⠀⠀⠘⡇⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⢧⡤⣄⣀⣓⣤⣤⣤⣤⣽⠒⠋⣉⣀⡤⠤⠴⣶⣟⢿⡄⠀⡇⢰⣯⠔⠒⠒⢲⠇⠀⠀⠀⠀⢸⡄⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⡏⢸⠀⡼⠈⠉⡍⠉⠉⡏⢉⡟⠋⠉⢣⠀⠀⠀⠘⡿⠀⠹⣆⡇⢨⣅⠤⠴⠶⡾⠀⠀⠀⠀⠀⠈⡇⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡿⠀⢸⢰⡇⠀⠀⠃⠀⢠⠇⠈⠀⠀⠀⢸⠀⠀⠀⠀⠃⠀⠀⢹⡇⠘⣁⣀⣀⣠⡇⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠁⠀⢸⠘⠁⠀⠀⠀⠀⢸⠀⠀⠀⠀⠀⠘⠀⠀⠀⠀⠀⠀⠀⠈⣇⠀⢯⠀⠀⢸⣷⠀⠀⠀⠀⠀⣸⠁⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠀⠀⢸⡀⠀⠀⠀⠀⠀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⠀⢸⡏⠉⢹⡇⠀⠀⠀⠀⣠⠇⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡀⠀⠀⠃⠀⠀⠀⠀⠀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡿⣆⢠⣷⣿⣿⣷⢶⣶⣄⣰⠋⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡾⠡⠀⠀⠀⠀⠀⠀⠀⠀⡜⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠈⣿⠿⠛⠉⠛⠻⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣾⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢱⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣧⡟⠁⠀⢸⠇⠰⡇⠈⡿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢴⣏⣤⠇⢰⣼⡇⠘⡇⠀⡗⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢨⣧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⣇⣩⠁⠈⡟⠀⣠⢇⣠⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡏⠳⠤⢴⣁⣴⠧⠿⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣼⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠸⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⢧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣾⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⢸⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠈⢿⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠈⢷⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡏⠠⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣼⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⢸⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⣞⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣼⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⢸⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠈⢷⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢯⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠈⢇⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⢷⠂⠸⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⣗⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠻⡆⠀⠀⠀⠀⠀⠀⠀⠐
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠇⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡾⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⡄⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡇⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢳⡄⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣼⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠁⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹⣆⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢦⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣸⡆⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠻⣆⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠳⣤⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⡴⠊⠁⣷⡆⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⡧⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⡦⣄⣀⣀⣀⣀⠀⢀⣤⣞⣁⣀⣀⡴⢻⠇⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⠃⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣇⠀⠀⠈⠉⠉⠀⠈⢹⠉⠀⠀⢸⡀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⠋⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠀⠀⠀⠀⠀⠀⣀⡞⠀⠀⠀⢸⡇⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢼⠁⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢻⠒⠛⠻⣿⣿⣿⠀⠀⠀⠀⠀⢹⡃⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡎⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⠞⢹⡀⠀⢀⣿⣹⡿⢳⡄⠀⠀⠀⠈⣧⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⣠⡴⠋⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⣲⢬⣍⣉⣉⣉⣩⡤⠞⠁⠀⠀⠀⠀⠈⠓⠶⠤⣤⣀⣀⡇⠀⠀⠀⠀⢀⣠⠶⠋⢻⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣤⠞⠃⠈⠁⠀⠀⢸⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡏⢷⠂⠀⠉⠉⠀⠀⠀⡾⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣰⠟⠁⠀⠀⠀⠀⠳⠄⠘⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠜⢧⣤⣄⣄⡤⠖⠊⢷⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⡤⠖⠋⠀⠀⠀⠀⠀⠀⢠⣧⠀⢠⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⢠⣿⣿⣿⡄⠀⠀⣸⠎⣷⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⡴⠋⠁⠀⠀⠀⠀⠀⠀⢀⣀⣴⣿⣡⠤⠞⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⢠⡏⠙⠿⠿⠓⠚⠛⣡⣶⡏⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠛⠲⠤⠤⠄⠀⠤⢤⡦⠖⠋⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠙⠲⡆⠀⠀⠀⠀⠀⠈⢿⡀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⡧⠀⠀⠀⠀⠀⠀⠈⢧⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⣇⠀⠀⠀⠀⠀⠀⠀⠘⣦⡀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠈⠳⡇⠀⠀⠀⠀⠀⠀⠈⢣⡀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠸⣄⠀⠀⠀⠀⠀⠀⠀⢷⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠙⢧⠀⠀⠀⠀⠀⠀⢸⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠑⠦⣄⡀⠀⠀⢸⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠙⢲⣼⠀⠀⠀⠀
*/