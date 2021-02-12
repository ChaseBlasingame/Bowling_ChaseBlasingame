using System;
using System.Collections.Generic;
using System.Linq;

namespace Bowling.Models
{
    public class Game
    {
        private const int NumFrames = 10;
        private static readonly List<Frame> FramesPendingBonus = new List<Frame>();
        private static readonly Random Rand = new Random();

        public int TotalScore => Frames.Sum(frame => frame.TotalScore);
        public readonly Frame[] Frames = new Frame[NumFrames];

        public Game()
        {
            InitializeFrames();
            GenerateBowlScore();
        }

        #region Initialize

        private void InitializeFrames()
        {
            for (var i = 0; i < Frames.Length; i++)
            {
                var isLastFrame = (i == Frames.Length - 1);
                Frames[i] = Frame.StartFrame(isLastFrame);
            }
        }

        private void GenerateBowlScore()
        {
            foreach (var frame in Frames)
            {
                while (frame.CanBowl())
                {
                    var bowlScore = Bowl(frame.PinsRemaining());
                    frame.AddScore(bowlScore);

                    AddBonus(bowlScore);
                    if (frame.IsPendingBonus())
                    {
                        FramesPendingBonus.Add(frame);
                    }
                }
            }
        }

        #endregion

        private static int Bowl(int pinsRemaining)
        {
            const int perfectRollChance = 2; // Skews for a perfect roll, i.e. a strike or spare. Chance will be 1/(perfectRollChance + 1)
            var perfectRoll = Rand.Next(perfectRollChance + 1);
            if (perfectRoll == perfectRollChance) return pinsRemaining;

            return (Rand.Next(pinsRemaining + 1));
        }

        private static void AddBonus(int score)
        {
            var removeFrames = new List<Frame>();
            foreach (var frame in FramesPendingBonus)
            {
                frame.AddBonus(score);
                if (!frame.IsPendingBonus())
                    removeFrames.Add(frame);
            }

            foreach (var frame in removeFrames)
            {
                FramesPendingBonus.Remove(frame);
            }
        }
    }
}