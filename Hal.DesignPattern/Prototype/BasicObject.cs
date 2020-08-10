using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.DesignPattern.Prototype
{
    /// <summary>
    /// 原型模式的优点有：
    /// 1,原型模式向客户隐藏了创建新实例的复杂性
    /// 2,原型模式允许动态增加或较少产品类。
    /// 3,原型模式简化了实例的创建结构，工厂方法模式需要有一个与产品类等级结构相同的等级结构，而原型模式不需要这样。
    /// 4,产品类不需要事先确定产品的等级结构，因为原型模式适用于任何的等级结构
    /// 原型模式的缺点有：
    /// 1,每个类必须配备一个克隆方法
    /// 2,配备克隆方法需要对类的功能进行通盘考虑，这对于全新的类不是很难，但对于已有的类不一定很容易，特别当一个类引用不支持串行化的间接对象，或者引用含有循环结构的时候。
    /// </summary>
    public abstract class BasicObject
    {
        public string Id { get; set; }
        public BasicObject(string id)
        {
            this.Id = id;
        }

        public abstract BasicObject Clone();
    }
}
