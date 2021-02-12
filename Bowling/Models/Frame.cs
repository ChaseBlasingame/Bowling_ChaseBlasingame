using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bowling.Models
{
    public class Frame
    {
        public const int PerfectScore = 10;
        public virtual int MaxBowls => 2;

        public List<int> Scores { get; } = new List<int>();
        public List<int> BonusScores { get; } = new List<int>();
        public int BowlingScore => Scores.Sum();
        public int TotalScore => BowlingScore + BonusScores.Sum();
        public bool IsStrike => _strikeIndexes.Any();
        public bool IsSpare => _spareIndex.HasValue;


        /// <summary>
        /// Call StartFrame method instead.
        /// </summary>
        protected Frame() { }

        public static Frame StartFrame(bool isLastFrame)
        {
            if (isLastFrame)
            {
                return new LastFrame();
            }

            return new Frame();
        }

        public void AddScore(int score)
        {
            Scores.Add(score);

            var scoreIndex = Scores.Count - 1;
            if (score == PerfectScore)
            {
                var prevIndex = scoreIndex - 1;
                if (prevIndex >= 0)
                {
                    if (_spareIndex == prevIndex || _strikeIndexes.Contains(prevIndex))
                    {
                        AddStrike(scoreIndex);
                    }
                    else
                    {
                        AddSpare(scoreIndex);
                    }
                }
                else
                {
                    AddStrike(scoreIndex);
                }
            }

            if (!IsSpare && Scores.Count >= 2)
            {
                var last2 = Scores.Skip(Math.Max(0, Scores.Count - 2));
                if (last2.Sum() == PerfectScore)
                    AddSpare(scoreIndex);
            }
        }
        
        public void AddBonus(int bonus)
        {
            BonusScores.Add(bonus);
        }

        private void AddStrike(int index)
        {
            _strikeIndexes.Add(index);
        }

        private void AddSpare(int index)
        {
            _spareIndex = index;
        }

        protected List<int> _strikeIndexes = new List<int>();
        public List<int> StrikeIndexes()
        {
            return _strikeIndexes;
        }

        protected int? _spareIndex { get; set; }
        public int? SpareIndex()
        {
            return _spareIndex;
        }

        public virtual bool CanBowl()
        {
            if (IsStrike) return false;
            if (Scores.Count == MaxBowls) return false;
            return PinsRemaining() > 0;
        }

        public virtual int PinsRemaining()
        {
            return PerfectScore - BowlingScore;
        }

        public virtual bool IsPendingBonus()
        {
            if (IsStrike && BonusScores.Count() < 2) return true;
            if (IsSpare && !BonusScores.Any()) return true;

            return false;
        }
    }

    public class LastFrame : Frame
    {
        public override int MaxBowls => 3;

        public override bool CanBowl()
        {
            var numBowls = Scores.Count();

            if (numBowls < 2) return true;
            if (numBowls == 2 && (IsStrike || IsSpare))
            {
                return true;
            }

            return false;
        }

        public override int PinsRemaining()
        {
            // Last frame
            switch (Scores.Count)
            {
                case 1:
                    if (IsStrike) return PerfectScore;
                    return base.PinsRemaining();
                case 2:
                    if (IsStrike && StrikeIndexes().Contains(1))
                        return PerfectScore;
                    if (IsSpare && SpareIndex() == 1)
                        return PerfectScore;
                    return base.PinsRemaining();
                default:
                    return base.PinsRemaining();
            }
        }

        public override bool IsPendingBonus()
        {
            return false;
        }
    }
}