using System;
using UnityEngine;

namespace ARTech.GameFramework
{
    public interface ICharacter : ITransform, IHealth
    {


        bool CanSee(ICharacter character);
        ICharacter GetNearest(Predicate<ICharacter> match);

    }
}