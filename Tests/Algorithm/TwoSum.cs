using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests.Algorithm
{
    /// <summary>
    /// 给定一个整数数组 nums 和一个目标值 target，
    /// 请你在该数组中找出和为目标值的那 两个 整数，并返回他们的数组下标。
    /// 你可以假设每种输入只会对应一个答案。但是，数组中同一个元素不能使用两遍。
    /// Example 
    /// 给定 nums = [2, 7, 11, 15], target = 9
    /// 因为 nums[0] + nums[1] = 2 + 7 = 9
    /// 所以返回[0, 1]
    /// from：https://leetcode-cn.com/problems/two-sum
    /// </summary>
    public class TwoSum
    {
        [Fact]
        public void Test1()
        {
            var array = Enumerable.Range(1, 100).ToArray();
            //var output = new HashSet<(int a, int b)>();
            var output = new int[2];
            var target = 35;
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i + 1; j < array.Length - i - 1; j++)
                {
                    if (array[i] + array[j] == target)
                    {
                        //output.Add((i, j));
                        output[0] = i;
                        output[1] = j;
                    }
                }
            }

            var count = output.Length;
            count.ShouldBeGreaterThan(0);
        }
        
        [Fact]
        public void Test2()
        {
            var res = GetTwoSum(new int[] { 3, 2, 4 }, 6);
            res.Length.ShouldBeGreaterThan(0);
        }

        private int[] GetTwoSum(int[] nums, int target)
        {
            var output = new int[2];
            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = i + 1; j < nums.Length; j++)
                {
                    if (nums[i] + nums[j] == target)
                    {
                        output[0] = i;
                        output[1] = j;
                        return output;
                    }
                }
            }

            return output;
        }
    }
}
