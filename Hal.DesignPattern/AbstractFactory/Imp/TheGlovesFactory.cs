﻿using Hal.DesignPattern.Models;

namespace Hal.DesignPattern.AbstractFactory
{
    public class TheJiangXiFactory : TheAbstractFactory
    {
        public override Gloves GetGloves()
        {
            throw new System.NotImplementedException();
        }

        public override Knife GetKnife()
        {
            throw new System.NotImplementedException();
        }

        public override Shoes GetShoes()
        {
            throw new System.NotImplementedException();
        }
    }
}