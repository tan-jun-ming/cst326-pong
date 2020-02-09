using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject ball_obj;
    public GameObject p1_score_obj;
    public GameObject p2_score_obj;
    public GameObject letterboard_obj;
    public GameObject paddle1_obj;
    public GameObject paddle2_obj;
    public PowerupManager powerup_manager;

    public int winning_score = 11;

    private int p1_score;
    private int p2_score;

    private LetterBoard p1_score_display;
    private LetterBoard p2_score_display;
    private LetterBoard letterboard;
    private BallCollide ball;
    private PaddleMove paddle1;
    private PaddleMove paddle2;

    private float hue_delta = 0.1f;

    private int delay_counter;
    private int phase;

    private const int powerup_delay_max = 600;
    private int powerup_delay = powerup_delay_max;

    // Start is called before the first frame update
    void Start()
    {
        p1_score_display = (LetterBoard)p1_score_obj.GetComponent(typeof(LetterBoard));
        p2_score_display = (LetterBoard)p2_score_obj.GetComponent(typeof(LetterBoard));
        letterboard = (LetterBoard)letterboard_obj.GetComponent(typeof(LetterBoard));
        ball = (BallCollide)ball_obj.GetComponent(typeof(BallCollide));
        paddle1 = (PaddleMove)paddle1_obj.GetComponent(typeof(PaddleMove));
        paddle2 = (PaddleMove)paddle2_obj.GetComponent(typeof(PaddleMove));

        game_init();
    }

    void game_init()
    {
        powerup_manager.hide();

        p1_score = 0;
        p2_score = 0;

        phase = 0;

        hide_score();
        letterboard.display_message("game start");

        paddle1.unlock_paddle();
        paddle2.unlock_paddle();

        delay_counter = 60;
        ball.delay(delay_counter);
    }

    // Update is called once per frame
    void Update()
    {
        switch (phase)
        {
            case 0: phase_one();  break;
            case 1: phase_two(); break;
            case 2: phase_three(); break;
            default: break;
        }
    }

    void phase_one()
    {
        if (delay_counter > 0)
        {
            delay_counter--;
            return;
        }

        // Phase 1 ends, hide title and show score
        letterboard.clear();
        display_score();
        phase++;
    }

    void phase_two()
    {
        if (powerup_delay > 0)
        {
            powerup_delay--;
        } else
        {
            powerup_manager.show();
            powerup_delay = powerup_delay_max;
        }

        if (delay_counter > 0)
        {
            delay_counter--;
            return;
        }

        // Check for winners
        if (p1_score >= winning_score || p2_score >= winning_score)
        {
            game_end();
        }
    }

    void phase_three()
    {
        if (delay_counter > 0)
        {
            delay_counter--;
            return;
        }

        game_init(); // Reset game
    }

    void game_end()
    {
        phase++;

        delay_counter = 60;
        ball.delay(delay_counter);

        hide_score();
        paddle1.lock_paddle();
        paddle2.lock_paddle();

        int winner = 0;
        if (p1_score >= winning_score)
        {
            letterboard.display_message("player 1 wins");
            winner = 1;
        }
        else if (p2_score >= winning_score)
        {
            
            winner = 2;
        }

        string msg = string.Format("Player {0} wins", winner);

        letterboard.display_message(msg);
        Debug.Log(msg);

    }

    public void round_over(string wall)
    {
        int dir = 0;
        if (wall == "wall left")
        {
            dir = -1;
            p2_score++;

            Debug.Log("Player 2 scored");
        }
        else if (wall == "wall right")
        {
            dir = 1;
            p1_score++;

            Debug.Log("Player 1 scored");
        }

        powerup_manager.hide();
        powerup_delay = powerup_delay_max;
        display_score();
        paddle1.reset();
        paddle2.reset();

        delay_counter = 30;
        ball.delay(delay_counter);
        ball.reset(dir);


    }

    Color calculate_score_color(int score)
    {
        return score == 0 ? Color.black : Color.HSVToRGB((float)((score-1) * hue_delta) % 1, 1, 1);
    }
    void display_score()
    {

        p1_score_display.display_message(p1_score.ToString(), calculate_score_color(p1_score));
        p2_score_display.display_message(p2_score.ToString(), calculate_score_color(p2_score));

        Debug.Log(string.Format("{0} - {1}", p1_score, p2_score));
    }

    void hide_score()
    {
        p1_score_display.clear();
        p2_score_display.clear();
    }

    public void set_powerup(int player, int powerup)
    {
        PaddleMove paddle = paddle1;
        if (player == 2)
        {
            paddle = paddle2;
        }
        paddle.set_affliction(powerup);

        powerup_delay = powerup_delay_max;
    }
}
