using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests.Algorithm
{
    /// <summary>
    /// 给你 n 个非负整数 a1，a2，...，an，每个数代表坐标中的一个点 (i, ai) 。在坐标内画 n 条垂直线，垂直线 i 的两个端点分别为 (i, ai) 和 (i, 0)。找出其中的两条线，使得它们与 x 轴共同构成的容器可以容纳最多的水。
    /// 说明：你不能倾斜容器，且 n 的值至少为 2。
    /// 来源：力扣（LeetCode）
    /// 链接：https://leetcode-cn.com/problems/container-with-most-water
    /// 著作权归领扣网络所有。商业转载请联系官方授权，非商业转载请注明出处。
    /// </summary>
    public class ContainerWithMostWater
    {
        [Fact]
        public void Test1()
        {
            var arr = new int[] { 1, 8, 6, 2, 5, 4, 8, 3, 7 };
            var val = MaxArea(arr);
            val.ShouldBeGreaterThan(0);
        }

        public int MaxArea(int[] height)
        {
            var cache = new Dictionary<int, int>();
            for (int i = 1; i <= height.Length; i++)
            {
                cache.Add(i, height[i - 1]);
            }

            return DoCompare(cache);
        }

        private int DoCompare(Dictionary<int, int> cache)
        {
            var max = 0;
            var count = 0;
            var head = cache.FirstOrDefault().Key;
            var foot = cache.LastOrDefault().Key;
            while (true)
            {
                int temp;
                if (head == foot)
                {
                    return max;
                }
                count++;
                Debug.WriteLine("对比次数 == " + count);
                var first = cache[head];
                var last = cache[foot];
                if (first > last)
                {
                    temp = last * (foot - head);
                    foot--;
                }
                else
                {
                    temp = first * (foot - head);
                    head++;
                }

                if (temp > max)
                {
                    max = temp;
                }
            }
        }
    }
}
