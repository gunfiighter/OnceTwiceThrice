using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnceTwiceThrice
{
    public static class Strong
    {
        private static HashSet<Tuple<Type, Type>> strong;
        static Strong()
        {
            strong = new HashSet<Tuple<Type, Type>>(new [] {
                Tuple.Create(typeof(DinoMob), typeof(CactusMob)),
                Tuple.Create(typeof(DinoMob), typeof(TermiteMob)),
                Tuple.Create(typeof(DinoMob), typeof(FrogMob)),
                Tuple.Create(typeof(DinoMob), typeof(PenguinMob)),
                Tuple.Create(typeof(DinoMob), typeof(RedGolemMob)),
                Tuple.Create(typeof(DinoMob), typeof(BlueGolemMob)),
                Tuple.Create(typeof(DinoMob), typeof(MonkeyMob)),
                Tuple.Create(typeof(DinoMob), typeof(DinoMob)),

                Tuple.Create(typeof(CactusMob), typeof(BlueGolemMob)),
                Tuple.Create(typeof(CactusMob), typeof(RedGolemMob)),
                Tuple.Create(typeof(CactusMob), typeof(FrogMob)),
                Tuple.Create(typeof(CactusMob), typeof(PenguinMob)),
                Tuple.Create(typeof(CactusMob), typeof(TermiteMob)),

                Tuple.Create(typeof(FrogMob), typeof(PenguinMob)),
                Tuple.Create(typeof(FrogMob), typeof(TermiteMob)),

                Tuple.Create(typeof(HotGuyMob), typeof(DinoMob)),
                Tuple.Create(typeof(HotGuyMob), typeof(RedGolemMob)),
                Tuple.Create(typeof(HotGuyMob), typeof(BlueGolemMob)),
                Tuple.Create(typeof(HotGuyMob), typeof(PenguinMob)),
                Tuple.Create(typeof(HotGuyMob), typeof(MonkeyMob)),
                Tuple.Create(typeof(HotGuyMob), typeof(HotGuyMob)),
                Tuple.Create(typeof(HotGuyMob), typeof(TermiteMob)),

                Tuple.Create(typeof(SharkMob), typeof(PenguinMob)),

                Tuple.Create(typeof(TermiteMob), typeof(MonkeyMob)),
                Tuple.Create(typeof(TermiteMob), typeof(BlueGolemMob)),
                Tuple.Create(typeof(TermiteMob), typeof(RedGolemMob)),
                Tuple.Create(typeof(TermiteMob), typeof(PenguinMob))
            });
        }

        public static bool CanKill(IMob assaulter, IMob victim)
        {
            return strong.Contains(Tuple.Create(assaulter.GetType(), victim.GetType()));
        }
    }
}
