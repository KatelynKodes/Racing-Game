﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MathLibrary;
using Raylib_cs;
using System.Diagnostics;

namespace MathForGames
{
    class Engine
    {
        public static bool _applicationShouldClose = false;
        private static int _currentSceneIndex = 0;
        private static Scene[] _scenes = new Scene[0];
        private static string _winnerName = "";
        private Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        /// Called to begin the application
        /// </summary>
        public void Run()
        {
            //Call start for the entire application
            Start();

            float currTime = 0;
            float lastTime = 0;
            float deltaTime = 0;

            //Loop until application is told to close
            while (!Raylib.WindowShouldClose())
            {
                //Get how much time has passed since the application started
                currTime = _stopwatch.ElapsedMilliseconds / 1000;

                //Set deltatime to be the difference in time from the last time recorded to the current time recorded
                deltaTime = currTime - lastTime;

                Update(deltaTime);
                Draw();
                Thread.Sleep(150);

                //Set the last time recorded to be the current time
                lastTime = currTime;
            }

            //Calll at the end of the entire application.
            End();
        }

        /// <summary>
        /// Called when the application starts
        /// </summary>
        private void Start()
        {
            _stopwatch.Start();

            //Create Window using raylib
            Raylib.InitWindow(800, 450, "MathForgames");

            Scene RaceScene = new Scene();

            /* Finish lines 
            Actor FinishLine1 = new Actor('|', new Vector2 { X = 115, Y = 0 }, Color.GREEN, "FinishLine");
            Actor FinishLine2 = new Actor('|', new Vector2 { X = 115, Y = 1 }, Color.GREEN, "FinishLine2");
            Actor FinishLine3 = new Actor('|', new Vector2 { X = 115, Y = 2 }, Color.GREEN, "FinishLine3");
            Actor FinishLine4 = new Actor('|', new Vector2 { X = 115, Y = 3 }, Color.GREEN, "FinishLine4");
            Actor FinishLine5 = new Actor('|', new Vector2 { X = 116, Y = 0 }, Color.GREEN, "FinishLine5");
            Actor FinishLine6 = new Actor('|', new Vector2 { X = 116, Y = 1 }, Color.GREEN, "FinishLine6");
            Actor FinishLine7 = new Actor('|', new Vector2 { X = 116, Y = 2 }, Color.GREEN, "FinishLine7");
            Actor FinishLine8 = new Actor('|', new Vector2 { X = 116, Y = 3 }, Color.GREEN, "FinishLine8");

            //Racers
            Racer Racer1 = new Racer('S', 0, 0, 3, Color.YELLOW, "Selby");
            Racer Racer2 = new Racer('A', 0, 1, 4, Color.RED, "Adrien");
            Racer Racer3 = new Racer('D', 0, 2, 2, Color.DARKBLUE, "Dianne");*/

            //Player
            Player RacerPlayer = new Player('@', 0, 3, 2, Color.SKYBLUE, "Player");

            //Adds all actors to the race scene
            /*RaceScene.AddActor(FinishLine1);
            RaceScene.AddActor(FinishLine2);
            RaceScene.AddActor(FinishLine3);
            RaceScene.AddActor(FinishLine4);
            RaceScene.AddActor(FinishLine5);
            RaceScene.AddActor(FinishLine6);
            RaceScene.AddActor(FinishLine7);
            RaceScene.AddActor(FinishLine8);
            RaceScene.AddActor(Racer1);
            RaceScene.AddActor(Racer2);
            RaceScene.AddActor(Racer3);*/
            RaceScene.AddActor(RacerPlayer);

            //Sets up the scene array
            _scenes = new Scene[] { RaceScene };

            //Starts the current scene
            _scenes[_currentSceneIndex].Start();

            //Sets cursor to be invisible
            Console.CursorVisible = false;
        }

        /// <summary>
        /// Called everytime the game loops
        /// </summary>
        private void Update(float deltaTime)
        {
            _scenes[_currentSceneIndex].Update(deltaTime);

            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Called At the end of the application
        /// </summary>
        private void End()
        {
            _scenes[_currentSceneIndex].End();
            Raylib.CloseWindow();
            Console.Clear();
            Console.WriteLine(_winnerName + "Is the winner of the race");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Called everytime the game loops to update visuals
        /// </summary>
        private void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);

            //Adds all actor icons to buffer
            _scenes[_currentSceneIndex].Draw();

            Raylib.EndDrawing();
        }

        /// <summary>
        /// Adds a new scene to the engines scene array
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public int AddScene(Scene scene)
        {
            //Create Temporary Array
            Scene[] TempArray = new Scene[_scenes.Length + 1];

            //Copy all values into temporary array
            for (int i = 0; i < _scenes.Length; i++)
            {
                TempArray[i] = _scenes[i];
            }

            //Set the last index to be the new scene
            TempArray[_scenes.Length] = scene;

            //set the old array to the new array
            _scenes = TempArray;

            //Return the last index
            return _scenes.Length - 1;
        }


        /// <summary>
        /// Gets the next key in the input stream
        /// </summary>
        /// <returns>The key that was pressed</returns>
        public static ConsoleKey GetConsoleKey()
        {
            //If there is No Key being pressed...
            if (!Console.KeyAvailable)
            {
                //...Return
                return 0;
            }

            //Return the current key being pressed
            return Console.ReadKey(true).Key;
        }

        //Closes the Application
        public static void CloseApplication()
        {
            _applicationShouldClose = true;
        }

        public static void ChangeWinnerName(string newName)
        {
            _winnerName = newName;
        }

        /// <summary>
        /// Knocks an opposing actor back a space if the player collides with it
        /// </summary>
        /// <param name="opponent"></param>
        public static void KnockOpponentBack(Actor opponent)
        {
            //How much actor is knocked back
            Vector2 KnockbackValue = new Vector2 { X = 1, Y = 0 };

            //Subtract the current position by that knockback value
            opponent.GetPosition -= KnockbackValue;
        }
    }
}
