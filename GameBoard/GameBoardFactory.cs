using UnityEngine;

    public class GameBoardFactory : MonoBehaviour
    {
        [SerializeField] private GameBoard TwoD;
        [SerializeField] private GameBoard ThreeD;
        [SerializeField] private GameBoardStyle Survaival;
        [SerializeField] private GameBoardStyle Laser;
        [SerializeField] private GameObject PlanLaser;
        [SerializeField] private GameObject LineLaser;
        
        public  GameBoard GetGameBoard(string View, string Style)
        {

            GameBoard gb;

            GameBoardStyle gbStyle;

            if (View == "ThreeD")

            {
                gb = Instantiate(ThreeD, transform.position, Quaternion.identity);


                if (Style == "Laser")

                {

                    gbStyle = Instantiate(Laser, transform.position, Quaternion.identity);

                    ((Laser)gbStyle).SetLaser(PlanLaser);

                    gb.SetStyle(gbStyle);

                    gbStyle.setBoard(gb);
                    
                //    DontDestroyOnLoad(gb);
                    
                //    DontDestroyOnLoad(gbStyle); 
                    return gb;

                }

                else if (Style == "Survival")

                {

                    gbStyle = Instantiate(Survaival, transform.position, Quaternion.identity);

                    gb.SetStyle(gbStyle);

                    gbStyle.setBoard(gb);
                    
                //    DontDestroyOnLoad(gb);
                    
                //    DontDestroyOnLoad(gbStyle);
                    return gb;
                }

            }

            else if (View == "TwoD")

            {

                gb = Instantiate(TwoD, transform.position, Quaternion.identity);

                if (Style == "Laser")

                {

                    gbStyle = Instantiate(Laser, transform.position, Quaternion.identity);

                    ((Laser)gbStyle).SetLaser(LineLaser);

                    gb.SetStyle(gbStyle);

                    gbStyle.setBoard(gb);
                    
                //    DontDestroyOnLoad(gb);
                //    DontDestroyOnLoad(gbStyle);
                    return gb;

                }

                else if (Style == "Survival")

                {

                    gbStyle = Instantiate(Survaival, transform.position, Quaternion.identity);

                    gb.SetStyle(gbStyle);

                    gbStyle.setBoard(gb);
                    
                //    DontDestroyOnLoad(gb);
                    
                //    DontDestroyOnLoad(gbStyle);
                    return gb;
                }

            }

            return null;

        }
    }