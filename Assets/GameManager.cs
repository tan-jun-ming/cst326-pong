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

    public int winning_score = 11;

    private int p1_score;
    private int p2_score;

    private LetterBoard p1_score_display;
    private LetterBoard p2_score_display;
    private LetterBoard letterboard;
    private BallCollide ball;
    private PaddleMove paddle1;
    private PaddleMove paddle2;

    private int delay_counter;
    private int phase = 0;

    // Start is called before the first frame update
    void Start()
    {
        p1_score_display = (LetterBoard)p1_score_obj.GetComponent(typeof(LetterBoard));
        p2_score_display = (LetterBoard)p2_score_obj.GetComponent(typeof(LetterBoard));
        letterboard = (LetterBoard)letterboard_obj.GetComponent(typeof(LetterBoard));
        ball = (BallCollide)ball_obj.GetComponent(typeof(BallCollide));
        paddle1 = (PaddleMove)paddle1_obj.GetComponent(typeof(PaddleMove));
        paddle2 = (PaddleMove)paddle2_obj.GetComponent(typeof(PaddleMove));

        p1_score = 0;
        p2_score = 0;

        delay_counter = 30;
        ball.delay(delay_counter);

    }

    // Update is called once per frame
    void Update()
    {
        switch (phase)
        {
            case 0: phase_one();  break;
            case 1: phase_two(); break;
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

    void game_end()
    {
        phase++;

        hide_score();
        paddle1.lock_paddle();
        paddle2.lock_paddle();
        ball.end_game();

        if (p1_score >= winning_score)
        {
            letterboard.display_message("player 1 wins");
        }
        else if (p2_score >= winning_score)
        {
            letterboard.display_message("player 2 wins");
        }

    }

    public void round_over(string wall)
    {
        int dir = 0;
        if (wall == "wall left")
        {
            dir = -1;
            p2_score++;
        }
        else if (wall == "wall right")
        {
            dir = 1;
            p1_score++;
        }

        display_score();
        paddle1.reset();
        paddle2.reset();

        delay_counter = 30;
        ball.delay(delay_counter);
        ball.reset(dir);


    }
    void display_score()
    {
        p1_score_display.display_message(p1_score.ToString());
        p2_score_display.display_message(p2_score.ToString());
    }

    void hide_score()
    {
        p1_score_display.clear();
        p2_score_display.clear();
    }
}
