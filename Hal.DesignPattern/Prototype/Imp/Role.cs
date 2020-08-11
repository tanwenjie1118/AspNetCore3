using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.DesignPattern.Prototype.Imp
{
    /// <summary>
    /// 对象必须实现 Clone 方法
    /// </summary>
    public class Role : BasicObject
    {
        public Role(string id) : base(id)
        {
        }

        public override BasicObject Clone()
        {
            // 浅拷贝
            return (BasicObject)this.MemberwiseClone();
            // 深拷贝
            //return (BasicObject) new Role();
        }
    }
}
